using Microsoft.Extensions.DependencyInjection;

namespace Game.Jam.StreetRaceFate.Engine.Builders;
public abstract class GameObjectOptionsBuilder<TGameObject> where TGameObject : IGameObject
{
    public GameObjectOptionsBuilder(IServiceCollection services)
    {
        Services = services;
        Lifetime = ServiceLifetime.Singleton;
    }

    public IServiceCollection Services { get; }
    public ServiceLifetime Lifetime { get; set; }
}
