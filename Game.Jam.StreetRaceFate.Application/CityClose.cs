using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class CityClose : IGameInitalizable, IGameLoadable, IGameUpdatable, ISpriteBatchDrawable<BackgroundSpriteBatch>
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IMovementService _movementService;
    private readonly IViewportService _viewportService;
    private readonly IRaceService _raceService;

    public CityClose(
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

    private Texture2D LeftTexture { get; set; }
    private Texture2D RightTexture { get; set; }

    private Vector2 LeftPosition { get; set; }
    private Vector2 RightPosition { get; set; }
    private Vector2 StopTarget { get; set; }

    public void Initalize()
    {
        
    }

    public void LoadContent()
    {
        LeftTexture = _contentManagerService.LoadTexture2D("city.close.small");
        RightTexture = _contentManagerService.LoadTexture2D("city.close.small");

        StopTarget = new Vector2(-LeftTexture.Width, 0);
        RightPosition = ResetPosition();
    }

    public void Update(GameTime gameTime)
    {
        LeftPosition = Move(LeftPosition);
        RightPosition = Move(RightPosition);
    }

    private Vector2 Move(Vector2 position)
    {
        position = new Vector2(position.X - _raceService.GetCloseCitySpeed(), 0);

        if (position.X <= StopTarget.X)
        {
            position = ResetPosition();
        }

        return position;
    }

    private Vector2 ResetPosition()
    {
        return new Vector2(LeftTexture.Width, 0);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _drawService.Draw(spriteBatch, LeftTexture, LeftPosition);
        _drawService.Draw(spriteBatch, RightTexture, RightPosition);
    }
}