using Game.Jam.StreetRaceFate.Engine.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Jam.StreetRaceFate.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddGameObject<Dog>(ServiceLifetime.Transient);
        services.AddGameObject<DogsSpriteBatch>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetDrawPriority(1);
        });
        services.AddGameObject<Screen>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetDrawPriority(0);
        });
    }
}
