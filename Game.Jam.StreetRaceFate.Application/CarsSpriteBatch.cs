using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Application;
public class CarsSpriteBatch : ISpriteBatch
{
    private readonly ISpriteBatchFactory _spriteBatchFactory;
    private readonly IWeakRefrenceManager _weakRefrenceManager;
    private readonly IViewService _viewService;
    private readonly ISpriteBatchDrawablePriorityService _spriteBatchDrawablePriorityService;

    public CarsSpriteBatch(
        ISpriteBatchFactory spriteBatchFactory,
        IWeakRefrenceManager weakRefrenceManager,
        IViewService viewService,
        ISpriteBatchDrawablePriorityService spriteBatchDrawablePriorityProvider)
    {
        _spriteBatchFactory = spriteBatchFactory;
        _weakRefrenceManager = weakRefrenceManager;
        _viewService = viewService;
        _spriteBatchDrawablePriorityService = spriteBatchDrawablePriorityProvider;
    }

    public void LoadContent()
    {
        _spriteBatchFactory.CreateSpriteBatch<CarsSpriteBatch>();
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = _spriteBatchFactory.GetSpriteBatch<CarsSpriteBatch>();

        var matrix = _viewService.GetViewMatrix();

        spriteBatch.Begin(transformMatrix: matrix);

        var unsortedGameDrawables = _weakRefrenceManager.Get<ISpriteBatchDrawable<CarsSpriteBatch>>();

        //todo - this is really bad to loop twice here but I am lazy and dont have time to worry about that performance right now
        var sortedGameDrawables = new SortedList<int, List<ISpriteBatchDrawable>>();
        foreach (var gameDrawable in unsortedGameDrawables)
        {
            int priority = _spriteBatchDrawablePriorityService.GetDrawablePriority(gameDrawable);

            if (sortedGameDrawables.TryGetValue(priority, out var drawables))
            {
                drawables.Add(gameDrawable);
            }
            else
            {
                sortedGameDrawables.Add(priority, new List<ISpriteBatchDrawable>
                {
                    gameDrawable
                });
            }
        }
        foreach (var gameDrawables in sortedGameDrawables.Values)
        {
            foreach (var gameDrawable in gameDrawables)
            {
                gameDrawable.Draw(gameTime, spriteBatch);
            }
        }
        foreach (var gameDrawables in sortedGameDrawables.Values)
        {
            foreach (var gameDrawable in gameDrawables)
            {
                if (gameDrawable is Car c)
                {
                    c.DrawLater(gameTime, spriteBatch);
                }
            }
        }

        spriteBatch.End();
    }
}
