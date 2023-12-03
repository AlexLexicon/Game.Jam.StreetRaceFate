using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;

namespace Game.Jam.StreetRaceFate.Windows;
public interface IWindowsGameService
{
    void Initalize();
    void Run<TGameScene>() where TGameScene : IGameScene;
}
public class WindowsGameService : IWindowsGameService
{
    private readonly IGraphicsDeviceManagerService _graphicsDeviceManagerService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IGameWindowService _gameWindowService;
    private readonly IGameService _gameService;
    private readonly ISceneFactory _sceneFactory;

    public WindowsGameService(
        IGraphicsDeviceManagerService graphicsDeviceManagerService,
        IContentManagerService contentManagerService,
        IGameWindowService gameWindowService,
        IGameService gameService,
        ISceneFactory sceneFactory)
    {
        _graphicsDeviceManagerService = graphicsDeviceManagerService;
        _contentManagerService = contentManagerService;
        _gameWindowService = gameWindowService;
        _gameService = gameService;
        _sceneFactory = sceneFactory;
    }

    private IGameScene MainScene { get; set; }

    public void Initalize()
    {
        _contentManagerService.SetRootDirectory("Content");
        _gameWindowService.SetAllowUserResizing(true);
        _gameService.SetIsMouseVisible(true);
    }

    public void Run<TGameScene>() where TGameScene : IGameScene 
    {
        MainScene = _sceneFactory.Create<TGameScene>();

        _gameService.Run();
    }
}
