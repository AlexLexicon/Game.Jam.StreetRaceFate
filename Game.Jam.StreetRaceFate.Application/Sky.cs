using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    private SoundEffect CitySound { get; set; }

    private bool IsStarted { get; set; }
    private bool IsChill { get; set; }
    private bool CrowdPlaying { get; set; }
    private SoundEffectInstance CrowdSound { get; set; }
    public void LoadContent()
    {
        RoadTexture = _contentManagerService.LoadTexture2D("background.small");
        RaceSong = _contentManagerService.LoadSong("race.final");
        ChillSong = _contentManagerService.LoadSong("chill.final");
        CitySound = _contentManagerService.LoadSoundEffect("city");
        var x = CitySound.CreateInstance();
        x.Volume = 0.25f;
        x.IsLooped = true;
        x.Play();

        PlayCrowd();
    }

    public void PlayCrowd()
    {
        var a = _contentManagerService.LoadSoundEffect("crowd");
        CrowdSound = a.CreateInstance();
        CrowdSound.IsLooped = true;
        CrowdSound.Volume = 0.25f;
        CrowdSound.Play();
        CrowdPlaying = true;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _drawService.Draw(spriteBatch, RoadTexture, Vector2.Zero);
    }

    public void Update(GameTime gameTime)
    {
        if (_raceService.IsRacing() && CrowdPlaying)
        {
            CrowdPlaying = false;
            CrowdSound.Stop();
        }

        bool racing = _raceService.IsRacing();
        if (!IsStarted && racing)
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(RaceSong);
            IsStarted = true;
            IsChill = false;
        }

        if (!racing && !IsChill && MediaPlayer.State is not MediaState.Playing)
        {
            PlayChill();
        }
    }

    public void PlayChill()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(ChillSong);
        IsChill = true;
        IsStarted = false;
    }
}