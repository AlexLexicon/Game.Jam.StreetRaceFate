using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IViewportService
{
    int GetViewportWidth();
    int GetViewportHeight();
}
public class ViewportService : IViewportService
{
    private readonly IGraphicsDeviceProvider _graphicsDeviceProvider;

    public ViewportService(IGraphicsDeviceProvider graphicsDeviceProvider)
    {
        _graphicsDeviceProvider = graphicsDeviceProvider;
    }

    public int GetViewportWidth()
    {
        return _graphicsDeviceProvider.GraphicsDevice.Viewport.Width;
    }

    public int GetViewportHeight()
    {
        return _graphicsDeviceProvider.GraphicsDevice.Viewport.Height;
    }
}
