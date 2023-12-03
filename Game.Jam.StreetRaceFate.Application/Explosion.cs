using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class Explosion : IGameUpdatable, IGameLoadable, ISpriteBatchDrawable<ExplosionSpriteBatch>
{
    private readonly IContentManagerService _contentManagerService;
    private readonly IDelayService _delayService;

    private const int columns = 15;

    public Explosion(
        IContentManagerService contentManagerService, 
        IDelayService delayService)
    {
        _contentManagerService = contentManagerService;
        _delayService = delayService;
    }

    private Texture2D ExplosionTexture { get; set; }
    private Vector2 Position { get; set; }
    private Rectangle SheetRect { get; set; }
    private int SheetIndex { get; set; }
    private Vector2 Center => new Vector2(Position.X - ((Size / 2) * Scale), Position.Y - ((Size / 2) * Scale));

    private float Scale { get; set; }

    private int? _size;
    private int Size
    {
        get
        {
            if (_size is null)
            {
                int width = ExplosionTexture.Width;
                _size = (width - (columns + 1)) / columns;
            }
            return _size.Value;
        }
    }

    public void Spawn(Vector2 position, float scale)
    {
        Position = new Vector2(position.X, position.Y - 10);
        Scale = scale;
    }

    public void LoadContent()
    {
        ExplosionTexture = _contentManagerService.LoadTexture2D("explosion.edgeless");

        NextSheet();
    }

    public void Update(GameTime gameTime)
    {
        _delayService.Delay(gameTime, 0.1f, () =>
        {
            NextSheet();
        });
    }

    private void NextSheet()
    {
        int x = (SheetIndex * Size) + (SheetIndex + 1);

        SheetRect = new Rectangle(x, 1, Size, Size);

        SheetIndex++;

        if (SheetIndex >= columns)
        {
            SheetIndex = 0;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(ExplosionTexture, Center, SheetRect, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
    }
}
