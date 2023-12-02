namespace Game.Jam.StreetRaceFate.Engine.Services;
public class WindowService : IWindowService
{
    private readonly Microsoft.Xna.Framework.Game _game;

    public WindowService(Microsoft.Xna.Framework.Game game)
    {
        _game = game;
    }

    public void SetAllowUserResizing(bool allowUserResizing)
    {
        _game.Window.AllowUserResizing = true;
    }
}
