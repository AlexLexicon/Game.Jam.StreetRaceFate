namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IGameService
{
    void SetIsMouseVisible(bool isMouseVisible);
    void Run();
}
public class GameService : IGameService
{
    private readonly Microsoft.Xna.Framework.Game _game;

    public GameService(Microsoft.Xna.Framework.Game game)
    {
        _game = game;
    }

    public void SetIsMouseVisible(bool isMouseVisible)
    {
        _game.IsMouseVisible = isMouseVisible;
    }

    public void Run()
    {
        _game.Run();
    }
}
