using Game.Jam.StreetRaceFate.Application;
using Game.Jam.StreetRaceFate.Engine.Services;

namespace Game.Jam.StreetRaceFate.Windows;
public interface IWindowsGameService
{
    void Initalize();
    void Run();
}
public class WindowsGameService : IWindowsGameService
{
    private readonly IGraphicsDeviceManagerService _graphicsDeviceManagerService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IGameWindowService _gameWindowService;
    private readonly IGameService _gameService;

    public WindowsGameService(
        IGraphicsDeviceManagerService graphicsDeviceManagerService,
        IContentManagerService contentManagerService,
        IGameWindowService gameWindowService,
        IGameService gameService)
    {
        _graphicsDeviceManagerService = graphicsDeviceManagerService;
        _contentManagerService = contentManagerService;
        _gameWindowService = gameWindowService;
        _gameService = gameService;
    }

    public void Initalize()
    {
        _contentManagerService.SetRootDirectory("Content");
        _gameWindowService.SetAllowUserResizing(true);
        _gameService.SetIsMouseVisible(true);
    }

    public void Run()
    {
        _gameService.Run();
    }
}
