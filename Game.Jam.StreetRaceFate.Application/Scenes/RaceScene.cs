using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Game.Jam.StreetRaceFate.Application.Scenes;
public class RaceScene : IGameScene
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
        Lights lights)
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
    }

    private Dictionary<Keys, Car> KeyToCar { get; }

    public void Initalize()
    {

    }

    private bool IsReadyToRace { get; set; }
    private int ReadyCountdown { get; set; }

    public void Update(GameTime gameTime)
    {
        _raceText.IsVisible = IsReadyToRace;
        _raceText.Text = ReadyCountdown.ToString();

        if (!_raceService.IsRaceOver())
        {
            if (!_raceService.IsRaceStarted())
            {
                KeyboardState state = Keyboard.GetState();

                int count = KeyToCar.Count;

                if (count is > 0)
                {
                    bool isReadyToRace = KeyToCar.Values.All(c => c.IsReadyToRace);
                    if (isReadyToRace && !IsReadyToRace)
                    {
                        IsReadyToRace = true;
                        ReadyCountdown = 3;
                    }
                    else if (!isReadyToRace && IsReadyToRace)
                    {
                        IsReadyToRace = false;
                    }
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
                        }
                    });
                }
            }
            else
            {
                _delayService.Delay(gameTime, 20.2f, () =>
                {
                    _raceService.StopRace();
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

            int row = count switch
            {
                0 => 5,
                1 => 3,
                2 => 7,
                3 => 1,
                4 => 9,
                5 => 4,
                6 => 6,
                7 => 2,
                8 => 8,
                9 => 0,
                10 => 10,
                _ => count,
            };
            row += 5;
            car.RowIndex = row;
            car.Key = key;
            car.Spawn();

            KeyToCar.Add(key, car);
        }
    }

    private Color Bg { get; } = new Color(240, 205, 153);

    public void Draw(GameTime gameTime)
    {
        _graphicsService.Clear(Bg);
    }
}
