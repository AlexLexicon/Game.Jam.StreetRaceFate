using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IGraphicsDeviceManagerService
{
    void SetFullscreen(bool isFullscreen);
    void SetResolution(int width, int height);
}
public class GraphicsDeviceManagerService : IGraphicsDeviceManagerService
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;

    public GraphicsDeviceManagerService(GraphicsDeviceManager graphicsDeviceManager)
    {
        _graphicsDeviceManager = graphicsDeviceManager;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _graphicsDeviceManager.IsFullScreen = isFullscreen;
    }

    public void SetResolution(int width, int height)
    {
        _graphicsDeviceManager.PreferredBackBufferWidth = width;
        _graphicsDeviceManager.PreferredBackBufferHeight = height;
        _graphicsDeviceManager.ApplyChanges();
    }
}
