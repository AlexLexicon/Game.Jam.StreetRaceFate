using Game.Jam.StreetRaceFate.Application.Scenes;
using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Jam.StreetRaceFate.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IViewService, ViewService>();

        services.AddScene<RaceScene>();
        services.AddGameObject<Car>(options =>
        {
            options.Lifetime = ServiceLifetime.Transient;
            options.AddToSpriteBatch<CarsSpriteBatch>(options =>
            {
                options.SetDrawPriority(1);
            });
        });
        services.AddGameObject<RaceText>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.AddToSpriteBatch<RaceTextSpriteBatch>(options =>
            {
                options.SetDrawPriority(2);
            });
        });
    }
}
