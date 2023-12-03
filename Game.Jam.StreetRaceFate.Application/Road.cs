using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class Road : IGameLoadable, ISpriteBatchDrawable<BackgroundSpriteBatch>
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;

    public Road(IDrawService drawService, IContentManagerService contentManagerService)
    {
        _drawService = drawService;
        _contentManagerService = contentManagerService;
    }

    private Texture2D RoadTexture { get; set; }

    public void LoadContent()
    {
        RoadTexture = _contentManagerService.LoadTexture2D("road.better");
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _drawService.Draw(spriteBatch, RoadTexture, Vector2.Zero);
    }
}
