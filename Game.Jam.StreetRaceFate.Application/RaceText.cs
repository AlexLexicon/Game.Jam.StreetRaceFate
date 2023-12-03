using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class RaceText : IGameInitalizable, IGameLoadable, IGameUpdatable, ISpriteBatchDrawable<RaceTextSpriteBatch>
{
    private readonly IContentManagerService _contentManagerService;
    private readonly IViewportService _viewportService;
    private readonly IDrawService _drawService;

    public RaceText(
        IContentManagerService contentManagerService,
        IViewportService viewportService,
        IDrawService drawService)
    {
        _contentManagerService = contentManagerService;
        _viewportService = viewportService;
        _drawService = drawService;
    }

    public bool IsVisible { get; set; }
    public string? BigText { get; set; }
    public string? MediumText { get; set; }

    private SpriteFont LargeSpriteFont { get; set; }
    private SpriteFont MediumSpriteFont { get; set; }
    private Vector2 Position { get; set; }

    public void LoadContent()
    {
        LargeSpriteFont = _contentManagerService.LoadSpriteFont("large");
        MediumSpriteFont = _contentManagerService.LoadSpriteFont("medium");
    }

    public void Initalize()
    {
        int width = _viewportService.GetViewportWidth();
        int height = _viewportService.GetViewportHeight();

        Position = new Vector2(width / 2, height / 2);
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (IsVisible)
        {
            if (BigText is not null)
            {
                var p = Position;

                _drawService.Draw(spriteBatch, LargeSpriteFont, BigText, new Vector2(p.X + 9, p.Y + 9), new Color(0, 0, 0));
                _drawService.Draw(spriteBatch, LargeSpriteFont, BigText, new Vector2(p.X + 6, p.Y + 6), new Color(25, 195, 230));
                _drawService.Draw(spriteBatch, LargeSpriteFont, BigText, new Vector2(p.X + 3, p.Y + 3), new Color(254, 75, 179));
                _drawService.Draw(spriteBatch, LargeSpriteFont, BigText, p, Color.White);
            }
            if (MediumText is not null)
            {
                var p = new Vector2(Position.X, Position.Y + 28);

                _drawService.Draw(spriteBatch, MediumSpriteFont, MediumText, new Vector2(p.X + 3, p.Y + 3), new Color(0, 0, 0));
                _drawService.Draw(spriteBatch, MediumSpriteFont, MediumText, new Vector2(p.X + 2, p.Y + 2), new Color(25, 195, 230));
                _drawService.Draw(spriteBatch, MediumSpriteFont, MediumText, new Vector2(p.X + 1, p.Y + 1), new Color(254, 75, 179));
                _drawService.Draw(spriteBatch, MediumSpriteFont, MediumText, p, Color.White);
            }
        }
    }
}
