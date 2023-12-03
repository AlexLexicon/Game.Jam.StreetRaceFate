using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class Cones : IGameLoadable, ISpriteBatchDrawable<BackgroundSpriteBatch>, IGameUpdatable
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IMovementService _movementService;
    private readonly IViewportService _viewportService;
    private readonly IRaceService _raceService;

    public Cones(
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

    private Texture2D Texture { get; set; }

    private Vector2 Position { get; set; }

    public void LoadContent()
    {
        Texture = _contentManagerService.LoadTexture2D("cones");

        var w = _viewportService.GetViewportWidth();
        Position = new Vector2(((w * 10) + (w / 3)) - 1000, 203);
    }

    public void Update(GameTime gameTime)
    {
        if (_raceService.IsRaceStarted())
        {
            Position = new Vector2(Position.X - _raceService.GetRailSpeed(), Position.Y);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}