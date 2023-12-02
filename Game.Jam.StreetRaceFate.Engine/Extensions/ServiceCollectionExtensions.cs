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
        services.AddSingleton<Microsoft.Xna.Framework.Game, GameAggregator>();
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

        services.AddSingleton<IContentManagerService, ContentManagerService>();
        services.AddSingleton<IGameService, GameService>();
        services.AddSingleton<IGameWindowService, GameWindowService>();
        services.AddSingleton<IGraphicsDeviceManagerService, GraphicsDeviceManagerService>();
        services.AddSingleton<IGraphicsDeviceProvider,  GraphicsDeviceProvider>();
        services.AddSingleton<IGraphicsService, GraphicsService>();
        services.AddSingleton<ISpriteBatchFactory, SpriteBatchFactory>();
        services.AddSingleton<IViewportService, ViewportService>();
        services.AddSingleton<IWeakRefrenceManager, WeakRefrenceManager>();
    }

    public static void AddGameObject<TGameObject>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        Type gameObjectType = typeof(TGameObject);
        services.Add(new ServiceDescriptor(gameObjectType, (sp) =>
        {
            var gameObject = ActivatorUtilities.CreateInstance(sp, gameObjectType);

            var weakRefrenceManager = sp.GetRequiredService<IWeakRefrenceManager>();
            if (gameObject is IGameDrawable drawableEntity)
            {
                weakRefrenceManager.Add(drawableEntity);
            }
            if (gameObject is IGameInitalizable inializableEntity)
            {
                weakRefrenceManager.Add(inializableEntity);
            }
            if (gameObject is IGameLoadable loadableEntity)
            {
                weakRefrenceManager.Add(loadableEntity);
            }
            if (gameObject is IGameUpdatable updatableEntity)
            {
                weakRefrenceManager.Add(updatableEntity);
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
        }, lifetime));
    }
}
