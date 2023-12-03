using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game.Jam.StreetRaceFate.Application;
public class News : IGameLoadable, ISpriteBatchDrawable<NewsSpriteBatch>, IGameUpdatable
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
    private SpriteFont ReporterFont { get; set; }

    private Vector2 Position { get; set; }

    public bool IsCool { get; set; }
    public bool IsLame { get; set; }
    public bool IsEpic { get; set; }
    public bool IsDeadly { get; set; }

    private string? TextLine1 { get; set; }
    private string? TextLine2 { get; set; }

    private void Epic1()
    {
        TextLine1 = "A qoute from a fan who was at the epic street race tonight:";
        TextLine2 = "'That guy was a legend! If I knew their name I'd never forget it!'";
    }

    private int Lame1()
    {
        TextLine1 = "An anticlimactic street race was held tonight.";
        TextLine2 = "Fans say it sucked because no one died";

        return 0;
    }
    private int Lame2()
    {
        TextLine1 = "Street race deemed boring because I quote:";
        TextLine2 = "'No one died.'";

        return 1;
    }
    private int Lame3()
    {
        TextLine1 = "Casket company sponsor of street race goes bankrupt";
        TextLine2 = "after race ends in all participants living";

        return 2;
    }
    private int Lame4()
    {
        TextLine1 = "'Whats the point of a street race if no one dies?'";
        TextLine2 = "asks man we intervied after disappointing street race.";

        return 3;
    }

    private int Deadly1()
    {
        TextLine1 = "Street race ends with all drivers dead";
        TextLine2 = "'It was the coolest thing I've seen' says fan.";

        return 0;
    }

    private int Cool1()
    {
        TextLine1 = "'I guess that street race was cool. At least someone died.'";
        TextLine2 = "says man at tonights street race.";

        return 0;
    }

    private void GenerateText()
    {
        if (IsLame)
        {
            _ = Random.Shared.Next(0, 5) switch
            {
                0 => Lame1(),
                1 => Lame2(),
                2 => Lame3(),
                _ => Lame4(),
            };
        }
        else if (IsEpic)
        {
            Epic1();
        }
        else if (IsDeadly)
        {
            Deadly1();
        }
        else if (IsCool)
        {
            Cool1();
        }
    }

    public void LoadContent()
    {
        CoolTexture = _contentManagerService.LoadTexture2D("news.cool");
        LameTexture = _contentManagerService.LoadTexture2D("news.lame");
        EpicTexture = _contentManagerService.LoadTexture2D("news.epic");
        DeadlyTexture = _contentManagerService.LoadTexture2D("news.deadly");
        ReporterTexture = _contentManagerService.LoadTexture2D("reporter.big");
        ReporterFont = _contentManagerService.LoadSpriteFont("medium");

        var w = _viewportService.GetViewportWidth();
        var h = _viewportService.GetViewportHeight();
        Position = new Vector2(((w / 3) * 2) + 125, (h / 2) - 100);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (TextLine2 is not null)
        {
            _drawService.Draw(spriteBatch, ReporterTexture, Vector2.Zero);
            spriteBatch.DrawString(ReporterFont, TextLine1, new Vector2(208, 64), Color.White);
            spriteBatch.DrawString(ReporterFont, TextLine2, new Vector2(208, 120), Color.White);
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

    public void Update(GameTime gameTime)
    {
        if (_raceService.IsRaceOver() && TextLine1 is null)
        {
            GenerateText();
        }
    }
}