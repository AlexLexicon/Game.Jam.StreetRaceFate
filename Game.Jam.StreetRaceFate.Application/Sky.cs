using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Game.Jam.StreetRaceFate.Application;
public class Sky : IGameLoadable, IGameUpdatable, ISpriteBatchDrawable<BackgroundSpriteBatch>
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IRaceService _raceService;

    public Sky(
        IDrawService drawService, 
        IContentManagerService contentManagerService, 
        IRaceService raceService)
    {
        _drawService = drawService;
        _contentManagerService = contentManagerService;
        _raceService = raceService;
    }

    private Texture2D RoadTexture { get; set; }
    private Song RaceSong { get; set; }
    private Song ChillSong { get; set; }

    private bool IsStarted { get; set; }
    private bool IsChill { get; set; }

    public void LoadContent()
    {
        RoadTexture = _contentManagerService.LoadTexture2D("background.small");
        RaceSong = _contentManagerService.LoadSong("race.trim");
        ChillSong = _contentManagerService.LoadSong("chill.trim");
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        
        _drawService.Draw(spriteBatch, RoadTexture, Vector2.Zero);
    }

    public void Update(GameTime gameTime)
    {
        bool racing = _raceService.IsRacing();
        if (!IsStarted && racing)
        {
            MediaPlayer.IsRepeating = false;
            //MediaPlayer.Play(RaceSong);
            IsStarted = true;
            IsChill = false;
        }

        if (!racing && !IsChill && MediaPlayer.State is not MediaState.Playing)
        {
            MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(ChillSong);
            IsChill = true;
            IsStarted = false;
        }
    }
}