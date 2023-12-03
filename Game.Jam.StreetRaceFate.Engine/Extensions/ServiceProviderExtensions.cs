using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Game.Jam.StreetRaceFate.Engine.Extensions;
public static class ServiceProviderExtensions
{
    public static TGameObject ActivateGameInstance<TGameObject>(this IServiceProvider serviceProvider)
    {
        Type gameObjectType = typeof(TGameObject);

        var gameOrchestrer = serviceProvider.GetRequiredService<IGameOrchestrer>();

        var gameObject = (TGameObject)ActivatorUtilities.CreateInstance(serviceProvider, gameObjectType);

        if (gameObject is IGameInitalizable gameInializable)
        {
            gameOrchestrer.AddInitializable(gameInializable);
        }
        if (gameObject is IGameLoadable gameLoadable)
        {
            gameOrchestrer.AddLoadable(gameLoadable);
        }
        if (gameObject is IGameUpdatable gameUpdatable)
        {
            gameOrchestrer.AddUpdatable(gameUpdatable);
        }
        if (gameObject is IGameDrawable gameDrawable)
        {
            gameOrchestrer.AddDrawable(gameDrawable);
        }

        return gameObject;
    }

    public static TGameObject ActivateWeakGameInstance<TGameObject>(this IServiceProvider serviceProvider, IWeakRefrenceManager weakRefrenceManager)
    {
        Type gameObjectType = typeof(TGameObject);

        var gameObject = (TGameObject)ActivatorUtilities.CreateInstance(serviceProvider, gameObjectType);

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
        if (gameObject is IGameDrawable gameDrawable)
        {
            weakRefrenceManager.Add(gameDrawable);
        }

        return gameObject;
    }
}
