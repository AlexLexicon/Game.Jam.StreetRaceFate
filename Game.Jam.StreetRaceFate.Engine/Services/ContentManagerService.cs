namespace Game.Jam.StreetRaceFate.Engine.Services;
public class ContentManagerService : IContentManagerService
{
    private readonly Microsoft.Xna.Framework.Game _game;

    public ContentManagerService(Microsoft.Xna.Framework.Game game)
    {
        _game = game;
    }

    public void SetRootDirectory(string rootDirectory)
    {
        _game.Content.RootDirectory = rootDirectory;
    }
}
