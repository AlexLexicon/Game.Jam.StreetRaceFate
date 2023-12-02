namespace Game.Jam.StreetRaceFate.Engine.Services;
public class MonogameService : IMonogameService
{
    private readonly Microsoft.Xna.Framework.Game _game;

    public MonogameService(Microsoft.Xna.Framework.Game game)
    {
        _game = game;
    }

    public void SetIsMouseVisible(bool isMouseVisible)
    {
        _game.IsMouseVisible = isMouseVisible;
    }
}
