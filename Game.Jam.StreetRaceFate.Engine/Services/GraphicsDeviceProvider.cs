using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IGraphicsDeviceProvider
{
    GraphicsDevice GraphicsDevice { get; }
}
public class GraphicsDeviceProvider : IGraphicsDeviceProvider
{
    private readonly Microsoft.Xna.Framework.Game _game;

    public GraphicsDeviceProvider(Microsoft.Xna.Framework.Game game)
    {
        _game = game;
    }

    public GraphicsDevice GraphicsDevice => _game.GraphicsDevice;
}
