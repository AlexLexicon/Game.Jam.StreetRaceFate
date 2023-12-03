using Microsoft.Extensions.DependencyInjection;
using System;

namespace Game.Jam.StreetRaceFate.Engine.Factories;
public interface IGameObjectFactory
{
    TGameObject Create<TGameObject>() where TGameObject : IGameObject;
}
public class GameObjectFactory : IGameObjectFactory
{
    private readonly IServiceProvider _serviceProvider;

    public GameObjectFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TGameObject Create<TGameObject>() where TGameObject : IGameObject
    {
        var x = _serviceProvider.GetRequiredService<TGameObject>();

        if (x is IGameLoadable gameLoadable)
        {
            gameLoadable.LoadContent();
        }

        if (x is IGameInitalizable gameInitalizable)
        {
            gameInitalizable.Initalize();
        }

        return x;
    }
}
