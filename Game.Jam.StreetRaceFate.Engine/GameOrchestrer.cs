using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Engine;
public class GameOrchestrer : Microsoft.Xna.Framework.Game
{
    private readonly IWeakRefrenceManager _weakRefrenceManager;
    private readonly IDrawablePriorityService _drawablePriorityService;

    public GameOrchestrer(
        IWeakRefrenceManager weakRefrenceManager, 
        IDrawablePriorityService drawablePriorityService)
    {
        _weakRefrenceManager = weakRefrenceManager;
        _drawablePriorityService = drawablePriorityService;
    }

    protected override void Initialize()
    {
        foreach (var gameInitalizable in _weakRefrenceManager.Get<IGameInitalizable>())
        {
            gameInitalizable.Initalize();
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        foreach (var gameLoadable in _weakRefrenceManager.Get<IGameLoadable>())
        {
            gameLoadable.LoadContent();
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back is ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        foreach (var gameUpdatable in _weakRefrenceManager.Get<IGameUpdatable>())
        {
            gameUpdatable.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var unsortedGameDrawables = _weakRefrenceManager.Get<IGameDrawable>();
        //todo - this is really bad to loop twice here but I am lazy and dont have time to worry about that performance right now
        var sortedGameDrawables = new SortedList<int, List<IGameDrawable>>();
        foreach (var gameDrawable in unsortedGameDrawables)
        {
            int priority = _drawablePriorityService.GetDrawablePriority(gameDrawable);

            if (sortedGameDrawables.TryGetValue(priority, out var drawables))
            {
                drawables.Add(gameDrawable);
            }
            else
            {
                sortedGameDrawables.Add(priority, new List<IGameDrawable> 
                {
                    gameDrawable 
                });
            }
        }
        foreach (var gameDrawables in sortedGameDrawables.Values)
        {
            foreach (var gameDrawable in gameDrawables)
            {
                gameDrawable.Draw(gameTime);
            }
        }

        base.Draw(gameTime);
    }
}
