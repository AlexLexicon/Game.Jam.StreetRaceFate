using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IGraphicsDeviceManagerService
{
    void SetFullscreen(bool isFullscreen);
    Vector2 GetResolution();
    void SetResolution(int width, int height);
}
public class GraphicsDeviceManagerService : IGraphicsDeviceManagerService
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;

    public GraphicsDeviceManagerService(GraphicsDeviceManager graphicsDeviceManager)
    {
        _graphicsDeviceManager = graphicsDeviceManager;
    }

    private int Width { get; set; }
    private int Height { get; set; }

    public void SetFullscreen(bool isFullscreen)
    {
        _graphicsDeviceManager.IsFullScreen = isFullscreen;
    }

    public Vector2 GetResolution()
    {
        return new Vector2(Width, Height);
    }

    public void SetResolution(int width, int height)
    {
        Width = width;
        Height = height;

        _graphicsDeviceManager.PreferredBackBufferWidth = Width;
        _graphicsDeviceManager.PreferredBackBufferHeight = Height;
        _graphicsDeviceManager.ApplyChanges();
    }
}
