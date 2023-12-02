using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IContentManagerService
{
    void SetRootDirectory(string rootDirectory);
    Texture2D LoadTexture2D(string assetName);
    SpriteFont LoadSpriteFont(string assetName);
}
public class ContentManagerService : IContentManagerService
{
    private readonly ContentManager _contentManager;

    public ContentManagerService(ContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    public void SetRootDirectory(string rootDirectory)
    {
        _contentManager.RootDirectory = rootDirectory;
    }

    public Texture2D LoadTexture2D(string assetName)
    {
        return _contentManager.Load<Texture2D>(assetName);
    }

    public SpriteFont LoadSpriteFont(string assetName)
    {
        return _contentManager.Load<SpriteFont>(assetName);
    }
}
