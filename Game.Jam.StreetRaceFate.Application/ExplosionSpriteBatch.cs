using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class ExplosionSpriteBatch : ISpriteBatch
{
    private readonly ISpriteBatchFactory _spriteBatchFactory;
    private readonly IWeakRefrenceManager _weakRefrenceManager;
    private readonly IViewService _viewService;

    public ExplosionSpriteBatch(
        ISpriteBatchFactory spriteBatchFactory,
        IWeakRefrenceManager weakRefrenceManager,
        IViewService viewService)
    {
        _spriteBatchFactory = spriteBatchFactory;
        _weakRefrenceManager = weakRefrenceManager;
        _viewService = viewService;
    }

    public void LoadContent()
    {
        _spriteBatchFactory.CreateSpriteBatch<ExplosionSpriteBatch>();
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = _spriteBatchFactory.GetSpriteBatch<ExplosionSpriteBatch>();

        var matrix = _viewService.GetViewMatrix();

        spriteBatch.Begin(transformMatrix: matrix);

        foreach (ISpriteBatchDrawable<ExplosionSpriteBatch> spriteBatchDrawable in _weakRefrenceManager.Get<ISpriteBatchDrawable<ExplosionSpriteBatch>>())
        {
            spriteBatchDrawable.Draw(gameTime, spriteBatch);
        }

        spriteBatch.End();
    }
}
