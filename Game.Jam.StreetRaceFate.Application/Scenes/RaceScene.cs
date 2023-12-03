﻿using Game.Jam.StreetRaceFate.Application.Service;
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

    public RaceScene(
        IGameObjectFactory gameObjectFactory,
        IGraphicsService graphicsService,
        IKeysService keysService,
        IDelayService delayService,
        RaceText raceText)
    {
        _gameObjectFactory = gameObjectFactory;
        _graphicsService = graphicsService;

        KeyToCar = new Dictionary<Keys, Car>();
        _keysService = keysService;
        _delayService = delayService;
        _raceText = raceText;
    }

    private Dictionary<Keys, Car> KeyToCar { get; }

    public void Initalize()
    {

    }

    private bool IsReadyToRace { get; set; }
    private int ReadyCountdown { get; set; }
    private bool RaceStarted { get; set; }

    public void Update(GameTime gameTime)
    {
        if (!RaceStarted)
        {
            KeyboardState state = Keyboard.GetState();

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

            if (KeyToCar.Count is < ViewService.MAX_NUM_OF_PLAYERS)
            {
                foreach (var key in _keysService.GetValidKeys())
                {
                    AddCar(state, key);
                }
            }

            _delayService.Delay(gameTime, 1.25f, () =>
            {
                ReadyCountdown--;
                if (ReadyCountdown is <= 0)
                {
                    IsReadyToRace = false;
                    RaceStarted = true;
                }
            });
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
            row++;
            car.RowIndex = row;
            car.Key = key;
            car.Spawn();

            KeyToCar.Add(key, car);
        }
    }

    public void Draw(GameTime gameTime)
    {
        _graphicsService.Clear(Color.Blue);
    }
}