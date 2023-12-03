using Game.Jam.StreetRaceFate.Engine.Builders;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Jam.StreetRaceFate.Engine.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddEngine(this IServiceCollection services)
    {
        services.AddOrchestrer<GameOrchestrer>();

        services.AddSingleton<IGameObjectFactory, GameObjectFactory>();
        services.AddSingleton<ISceneFactory, SceneFactory>();
        services.AddSingleton<ISpriteBatchFactory, SpriteBatchFactory>();

        services.AddSingleton<IContentManagerService, ContentManagerService>();
        services.AddSingleton<IDelayService, DelayService>();
        services.AddSingleton<IDrawablePriorityService, DrawablePriorityService>();
        services.AddSingleton<IDrawService, DrawService>();
        services.AddSingleton<IGameService, GameService>();
        services.AddSingleton<IGameWindowService, GameWindowService>();
        services.AddSingleton<IGraphicsDeviceManagerService, GraphicsDeviceManagerService>();
        services.AddSingleton<IGraphicsDeviceProvider, GraphicsDeviceProvider>();
        services.AddSingleton<IGraphicsService, GraphicsService>();
        services.AddSingleton<IKeysService, KeysService>();
        services.AddSingleton<IMovementService, MovementService>();
        services.AddSingleton<IOscillateService>(sp =>
        {
            return sp.ActivateGameInstance<OscillateService>();
        });
        services.AddSingleton<IViewportService, ViewportService>();
        services.AddSingleton<IWeakRefrenceManager, WeakRefrenceManager>();

        services.AddGameObject<FramerateService>(options =>
        {
            options.AddToSpriteBatch<FramerateSpriteBatch>(options =>
            {
                options.SetDrawPriority(int.MaxValue);
            });
        });
    }

    public static void AddScene<TGameScene>(this IServiceCollection services) where TGameScene : class, IGameScene
    {
        services.AddTransient(sp =>
        {
            var weakRefrenceManager = sp.GetRequiredService<IWeakRefrenceManager>();

            var gameScene = (TGameScene)ActivatorUtilities.CreateInstance(sp, typeof(TGameScene));

            weakRefrenceManager.Add<IGameScene>(gameScene);

            return gameScene;
        });
    }

    public static void AddGameObject<TGameObject>(this IServiceCollection services, Action<DrawableGameObjectOptionsBuilder<TGameObject>>? configure = null, ServiceLifetime? lifetime = null) where TGameObject : IGameDrawable
    {
        var builder = new DrawableGameObjectOptionsBuilder<TGameObject>(services);

        configure?.Invoke(builder);

        if (lifetime is not null)
        {
            builder.Lifetime = lifetime.Value;
        }

        AddGameObject(services, builder);
    }
    public static void AddGameObject<TGameObject>(this IServiceCollection services, Action<SpriteBatchDrawableGameObjectOptionsBuilder<TGameObject>>? configure = null, ServiceLifetime? lifetime = null) where TGameObject : IGameObject, ISpriteBatchDrawable
    {
        var builder = new SpriteBatchDrawableGameObjectOptionsBuilder<TGameObject>(services);

        configure?.Invoke(builder);

        if (lifetime is not null)
        {
            builder.Lifetime = lifetime.Value;
        }

        AddGameObject(services, builder);
    }

    private static void AddGameObject<TGameObject>(this IServiceCollection services, GameObjectOptionsBuilder<TGameObject> builder) where TGameObject : IGameObject
    {
        Type gameObjectType = typeof(TGameObject);
        services.Add(new ServiceDescriptor(gameObjectType, (sp) =>
        {
            var weakRefrenceManager = sp.GetRequiredService<IWeakRefrenceManager>();

            object gameObject = sp.ActivateWeakGameInstance<TGameObject>(weakRefrenceManager);

            Type spriteBatchDrawableInterfaceType = typeof(ISpriteBatchDrawable<>);
            List<Type> spriteBatchDrawableGenericArgumentTypes = gameObjectType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == spriteBatchDrawableInterfaceType)
                .Select(t => t.GetGenericArguments().First())
                .ToList();
            if (spriteBatchDrawableGenericArgumentTypes.Count is > 0)
            {
                foreach (Type spriteBatchType in spriteBatchDrawableGenericArgumentTypes)
                {
                    var x = sp.GetRequiredService(spriteBatchType);
                    Type interfaceType = spriteBatchDrawableInterfaceType.MakeGenericType(spriteBatchType);
                    weakRefrenceManager.Add(interfaceType, gameObject);
                }
            }

            return gameObject;
        }, builder.Lifetime));
    }

    public static void AddOrchestrer<TGameOrchestrer>(this IServiceCollection services) where TGameOrchestrer : Microsoft.Xna.Framework.Game, IGameOrchestrer
    {
        services.AddSingleton<TGameOrchestrer>();
        services.AddSingleton<IGameOrchestrer>(sp =>
        {
            return sp.GetRequiredService<TGameOrchestrer>();
        });
        services.AddSingleton<Microsoft.Xna.Framework.Game>(sp =>
        {
            return sp.GetRequiredService<TGameOrchestrer>();
        });
        services.AddSingleton(sp =>
        {
            var game = sp.GetRequiredService<Microsoft.Xna.Framework.Game>();

            return new GraphicsDeviceManager(game);
        });
        services.AddSingleton(sp =>
        {
            return sp.GetRequiredService<Microsoft.Xna.Framework.Game>().Content;
        });
        services.AddSingleton(sp =>
        {
            return sp.GetRequiredService<Microsoft.Xna.Framework.Game>().Window;
        });
    }
}
