using Microsoft.Extensions.DependencyInjection;

namespace Game.Jam.StreetRaceFate.Engine.Builders;
public class GameObjectOptionsBuilder<TGameObject>
{
    public GameObjectOptionsBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
    public ServiceLifetime Lifetime { get; set; }
}
