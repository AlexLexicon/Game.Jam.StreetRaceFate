using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Jam.StreetRaceFate.Engine.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddEngine(this IServiceCollection services)
    {
        services.AddSingleton<IContentManagerService, ContentManagerService>();
        services.AddSingleton<IGraphicsDeviceManagerService, GraphicsDeviceManagerService>();
        services.AddSingleton<IMonogameService, MonogameService>();
        services.AddSingleton<IWindowService, WindowService>();
    }
}
