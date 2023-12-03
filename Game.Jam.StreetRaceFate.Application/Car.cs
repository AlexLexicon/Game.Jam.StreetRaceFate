using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
    private readonly IGraphicsDeviceManagerService _graphicsDeviceManagerService;
    private readonly IGameObjectFactory _gameObjectFactory;

    public Car(
        IContentManagerService contentManagerService,
        IMovementService movementService,
        IDrawService drawService,
        IDelayService delayService,
        IOscillateService oscillateService,
        IKeysService keysService,
        IRaceService raceService,
        IGraphicsDeviceManagerService graphicsDeviceManagerService,
        IGameObjectFactory gameObjectFactory)
    {
        _contentManagerService = contentManagerService;
        _movementService = movementService;
        _drawService = drawService;
        _delayService = delayService;
        _oscillateService = oscillateService;

        Speed = 0.25f;
        _keysService = keysService;
        _raceService = raceService;
        _graphicsDeviceManagerService = graphicsDeviceManagerService;
        _gameObjectFactory = gameObjectFactory;
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
        RaceOver,
    }

    public enum Bodies
    {
        Blue,
        Brown,
        Orange,
        Gold,
        Green,
        Purple,
        Silver,
        Spray,
        Red,
        Black,
        White,
    }

    private CarBody Body { get; set; }
    public Texture2D ThrottleTexture { get; set; }
    private Texture2D TrophyTexture { get; set; }
    private Texture2D DeathTexture { get; set; }
    private Texture2D LoserTexture { get; set; }
    private SpriteFont SpriteFont { get; set; }

    private float Speed { get; set; }
    public Vector2 Position { get; private set; }
    private Vector2 TargetPosition { get; set; }

    private Vector2 CarPosition => new Vector2(Position.X + CarXOscillation, Position.Y + CarYOscillation);
    private Vector2 LeftWheelPosition => new Vector2(Position.X + Body.LeftWheelX + CarXOscillation, Position.Y + Body.WheelY);
    private Vector2 RightWheelPosition => new Vector2(Position.X + Body.RightWheelX + CarXOscillation, Position.Y + Body.WheelY);
    private Vector2 ThrottlePosition => new Vector2(Position.X + Body.Width + 21, Position.Y);
    private Vector2 ThrottleTextPosition => new Vector2(ThrottlePosition.X + ThrottleTexture.Width / 2 + 16, ThrottlePosition.Y + ThrottleTexture.Height / 2);
    private Vector2 Center => new Vector2(Position.X + Body.Width / 2, Position.Y + Body.Height / 2);
    private float WheelRotation { get; set; }
    private int CarYOscillation { get; set; }
    private int CarXOscillation { get; set; }
    private bool IsVisible { get; set; }
    public bool IsReadyToRace { get; private set; }

    private Vector2 TrophyPosition => new Vector2(Math.Max(21, Position.X + Body.Width + 21), Position.Y);
    private Vector2 TextTrophyPosition => new Vector2(TrophyPosition.X + TrophyTexture.Width / 2 + 16, TrophyPosition.Y + TrophyTexture.Height / 2);
    private Vector2 DeathPosition => new Vector2(Position.X + Body.Width + 21, Position.Y);
    private Vector2 TextDeathPosition => new Vector2(DeathPosition.X + DeathTexture.Width / 2 + 16, DeathPosition.Y + DeathTexture.Height / 2);
    private Vector2 LoserPosition => new Vector2(Position.X + Body.Width + 21, Position.Y);
    private Vector2 TextLoserPosition => new Vector2(LoserPosition.X + LoserTexture.Width / 2 + 16, LoserPosition.Y + LoserTexture.Height / 2);

    private int MinXOsc { get; set; }
    private int MaxXOsc { get; set; }

    private bool StopOsc { get; set; }

    public bool IsExploded { get; private set; }

    public bool IsStopped { get; private set; }

    public bool IsDeath { get; set; }
    public bool IsWinner { get; set; }
    public bool IsLoser { get; set; }

    public void LoadContent()
    {
        ThrottleTexture = _contentManagerService.LoadTexture2D("throttle.big");
        SpriteFont = _contentManagerService.LoadSpriteFont("Normal");
        TrophyTexture = _contentManagerService.LoadTexture2D("trophy.big");
        DeathTexture = _contentManagerService.LoadTexture2D("death.big");
        LoserTexture = _contentManagerService.LoadTexture2D("lose.big");
    }

    public void Initalize()
    {
        CurrentState = State.New;

        IsVisible = false;
    }

    public void Spawn(Bodies body)
    {
        Body = body switch
        {
            Bodies.Blue => new CarBody(body, leftWheelX: 12, rightWheelX: 91, wheelY: 22),
            Bodies.Brown => new CarBody(body, leftWheelX: 18, rightWheelX: 95, wheelY: 23),
            Bodies.Orange => new CarBody(body, leftWheelX: 13, rightWheelX: 88, wheelY: 21),
            Bodies.Gold => new CarBody(body, leftWheelX: 16, rightWheelX: 87, wheelY: 21),
            Bodies.Green => new CarBody(body, leftWheelX: 22, rightWheelX: 94, wheelY: 21),
            Bodies.Purple => new CarBody(body, leftWheelX: 19, rightWheelX: 92, wheelY: 21),
            Bodies.Silver => new CarBody(body, leftWheelX: 15, rightWheelX: 86, wheelY: 22),
            Bodies.Spray => new CarBody(body, leftWheelX: 16, rightWheelX: 90, wheelY: 21),
            Bodies.Red => new CarBody(body, leftWheelX: 10, rightWheelX: 92, wheelY: 21),
            Bodies.Black => new CarBody(body, leftWheelX: 23, rightWheelX: 95, wheelY: 21),
            _ => new CarBody(body, leftWheelX: 14, rightWheelX: 91, wheelY: 23),//white
        };

        Body.Load(_contentManagerService);

        MoveToStart();

        IsVisible = true;
        IsReadyToRace = false;

        MinXOsc = Random.Shared.Next(-16, 0);
        MaxXOsc = Random.Shared.Next(0, 16);
    }

    private void MoveToStart()
    {
        float y = RowIndex * (Body.Height + ViewService.CAR_PADDING);
        Position = new Vector2(-Body.Width, y);
        TargetPosition = new Vector2(42, y);
        Speed = 0.025f;
    }

    private bool RaceFinishReset { get; set; }

    public void Update(GameTime gameTime)
    {
        if (_raceService.IsRaceOver())
        {
            if ((IsWinner || IsLoser) && !RaceFinishReset)
            {
                RaceFinishReset = true;
                MoveToStart();
            }
        }

        if (CurrentState != State.Racing && _raceService.IsRacing())
        {
            CurrentState = State.Racing;
            TargetPosition = new Vector2(_graphicsDeviceManagerService.GetResolution().X / 2 - Body.Width / 2, Position.Y);
            Speed = (float)Random.Shared.Next(5, 7) * 0.001f;
        }
        else if (CurrentState == State.Racing && _raceService.IsRaceOver())
        {
            CurrentState = State.RaceOver;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key))
            {
                if (!IsExploded)
                {
                    Explode();
                    TargetPosition = new Vector2(Position.X + 25, Position.Y);
                }
            }
        }

        if (CurrentState is State.Readying or State.Racing)
        {
            var state = Keyboard.GetState();
            var keyReleased = state.IsKeyUp(Key);
            if (CurrentState == State.Readying)
            {
                IsReadyToRace = !keyReleased;
            }
            else if (CurrentState == State.Racing && keyReleased)
            {
                IsStopped = true;
            }
        }

        bool reachedTarget = false;
        if (_raceService.IsRaceOver() || !IsStopped)
        {
            Position = _movementService.MoveTo(gameTime, Position, TargetPosition, 1f, Speed, out reachedTarget);
        }
        else
        {
            Position = new Vector2(Position.X - _raceService.GetRailSpeed(), Position.Y);
        }

        if (reachedTarget && CurrentState == State.New)
        {
            CurrentState = State.Readying;
        }
        else
        {
            float s1 = Vector2.Distance(Position, TargetPosition) / 750;
            if (IsStopped)
            {
                WheelRotation = 0;
            }
            else if (CurrentState == State.Racing)
            {
                float s2 = _raceService.GetRailSpeed();
                WheelRotation += Math.Max(s1, s2);
            }
            else
            {
                WheelRotation += s1;
            }
        }

        if (CurrentState == State.Racing && !StopOsc)
        {
            _delayService.Delay(gameTime, 12f, () =>
            {
                StopOsc = true;
            });
        }

        if (!IsDeath)
        {
            _delayService.Delay(gameTime, (1 - Speed) / 4, () =>
            {
                _oscillateService.Oscillate(0, 2, o =>
                {
                    CarYOscillation = o;
                });
                if (CurrentState == State.Racing)
                {
                    _oscillateService.Oscillate(MinXOsc, MaxXOsc, o =>
                    {
                        if (!StopOsc || CarXOscillation != 0)
                        {
                            CarXOscillation = o;
                        }
                    });
                }
            });
        }
        else
        {
            CarYOscillation = 3;
        }
    }

    public void Explode()
    {
        IsExploded = true;
        var r = Random.Shared;
        int total = r.Next(6, 15);
        for (int count = 0; count < total; count++)
        {
            var ex = _gameObjectFactory.Create<Explosion>();

            var xx = r.Next(-Body.Width / 2, Body.Width / 2);
            var yy = r.Next(-Body.Height / 2, Body.Height / 2);

            Vector2 pos = new Vector2(Center.X + xx, Center.Y + yy);

            ex.Spawn(pos, (float)count / 5);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (IsVisible)
        {
            if (CurrentState == State.Readying && !IsReadyToRace)
            {
                _drawService.Draw(spriteBatch, ThrottleTexture, ThrottlePosition);
                _drawService.Draw(spriteBatch, SpriteFont, KeyString, ThrottleTextPosition, Color.White);
            }
            if (IsWinner)
            {
                _drawService.Draw(spriteBatch, TrophyTexture, TrophyPosition);
                _drawService.Draw(spriteBatch, SpriteFont, KeyString, TextTrophyPosition, Color.White);
            }
            else if (IsDeath)
            {
                _drawService.Draw(spriteBatch, DeathTexture, DeathPosition);
                _drawService.Draw(spriteBatch, SpriteFont, KeyString, TextDeathPosition, Color.White);
            }
            else if (IsLoser)
            {
                _drawService.Draw(spriteBatch, LoserTexture, LoserPosition);
                _drawService.Draw(spriteBatch, SpriteFont, KeyString, TextLoserPosition, Color.White);
            }
            Body.Draw(_drawService, spriteBatch, CarPosition, LeftWheelPosition, RightWheelPosition, WheelRotation);
        }
    }

    private class CarBody
    {
        private readonly Bodies _body;
        public CarBody(Bodies body, int leftWheelX, int rightWheelX, int wheelY)
        {
            _body = body;

            LeftWheelX = leftWheelX;
            RightWheelX = rightWheelX;
            WheelY = wheelY;
        }

        public int LeftWheelX { get; }
        public int RightWheelX { get; }
        public int WheelY { get; }

        public int Width => CarTexture.Width;
        public int Height => CarTexture.Height;

        private Texture2D CarTexture { get; set; }
        private Texture2D WheelWellsTexture { get; set; }
        private Texture2D LeftWheel { get; set; }
        private Texture2D RightWheel { get; set; }

        public void Load(IContentManagerService cms)
        {
            string carname = _body switch
            {
                Bodies.Blue => "blue",
                Bodies.Brown => "brown",
                Bodies.Orange => "orange",
                Bodies.Gold => "gold",
                Bodies.Green => "green",
                Bodies.Purple => "purple",
                Bodies.Silver => "silver",
                Bodies.Spray => "spray",
                Bodies.Red => "red",
                Bodies.Black => "black",
                _ => "white",
            };

            CarTexture = cms.LoadTexture2D($"car.{carname}");
            WheelWellsTexture = cms.LoadTexture2D($"car.{carname}.well");
            LeftWheel = cms.LoadTexture2D($"car.{carname}.tire");
            RightWheel = cms.LoadTexture2D($"car.{carname}.tire");
        }

        public void Draw(IDrawService ds, SpriteBatch spriteBatch, Vector2 carPosition, Vector2 lwPosition, Vector2 rwPosition, float wRotation)
        {
            ds.Draw(spriteBatch, CarTexture, carPosition);
            ds.Draw(spriteBatch, LeftWheel, lwPosition, wRotation);
            ds.Draw(spriteBatch, RightWheel, rwPosition, wRotation);
            ds.Draw(spriteBatch, WheelWellsTexture, carPosition);
        }
    }
}
