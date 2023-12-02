using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IGameWindowService
{
    void SetAllowUserResizing(bool allowUserResizing);
}

public class GameWindowService : IGameWindowService
{
    private readonly GameWindow _gameWindow;

    public GameWindowService(GameWindow gameWindow)
    {
        _gameWindow = gameWindow;
    }

    public void SetAllowUserResizing(bool allowUserResizing)
    {
        _gameWindow.AllowUserResizing = true;
    }
}
