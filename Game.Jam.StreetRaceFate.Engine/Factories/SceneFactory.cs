using Microsoft.Extensions.DependencyInjection;
using System;

namespace Game.Jam.StreetRaceFate.Engine.Factories;
public interface ISceneFactory
{
    TGameScene Create<TGameScene>() where TGameScene : IGameScene;
}
public class SceneFactory : ISceneFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SceneFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TGameScene Create<TGameScene>() where TGameScene : IGameScene
    {
        return _serviceProvider.GetRequiredService<TGameScene>();
    }
}
