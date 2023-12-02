using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public class GameAggregator : Microsoft.Xna.Framework.Game
{
    private readonly IWeakRefrenceManager _weakRefrenceManager;

    public GameAggregator(IWeakRefrenceManager weakRefrenceManager)
    {
        _weakRefrenceManager = weakRefrenceManager;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        foreach (var loadableEntity in _weakRefrenceManager.Get<IGameLoadable>())
        {
            loadableEntity.LoadContent();
        }
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back is ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        foreach (var drawableEntity in _weakRefrenceManager.Get<IGameDrawable>())
        {
            drawableEntity.Draw(gameTime);
        }

        base.Draw(gameTime);
    }
}
