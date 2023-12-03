using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class RaceTextSpriteBatch : ISpriteBatch
{
    private readonly ISpriteBatchFactory _spriteBatchFactory;
    private readonly IViewService _viewService;
    private readonly IWeakRefrenceManager _weakRefrenceManager;

    public RaceTextSpriteBatch(
        ISpriteBatchFactory spriteBatchFactory,
        IViewService viewService,
        IWeakRefrenceManager weakRefrenceManager)
    {
        _spriteBatchFactory = spriteBatchFactory;
        _viewService = viewService;
        _weakRefrenceManager = weakRefrenceManager;
    }

    public void LoadContent()
    {
        _spriteBatchFactory.CreateSpriteBatch<RaceTextSpriteBatch>();
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = _spriteBatchFactory.GetSpriteBatch<RaceTextSpriteBatch>();

        var matrix = _viewService.GetViewMatrix();

        spriteBatch.Begin(transformMatrix: matrix);

        foreach (ISpriteBatchDrawable<RaceTextSpriteBatch> spriteBatchDrawable in _weakRefrenceManager.Get<ISpriteBatchDrawable<RaceTextSpriteBatch>>())
        {
            spriteBatchDrawable.Draw(gameTime, spriteBatch);
        }

        spriteBatch.End();
    }
}
