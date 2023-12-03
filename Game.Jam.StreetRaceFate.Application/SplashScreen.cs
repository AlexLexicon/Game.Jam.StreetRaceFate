using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class SplashScreen : IGameLoadable, ISpriteBatchDrawable<RaceTextSpriteBatch>, IGameUpdatable
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IViewportService _viewportService;
    private readonly IGraphicsService _graphicsService;
    private readonly IDelayService _delayService;

    public SplashScreen(IDrawService drawService, IContentManagerService contentManagerService, IViewportService viewportService, IGraphicsService graphicsService, IDelayService delayService)
    {
        _drawService = drawService;
        _contentManagerService = contentManagerService;
        _viewportService = viewportService;
        _graphicsService = graphicsService;
        _delayService = delayService;
    }

    public bool IsVisible { get; set; }

    public string? TitleText { get; set; }
    public string? CreditsText { get; set; }

    private SpriteFont MediumSpriteFont { get; set; }
    private SpriteFont SmallSpriteFont { get; set; }

    private Vector2 TitlePosition { get; set; }
    private Vector2 CreditsPosition { get; set; }

    private Texture2D Background { get; set; }

    public float Opacity { get; set; }
    public float TargetOpacity { get; set; }

    public void LoadContent()
    {
        IsVisible = true;
        Opacity = 1f;
        TargetOpacity = Opacity;

        TitleText = "STREET RACE FATE";
        CreditsText = "created by ALEX STROOT with music by ANDY CHAMBERLAIN";

        var w = _viewportService.GetViewportWidth();
        var h = _viewportService.GetViewportHeight();

        MediumSpriteFont = _contentManagerService.LoadSpriteFont("large");
        SmallSpriteFont = _contentManagerService.LoadSpriteFont("normal");
        Background = _graphicsService.CreateTexture(1, 1);
        Background.SetData(new[] { Color.Black });

        var titleSize = MediumSpriteFont.MeasureString(TitleText);
        var cSize = SmallSpriteFont.MeasureString(CreditsText);

        TitlePosition = new Vector2(w / 2, titleSize.Y);
        CreditsPosition = new Vector2(w / 2, h - (cSize.Y * 2));
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Background, new Rectangle(0, 0, _viewportService.GetViewportWidth(), _viewportService.GetViewportHeight()), new Color(Color.Black, Opacity));
        if (TitleText is not null)
        {
            var p = TitlePosition;

            var x1 = new Color(0, 0, 0);
            var x2 = new Color(25, 195, 230);
            var x3 = new Color(254, 75, 179);
            var x4 = Color.White;

            if (Opacity < 1)
            {
                x1 = Color.Black;
                x2 = Color.Black;
                x3 = Color.Black;
                x4 = Color.Black;
            }

            _drawService.Draw(spriteBatch, MediumSpriteFont, TitleText, new Vector2(p.X + 9, p.Y + 9), new Color(x1, Opacity));
            _drawService.Draw(spriteBatch, MediumSpriteFont, TitleText, new Vector2(p.X + 6, p.Y + 6), new Color(x2, Opacity));
            _drawService.Draw(spriteBatch, MediumSpriteFont, TitleText, new Vector2(p.X + 3, p.Y + 3), new Color(x3, Opacity));
            _drawService.Draw(spriteBatch, MediumSpriteFont, TitleText, p, new Color(x4, Opacity));
        }
        if (CreditsText is not null)
        {
            var p = new Vector2(CreditsPosition.X, CreditsPosition.Y);
            var x4 = Color.White;
            if (Opacity < 1)
            {
                x4 = Color.Black;
            }

            //_drawService.Draw(spriteBatch, MediumSpriteFont, CreditsText, new Vector2(p.X + 3, p.Y + 3), new Color(0, 0, 0));
            //_drawService.Draw(spriteBatch, MediumSpriteFont, CreditsText, new Vector2(p.X + 2, p.Y + 2), new Color(25, 195, 230));
            //_drawService.Draw(spriteBatch, MediumSpriteFont, CreditsText, new Vector2(p.X + 1, p.Y + 1), new Color(254, 75, 179));
            _drawService.Draw(spriteBatch, SmallSpriteFont, CreditsText, p, new Color(x4, Opacity));
        }
    }

    public void Update(GameTime gameTime)
    {
        if (IsVisible)
        {
            _delayService.Delay(gameTime, 5f, () =>
            {
                IsVisible = false;
            });
        }

        if (IsVisible && Opacity < 1)
        {
            TargetOpacity = 1;
        }
        else if (!IsVisible && Opacity > 0)
        {
            TargetOpacity = 0;
        }

        Opacity = Vector2.Lerp(new Vector2(Opacity), new Vector2(TargetOpacity), 0.05f).X;
    }
}
