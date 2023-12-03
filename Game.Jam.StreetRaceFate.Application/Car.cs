using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game.Jam.StreetRaceFate.Application;
public class Car : IGameInitalizable, IGameLoadable, IGameUpdatable, ISpriteBatchDrawable<CarsSpriteBatch>
{
    private readonly IContentManagerService _contentManagerService;
    private readonly IMovementService _movementService;
    private readonly IDrawService _drawService;
    private readonly IDelayService _delayService;
    private readonly IOscillateService _oscillateService;
    private readonly IKeysService _keysService;
    private readonly IRaceService _raceService;

    public Car(
        IContentManagerService contentManagerService,
        IMovementService movementService,
        IDrawService drawService,
        IDelayService delayService,
        IOscillateService oscillateService,
        IKeysService keysService,
        IRaceService raceService)
    {
        _contentManagerService = contentManagerService;
        _movementService = movementService;
        _drawService = drawService;
        _delayService = delayService;
        _oscillateService = oscillateService;

        Speed = 0.25f;
        _keysService = keysService;
        _raceService = raceService;
    }

    public int RowIndex { get; set; }
    public Keys Key { get; set; }
    private string? _keyString;
    public string KeyString => _keyString ??= _keysService.GetKeyString(Key);

    private State CurrentState { get; set; }
    public enum State
    {
        New,
        Readying,
        Racing,
    }

    private Texture2D CarTexture { get; set; }
    private Texture2D WheelWellsTexture { get; set; }
    private Texture2D LeftWheel { get; set; }
    private Texture2D RightWheel { get; set; }
    public Texture2D KeyBox { get; set; }
    private SpriteFont SpriteFont { get; set; }

    private float Speed { get; set; }
    private Vector2 Position { get; set; }
    private Vector2 TargetPosition { get; set; }

    private Vector2 CarPosition => new Vector2(Position.X, Position.Y + CarOscillation);
    private Vector2 LeftWheelPosition => new Vector2(Position.X + 13, Position.Y + 21);
    private Vector2 RightWheelPosition => new Vector2(Position.X + 88, Position.Y + 21);
    private Vector2 KeyBoxPosition => new Vector2(Position.X + CarTexture.Width + 21, Position.Y);
    private Vector2 KeyBoxTextPosition => new Vector2(KeyBoxPosition.X + KeyBox.Width / 2, KeyBoxPosition.Y + KeyBox.Height / 2);
    private float WheelRotation { get; set; }
    private int CarOscillation { get; set; }
    private bool IsVisible { get; set; }
    public bool IsReadyToRace { get; private set; }

    public void LoadContent()
    {
        CarTexture = _contentManagerService.LoadTexture2D("car.pixel");
        WheelWellsTexture = _contentManagerService.LoadTexture2D("wheelwell.pixel");
        LeftWheel = _contentManagerService.LoadTexture2D("tire.pixel");
        RightWheel = _contentManagerService.LoadTexture2D("tire.pixel");
        KeyBox = _contentManagerService.LoadTexture2D("keybox");
        SpriteFont = _contentManagerService.LoadSpriteFont("Normal");
    }

    public void Initalize()
    {
        CurrentState = State.New;

        IsVisible = false;
    }

    public void Spawn()
    {
        float y = RowIndex * (CarTexture.Height + ViewService.CAR_PADDING);

        Position = new Vector2(-CarTexture.Width, y);
        TargetPosition = new Vector2(42, y);
        Speed = 0.025f;

        IsVisible = true;
        IsReadyToRace = false;
    }

    public void Update(GameTime gameTime)
    {
        if (CurrentState != State.Racing && _raceService.IsRaceStarted())
        {
            CurrentState = State.Racing;
        }

        if (CurrentState == State.Readying)
        {
            IsReadyToRace = !Keyboard.GetState().IsKeyUp(Key);
        }

        Position = _movementService.MoveTo(gameTime, Position, TargetPosition, 1f, Speed, out bool reachedTarget);
        if (reachedTarget && CurrentState == State.New)
        {
            CurrentState = State.Readying;
        }
        else
        {
            WheelRotation += Vector2.Distance(Position, TargetPosition) / 750;
        }

        _delayService.Delay(gameTime, (1 - Speed) / 4, () =>
        {
            _oscillateService.Oscillate(0, 2, o =>
            {
                CarOscillation = o;
            });
        });
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (IsVisible)
        {
            if (CurrentState == State.Readying && !IsReadyToRace)
            {
                _drawService.Draw(spriteBatch, KeyBox, KeyBoxPosition);
                _drawService.Draw(spriteBatch, SpriteFont, KeyString, KeyBoxTextPosition);
            }
            _drawService.Draw(spriteBatch, CarTexture, CarPosition);
            _drawService.Draw(spriteBatch, LeftWheel, LeftWheelPosition, WheelRotation);
            _drawService.Draw(spriteBatch, RightWheel, RightWheelPosition, WheelRotation);
            _drawService.Draw(spriteBatch, WheelWellsTexture, CarPosition);
        }
    }
}
