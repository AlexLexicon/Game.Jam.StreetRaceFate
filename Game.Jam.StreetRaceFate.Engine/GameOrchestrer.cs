using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Engine;
public interface IGameOrchestrer
{
    void AddInitializable(IGameInitalizable gameInitalizable);
    void AddLoadable(IGameLoadable gameLoadable);
    void AddUpdatable(IGameUpdatable gameUpdatable);
    void AddDrawable(IGameDrawable gameDrawable);
}
public class GameOrchestrer : Microsoft.Xna.Framework.Game, IGameOrchestrer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWeakRefrenceManager _weakRefrenceManager;
    private readonly IDrawablePriorityService _drawablePriorityService;

    public GameOrchestrer(
        IServiceProvider serviceProvider,
        IWeakRefrenceManager weakRefrenceManager,
        IDrawablePriorityService drawablePriorityService)
    {
        _serviceProvider = serviceProvider;
        _weakRefrenceManager = weakRefrenceManager;
        _drawablePriorityService = drawablePriorityService;

        GameInitalizables = new List<IGameInitalizable>();
        GameLoadables = new List<IGameLoadable>();
        GameUpdatables = new List<IGameUpdatable>();
        GameDrawables = new List<IGameDrawable>();
    }

    private List<IGameInitalizable> GameInitalizables { get; }
    private List<IGameLoadable> GameLoadables { get; }
    private List<IGameUpdatable> GameUpdatables { get; }
    private List<IGameDrawable> GameDrawables { get; }

    public void AddInitializable(IGameInitalizable gameInitalizable)
    {
        GameInitalizables.Add(gameInitalizable);
    }

    public void AddLoadable(IGameLoadable gameLoadable)
    {
        GameLoadables.Add(gameLoadable);
    }

    public void AddUpdatable(IGameUpdatable gameUpdatable)
    {
        GameUpdatables.Add(gameUpdatable);
    }

    public void AddDrawable(IGameDrawable gameDrawable)
    {
        GameDrawables.Add(gameDrawable);
    }

    protected override void Initialize()
    {
        var scenes = _weakRefrenceManager.Get<IGameScene>();
        foreach (var scene in scenes)
        {
            scene.Initalize();
        }

        var gameInitalizables = _weakRefrenceManager.Get<IGameInitalizable>();
        gameInitalizables.AddRange(GameInitalizables);

        foreach (var gameInitalizable in gameInitalizables)
        {
            gameInitalizable.Initalize();
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _serviceProvider.GetService<IEnumerable<ISpriteBatch>>();

        var gameLoadables = _weakRefrenceManager.Get<IGameLoadable>();
        gameLoadables.AddRange(GameLoadables);

        foreach (var gameLoadable in gameLoadables)
        {
            gameLoadable.LoadContent();
        }
    }

    protected override void Update(GameTime gameTime)
    {
        var scenes = _weakRefrenceManager.Get<IGameScene>();
        foreach (var scene in scenes)
        {
            scene.Update(gameTime);
        }

        var gameUpdatables = _weakRefrenceManager.Get<IGameUpdatable>();
        gameUpdatables.AddRange(GameUpdatables);

        foreach (var gameUpdatable in gameUpdatables)
        {
            gameUpdatable.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var scenes = _weakRefrenceManager.Get<IGameScene>();
        foreach (var scene in scenes)
        {
            scene.Draw(gameTime);
        }

        var unsortedGameDrawables = _weakRefrenceManager.Get<IGameDrawable>();
        unsortedGameDrawables.AddRange(GameDrawables);

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
