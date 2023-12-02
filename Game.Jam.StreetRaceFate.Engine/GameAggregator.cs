using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game.Jam.StreetRaceFate.Engine;
public class GameAggregator : Microsoft.Xna.Framework.Game
{
    private readonly IWeakRefrenceManager _weakRefrenceManager;

    public GameAggregator(IWeakRefrenceManager weakRefrenceManager)
    {
        _weakRefrenceManager = weakRefrenceManager;
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
        foreach (var gameDrawable in _weakRefrenceManager.Get<IGameDrawable>())
        {
            gameDrawable.Draw(gameTime);
        }

        base.Draw(gameTime);
    }
}
