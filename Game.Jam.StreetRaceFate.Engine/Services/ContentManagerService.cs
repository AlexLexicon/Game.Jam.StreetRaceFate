using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IContentManagerService
{
    void SetRootDirectory(string rootDirectory);
    Texture2D LoadTexture2D(string assetName);
    SpriteFont LoadSpriteFont(string assetName);
    Song LoadSong(string song);
    SoundEffect LoadSoundEffect(string assetName);
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

    public Song LoadSong(string assetName)
    {
        return _contentManager.Load<Song>(assetName);
    }

    public SoundEffect LoadSoundEffect(string assetName)
    {
        return _contentManager.Load<SoundEffect>(assetName);
    }
}
