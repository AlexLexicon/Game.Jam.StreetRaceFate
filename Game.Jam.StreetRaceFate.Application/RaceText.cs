using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class RaceText : IGameInitalizable, IGameLoadable, IGameUpdatable, ISpriteBatchDrawable<RaceTextSpriteBatch>
{
    private readonly IContentManagerService _contentManagerService;
    private readonly IViewportService _viewportService;

    public RaceText(
        IContentManagerService contentManagerService, 
        IViewportService viewportService)
    {
        _contentManagerService = contentManagerService;
        _viewportService = viewportService;
    }

    public bool IsVisible { get; set; }
    public string? Text { get; set; }

    private SpriteFont SpriteFont { get; set; }
    private Vector2 Position { get; set; }

    public void LoadContent()
    {
        SpriteFont = _contentManagerService.LoadSpriteFont("Large");
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
        if (IsVisible && Text is not null)
        {
            spriteBatch.DrawString(SpriteFont, Text, Position, Color.White);
        }
    }
}
