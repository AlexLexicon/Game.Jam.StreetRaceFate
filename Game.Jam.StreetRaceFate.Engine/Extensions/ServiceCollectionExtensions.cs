using Game.Jam.StreetRaceFate.Engine.Builders;
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

        services.AddSingleton<IContentManagerService, ContentManagerService>();
        services.AddSingleton<IDrawablePriorityService,  DrawablePriorityService>();   
        services.AddSingleton<IGameService, GameService>();
        services.AddSingleton<IGameWindowService, GameWindowService>();
        services.AddSingleton<IGraphicsDeviceManagerService, GraphicsDeviceManagerService>();
        services.AddSingleton<IGraphicsDeviceProvider,  GraphicsDeviceProvider>();
        services.AddSingleton<IGraphicsService, GraphicsService>();
        services.AddSingleton<ISpriteBatchFactory, SpriteBatchFactory>();
        services.AddSingleton<IViewportService, ViewportService>();
        services.AddSingleton<IWeakRefrenceManager, WeakRefrenceManager>();

        services.AddGameObject<FramerateService>(ServiceLifetime.Singleton);
        services.AddGameObject<FramerateSpriteBatch>(options =>
        {
            options.SetDrawPriority(int.MaxValue);
        });
    }

    public static void AddGameObject<TGameObject>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        AddGameObject<TGameObject>(services, options =>
        {
            options.Lifetime = lifetime;
        });
    }
    public static void AddGameObject<TGameObject>(this IServiceCollection services, Action<GameObjectOptionsBuilder<TGameObject>> configure)
    {
        var builder = new GameObjectOptionsBuilder<TGameObject>(services);

        configure?.Invoke(builder);

        Type gameObjectType = typeof(TGameObject);
        services.Add(new ServiceDescriptor(gameObjectType, (sp) =>
        {
            object gameObject = ActivatorUtilities.CreateInstance(sp, gameObjectType);

            var weakRefrenceManager = sp.GetRequiredService<IWeakRefrenceManager>();
            if (gameObject is IGameDrawable gameDrawabley)
            {
                weakRefrenceManager.Add(gameDrawabley);
            }
            if (gameObject is IGameInitalizable gameInializable)
            {
                weakRefrenceManager.Add(gameInializable);
            }
            if (gameObject is IGameLoadable gameLoadable)
            {
                weakRefrenceManager.Add(gameLoadable);
            }
            if (gameObject is IGameUpdatable gameUpdatable)
            {
                weakRefrenceManager.Add(gameUpdatable);
            }

            Type spriteBatchDrawableType = typeof(ISpriteBatchDrawable<>);
            List<Type> spriteBatchDrawableGenericArgumentTypes = gameObjectType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == spriteBatchDrawableType)
                .Select(t => t.GetGenericArguments().First())
                .ToList();
            if (spriteBatchDrawableGenericArgumentTypes.Count is > 0)
            {
                foreach (Type spriteBatchDrawableGenericArgumentType in spriteBatchDrawableGenericArgumentTypes)
                {
                    Type interfaceType = spriteBatchDrawableType.MakeGenericType(spriteBatchDrawableGenericArgumentType);
                    weakRefrenceManager.Add(interfaceType, gameObject);
                }
            }

            return gameObject;
        }, builder.Lifetime));
    }

    public static void AddOrchestrer<TGameOrchestrer>(this IServiceCollection services) where TGameOrchestrer : Microsoft.Xna.Framework.Game
    {
        services.AddSingleton<Microsoft.Xna.Framework.Game, TGameOrchestrer>();
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
