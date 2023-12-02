using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IGraphicsDeviceManagerService
{
}

public class GraphicsDeviceManagerService : IGraphicsDeviceManagerService
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;

    public GraphicsDeviceManagerService(GraphicsDeviceManager graphicsDeviceManager)
    {
        _graphicsDeviceManager = graphicsDeviceManager;
    }
}
