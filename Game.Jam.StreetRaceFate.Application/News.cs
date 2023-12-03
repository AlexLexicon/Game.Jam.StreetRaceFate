using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class News : IGameLoadable, ISpriteBatchDrawable<NewsSpriteBatch>
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IMovementService _movementService;
    private readonly IViewportService _viewportService;
    private readonly IRaceService _raceService;

    public News(
        IDrawService drawService,
        IContentManagerService contentManagerService,
        IMovementService movementService,
        IViewportService viewportService,
        IRaceService raceService)
    {
        _drawService = drawService;
        _contentManagerService = contentManagerService;
        _movementService = movementService;
        _viewportService = viewportService;
        _raceService = raceService;
    }

    private Texture2D CoolTexture { get; set; }
    private Texture2D LameTexture { get; set; }
    private Texture2D EpicTexture { get; set; }
    private Texture2D DeadlyTexture { get; set; }

    private Texture2D ReporterTexture { get; set; }

    private Vector2 Position { get; set; }

    public bool IsCool { get; set; }
    public bool IsLame { get; set; }
    public bool IsEpic { get; set; }
    public bool IsDeadly { get; set; }

    public void LoadContent()
    {
        CoolTexture = _contentManagerService.LoadTexture2D("news.cool");
        LameTexture = _contentManagerService.LoadTexture2D("news.lame");
        EpicTexture = _contentManagerService.LoadTexture2D("news.epic");
        DeadlyTexture = _contentManagerService.LoadTexture2D("news.deadly");
        ReporterTexture = _contentManagerService.LoadTexture2D("reporter");

        var w = _viewportService.GetViewportWidth();
        var h = _viewportService.GetViewportHeight();
        Position = new Vector2(((w / 3) * 2) + 125, (h / 2) - 100);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (IsCool || IsLame || IsEpic || IsDeadly)
        {
            //_drawService.Draw(spriteBatch, ReporterTexture, Vector2.Zero);
        }
        if (IsCool)
        {
            _drawService.Draw(spriteBatch, CoolTexture, Position, scale: 2.5f);
        }
        else if (IsLame)
        {
            _drawService.Draw(spriteBatch, LameTexture, Position, scale: 2.5f);
        }
        else if (IsEpic)
        {
            _drawService.Draw(spriteBatch, EpicTexture, Position, scale: 2.5f);
        }
        else if (IsDeadly)
        {
            _drawService.Draw(spriteBatch, DeadlyTexture, Position, scale: 2.5f);
        }
    }
}