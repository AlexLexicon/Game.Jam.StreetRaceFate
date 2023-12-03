using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Net.NetworkInformation;

namespace Game.Jam.StreetRaceFate.Application;
public class Crowd : IGameLoadable, IGameUpdatable, ISpriteBatchDrawable<BackgroundSpriteBatch>
{
    private readonly IDrawService _drawService;
    private readonly IContentManagerService _contentManagerService;
    private readonly IMovementService _movementService;
    private readonly IViewportService _viewportService;
    private readonly IRaceService _raceService;
    private readonly IOscillateService _oscillateService;
    private readonly IDelayService _delayService;

    public Crowd(
        IDrawService drawService,
        IContentManagerService contentManagerService,
        IMovementService movementService,
        IViewportService viewportService,
        IRaceService raceService,
        IOscillateService oscillateService,
        IDelayService delayService)
    {
        _drawService = drawService;
        _contentManagerService = contentManagerService;
        _movementService = movementService;
        _viewportService = viewportService;
        _raceService = raceService;
        _oscillateService = oscillateService;
        _delayService = delayService;
    }

    private Texture2D FrontTexture { get; set; }
    private Texture2D BackTexture { get; set; }

    private Vector2 Position { get; set; }

    private Vector2 FrontFinalPosition => new Vector2(Position.X, Position.Y + FrontOsc);
    private int FrontOsc { get; set; }
    private Vector2 BackFinalPosition => new Vector2(Position.X, Position.Y + BackOsc);
    private int BackOsc { get; set; }

    private int MinOscBack { get; set; }
    private int MaxOscBack { get; set; }
    private int MinOscFront { get; set; }
    private int MaxOscFront { get; set; }

    private int Index { get; set; }

    public void Spawn(int index)
    {
        Index = index;

        MinOscBack = Random.Shared.Next(-2, 0);
        MaxOscBack = Random.Shared.Next(0, 2);
        MinOscFront = Random.Shared.Next(-4, 0);
        MaxOscFront = Random.Shared.Next(0, 4);
    }

    public void LoadContent()
    {
        FrontTexture = _contentManagerService.LoadTexture2D("crowd.front");
        BackTexture = _contentManagerService.LoadTexture2D("crowd.back");

        Position = new Vector2(Index * FrontTexture.Width, 218);
    }

    public void Update(GameTime gameTime)
    {
        _delayService.Delay(gameTime, 0.15f, () =>
        {
            _oscillateService.Oscillate(MinOscBack, MaxOscBack, o =>
            {
                BackOsc = o;
            });
            _oscillateService.Oscillate(MinOscFront, MaxOscFront, o =>
            {
                FrontOsc = o;
            });
        });

        if (_raceService.IsRaceStarted())
        {
            Position = new Vector2(Position.X - _raceService.GetRailSpeed(), Position.Y);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _drawService.Draw(spriteBatch, BackTexture, BackFinalPosition);
        _drawService.Draw(spriteBatch, FrontTexture, FrontFinalPosition);
    }
}