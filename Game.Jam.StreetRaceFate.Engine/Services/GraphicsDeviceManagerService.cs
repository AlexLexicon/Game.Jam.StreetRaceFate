namespace Game.Jam.StreetRaceFate.Engine.Services;
public class GraphicsDeviceManagerService : IGraphicsDeviceManagerService
{
    private readonly Microsoft.Xna.Framework.Game _game;

    public GraphicsDeviceManagerService(Microsoft.Xna.Framework.Game game)
    {
        _game = game;
    }
}
