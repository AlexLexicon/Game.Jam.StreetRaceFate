using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Application;
public class BackgroundSpriteBatch : ISpriteBatch
{
    private readonly ISpriteBatchFactory _spriteBatchFactory;
    private readonly IViewService _viewService;
    private readonly IWeakRefrenceManager _weakRefrenceManager;
    private readonly ISpriteBatchDrawablePriorityService _spriteBatchDrawablePriorityService;

    public BackgroundSpriteBatch(
        ISpriteBatchFactory spriteBatchFactory,
        IViewService viewService,
        IWeakRefrenceManager weakRefrenceManager,
        ISpriteBatchDrawablePriorityService spriteBatchDrawablePriorityService)
    {
        _spriteBatchFactory = spriteBatchFactory;
        _viewService = viewService;
        _weakRefrenceManager = weakRefrenceManager;
        _spriteBatchDrawablePriorityService = spriteBatchDrawablePriorityService;
    }

    public void LoadContent()
    {
        _spriteBatchFactory.CreateSpriteBatch<BackgroundSpriteBatch>();
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = _spriteBatchFactory.GetSpriteBatch<BackgroundSpriteBatch>();

        var matrix = _viewService.GetViewMatrix();

        spriteBatch.Begin(transformMatrix: matrix);

        var unsortedGameDrawables = _weakRefrenceManager.Get<ISpriteBatchDrawable<BackgroundSpriteBatch>>();

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

        spriteBatch.End();
    }
}
