using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IGraphicsService
{
    void Clear(Color color);
    SpriteBatch CreateSpriteBatch();
}
public class GraphicsService : IGraphicsService
{
    private readonly IGraphicsDeviceProvider _graphicsDeviceProvider;

    public GraphicsService(IGraphicsDeviceProvider graphicsDeviceProvider)
    {
        _graphicsDeviceProvider = graphicsDeviceProvider;
    }

    public void Clear(Color color)
    {
        _graphicsDeviceProvider.GraphicsDevice.Clear(color);
    }

    public SpriteBatch CreateSpriteBatch()
    {
        return new SpriteBatch(_graphicsDeviceProvider.GraphicsDevice);
    }
}
