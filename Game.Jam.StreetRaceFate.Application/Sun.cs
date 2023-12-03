using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class Sun : IGameLoadable, ISpriteBatchDrawable<BackgroundSpriteBatch>
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IViewportService _viewportService;

    public Sun(IDrawService drawService, IContentManagerService contentManagerService, IViewportService viewportService)
    {
        _drawService = drawService;
        _contentManagerService = contentManagerService;
        _viewportService = viewportService;
    }

    private Texture2D RoadTexture { get; set; }
    private Vector2 Position { get; set; }

    public void LoadContent()
    {
        RoadTexture = _contentManagerService.LoadTexture2D("sun.small");

        Position = new Vector2(_viewportService.GetViewportWidth() / 2 - (RoadTexture.Width / 2), 85);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _drawService.Draw(spriteBatch, RoadTexture, Position);
    }
}