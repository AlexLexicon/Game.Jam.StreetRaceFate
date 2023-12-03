using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Game.Jam.StreetRaceFate.Application.Scenes;
public class RaceScene : IGameScene, IEndDraw
{
    private readonly IGameObjectFactory _gameObjectFactory;
    private readonly IGraphicsService _graphicsService;
    private readonly IKeysService _keysService;
    private readonly IDelayService _delayService;
    private readonly RaceText _raceText;
    private readonly Sky _sky;
    private readonly Sun _sun;
    private readonly CityFar _cityFar;
    private readonly CityClose _cityClose;
    private readonly Road _road;
    private readonly Rail _rail;
    private readonly IRaceService _raceService;
    private readonly Trees _trees;
    private readonly TreesShadows _treesShadows;
    private readonly Lights _lights;
    private readonly IViewportService _viewportService;
    private readonly IContentManagerService _contentManagerService;
    private readonly News _news;

    public RaceScene(
        IGameObjectFactory gameObjectFactory,
        IGraphicsService graphicsService,
        IKeysService keysService,
        IDelayService delayService,
        RaceText raceText,
        Road road,
        Sky sky,
        Sun sun,
        CityFar cityFar,
        CityClose cityClose,
        Rail rail,
        IRaceService raceService,
        Trees trees,
        TreesShadows treesShadows,
        Lights lights,
        IViewportService viewportService,
        IContentManagerService contentManagerService,
        News news)
    {
        _gameObjectFactory = gameObjectFactory;
        _graphicsService = graphicsService;

        KeyToCar = new Dictionary<Keys, Car>();
        _keysService = keysService;
        _delayService = delayService;
        _raceText = raceText;
        _road = road;
        _sky = sky;
        _sun = sun;
        _cityFar = cityFar;
        _cityClose = cityClose;
        _rail = rail;
        _raceService = raceService;
        _trees = trees;
        _treesShadows = treesShadows;
        _lights = lights;
        _viewportService = viewportService;
        _contentManagerService = contentManagerService;
        _news = news;
    }

    private Dictionary<Keys, Car> KeyToCar { get; }

    public void Initalize()
    {
        for (int i = 0; i < 2; i++)
        {
            var c1 = _gameObjectFactory.Create<Crowd>();
            c1.Spawn(0, i == 0);
            var c2 = _gameObjectFactory.Create<Crowd>();
            c2.Spawn(1, i == 0);
            var c3 = _gameObjectFactory.Create<Crowd>();
            c3.Spawn(2, i == 0);
        }
    }

    private void Boo()
    {
        var a = _contentManagerService.LoadSoundEffect("booing");
        var x = a.CreateInstance();
        x.Volume = 1f;
        x.Play();
    }

    private void Clap()
    {
        var a = _contentManagerService.LoadSoundEffect("clapping");
        var x = a.CreateInstance();
        x.Volume = 1f;
        x.Play();
    }

    private void Camera()
    {
        IsFlash = true;
        var a = _contentManagerService.LoadSoundEffect("camera");
        var x = a.CreateInstance();
        x.Volume = 1f;
        x.Play();
    }

    private void Starting()
    {
        //var a = _contentManagerService.LoadSoundEffect("start");
        //var x = a.CreateInstance();
        //x.Volume = 1f;
        //x.Play();
    }

    private bool IsReadyToRace { get; set; }
    private int ReadyCountdown { get; set; }

    private bool RaceStarted { get; set; }
    private bool WinnerFound { get; set; }

    public void Update(GameTime gameTime)
    {
        if (IsFlash)
        {
            _delayService.Delay(gameTime, 0.01f, () =>
            {
                IsFlash = false;
            });
        }

        _raceText.IsVisible = IsReadyToRace;
        if (IsReadyToRace)
        {
            _raceText.MediumText = null;
            _raceText.BigText = ReadyCountdown.ToString();
        }
        else
        {
            _raceText.BigText = null;
        }

        if (_raceService.IsRacing())
        {
            bool allStopped = KeyToCar.Values.All(c => c.IsStopped);
            if (allStopped)
            {
                _raceService.StopRace();
            }
        }

        if (_raceService.IsRaceOver() && RaceStarted && !WinnerFound)
        {
            Car? winningCar = null;
            float winningDis = float.MaxValue;
            var explodedCars = new List<Car>();
            foreach (var car in KeyToCar.Values)
            {
                if (car.IsExploded)
                {
                    car.IsDeath = true;
                    explodedCars.Add(car);
                }
                else if (car.IsStopped)
                {
                    car.IsLoser = true;

                    float dis = Vector2.Distance(car.Position, new Vector2(_viewportService.GetViewportWidth(), car.Position.Y));
                    if (dis < winningDis)
                    {
                        winningCar = car;
                        winningDis = dis;
                    }
                }
            }

            if (explodedCars.Count is 1)
            {
                explodedCars.First().IsWinner = true;
                Clap();
                _news.IsEpic = true;
            }
            else if (winningCar is not null && explodedCars.Count is > 0)
            {
                winningCar.IsWinner = true;
                _news.IsCool = true;
            }
            else
            {
                if (explodedCars.Count is <= 0)
                {
                    Boo();
                    _sky.PlayChill();
                    _news.IsLame = true;
                }
            }

            WinnerFound = true;
        }

        if (!_raceService.IsRaceOver())
        {
            if (!_raceService.IsRaceStarted())
            {
                KeyboardState state = Keyboard.GetState();

                int count = KeyToCar.Count;

                if (count is > 2)
                {
                    bool isReadyToRace = KeyToCar.Values.All(c => c.IsReadyToRace);
                    if (isReadyToRace && !IsReadyToRace)
                    {
                        foreach (var c in KeyToCar.Values)
                        {
                            c.Rev();
                        }
                        IsReadyToRace = true;
                        ReadyCountdown = 3;
                        Starting();
                    }
                    else if (!isReadyToRace && IsReadyToRace)
                    {
                        IsReadyToRace = false;
                    }
                }
                else
                {
                    _raceText.MediumText = $"{3 - count} MORE PLAYER(S) MUST JOIN BEFORE RACE CAN BEGIN!";
                    _raceText.IsVisible = true;
                }

                if (count is < ViewService.MAX_NUM_OF_PLAYERS)
                {
                    foreach (var key in _keysService.GetValidKeys())
                    {
                        AddCar(state, key);
                    }
                }

                if (IsReadyToRace)
                {
                    _delayService.Delay(gameTime, 1.25f, () =>
                    {
                        ReadyCountdown--;
                        if (ReadyCountdown is <= 0)
                        {
                            IsReadyToRace = false;
                            _raceService.StartRace();
                            RaceStarted = true;
                        }
                    });
                }
            }
            else
            {
                _delayService.Delay(gameTime, 20.2f, () =>
                {
                    _raceService.StopRace();
                    _sky.PlayCrowd();
                    Camera();
                });
            }
        }
    }

    private void AddCar(KeyboardState state, Keys key)
    {
        if (state.IsKeyDown(key) && !KeyToCar.ContainsKey(key))
        {
            SpawnCar(key);
        }
    }

    private void SpawnCar(Keys key)
    {
        int count = KeyToCar.Count;
        if (count is < ViewService.MAX_NUM_OF_PLAYERS)
        {
            var car = _gameObjectFactory.Create<Car>();

            (int row, Car.Bodies body) = count switch
            {
                0 => (5, Car.Bodies.Orange),
                1 => (3, Car.Bodies.Green),
                2 => (7, Car.Bodies.Purple),
                3 => (1, Car.Bodies.Red),
                4 => (9, Car.Bodies.Blue),
                5 => (4, Car.Bodies.Black),
                6 => (6, Car.Bodies.Silver),
                7 => (2, Car.Bodies.Spray),
                8 => (8, Car.Bodies.White),
                9 => (0, Car.Bodies.Gold),
                10 => (10, Car.Bodies.Brown),
                _ => (count, Car.Bodies.Orange),
            };
            row += 5;
            car.RowIndex = row;
            car.Key = key;
            car.Spawn(body);

            KeyToCar.Add(key, car);
        }
    }

    private Color Bg { get; } = new Color(240, 205, 153);

    public void Draw(GameTime gameTime)
    {
        _graphicsService.Clear(Bg);
    }

    private bool IsFlash { get; set; }

    public void EndDraw(GameTime gameTime)
    {
        if (IsFlash)
        {
            _graphicsService.Clear(Color.White);
        }
    }
}
