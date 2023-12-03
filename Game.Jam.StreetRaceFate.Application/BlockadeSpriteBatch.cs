using Game.Jam.StreetRaceFate.Application.Service;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Factories;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class BlockadeSpriteBatch : ISpriteBatch
{
    private readonly ISpriteBatchFactory _spriteBatchFactory;
    private readonly IWeakRefrenceManager _weakRefrenceManager;
    private readonly IViewService _viewService;

    public BlockadeSpriteBatch(
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
        _spriteBatchFactory.CreateSpriteBatch<BlockadeSpriteBatch>();
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = _spriteBatchFactory.GetSpriteBatch<BlockadeSpriteBatch>();

        var matrix = _viewService.GetViewMatrix();

        spriteBatch.Begin(transformMatrix: matrix);

        foreach (ISpriteBatchDrawable<BlockadeSpriteBatch> spriteBatchDrawable in _weakRefrenceManager.Get<ISpriteBatchDrawable<BlockadeSpriteBatch>>())
        {
            spriteBatchDrawable.Draw(gameTime, spriteBatch);
        }

        spriteBatch.End();
    }
}