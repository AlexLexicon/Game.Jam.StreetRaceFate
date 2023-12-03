using Game.Jam.StreetRaceFate.Application.Scenes;
using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Jam.StreetRaceFate.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IRaceService, RaceService>();
        services.AddSingleton<IViewService, ViewService>();

        services.AddScene<RaceScene>();

        services.AddGameObject<Sky>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetSbDrawPrioirty(1);
            options.AddToSpriteBatch<BackgroundSpriteBatch>(options =>
            {
                options.SetDrawPriority(1);
            });
        });
        services.AddGameObject<Sun>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetSbDrawPrioirty(2);
            options.AddToSpriteBatch<BackgroundSpriteBatch>();
        });
        services.AddGameObject<CityFar>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetSbDrawPrioirty(3);
            options.AddToSpriteBatch<BackgroundSpriteBatch>();
        });
        services.AddGameObject<CityClose>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetSbDrawPrioirty(4);
            options.AddToSpriteBatch<BackgroundSpriteBatch>();
        });
        services.AddGameObject<Road>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetSbDrawPrioirty(5);
            options.AddToSpriteBatch<BackgroundSpriteBatch>();
        });
        services.AddGameObject<Rail>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.SetSbDrawPrioirty(6);
            options.AddToSpriteBatch<BackgroundSpriteBatch>();
        });
        services.AddGameObject<Car>(options =>
        {
            options.Lifetime = ServiceLifetime.Transient;
            options.AddToSpriteBatch<CarsSpriteBatch>(options =>
            {
                options.SetDrawPriority(2);
            });
        });
        services.AddGameObject<RaceText>(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.AddToSpriteBatch<RaceTextSpriteBatch>(options =>
            {
                options.SetDrawPriority(3);
            });
        });
    }
}
