using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

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
        services.AddSingleton<IWeakRefrenceManager, WeakRefrenceManager>();
        services.AddSingleton<IGameService, GameService>();
        services.AddSingleton<IGameWindowService, GameWindowService>();
        services.AddSingleton<IGraphicsDeviceManagerService, GraphicsDeviceManagerService>();
        services.AddSingleton<IGraphicsDeviceProvider,  GraphicsDeviceProvider>();
        services.AddSingleton<IGraphicsService, GraphicsService>();
        services.AddSingleton<IViewportService, ViewportService>();
    }

    public static void AddEntity<TEntity>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.Add(new ServiceDescriptor(typeof(TEntity), (sp) =>
        {
            var entity = ActivatorUtilities.CreateInstance(sp, typeof(TEntity));
            var entityAggregator = sp.GetRequiredService<IWeakRefrenceManager>();

            if (entity is IGameDrawable drawableEntity)
            {
                entityAggregator.Add(drawableEntity);
            }
            if (entity is IGameInitalizable inializableEntity)
            {
                entityAggregator.Add(inializableEntity);
            }
            if (entity is IGameLoadable loadableEntity)
            {
                entityAggregator.Add(loadableEntity);
            }
            if (entity is IGameUpdatable updatableEntity)
            {
                entityAggregator.Add(updatableEntity);
            }

            return entity;
        }, lifetime));
    }
}
