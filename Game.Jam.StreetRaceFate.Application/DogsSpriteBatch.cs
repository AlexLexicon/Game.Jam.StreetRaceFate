using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Application;
public class DogsSpriteBatch : IGameLoadable, IGameDrawable
{
    private readonly IViewportService _viewportService;
    private readonly ISpriteBatchFactory _spriteBatchFactory;
    private readonly IWeakRefrenceManager _weakRefrenceManager;

    public DogsSpriteBatch(
        IViewportService viewportService,
        ISpriteBatchFactory spriteBatchFactory,
        IWeakRefrenceManager weakRefrenceManager)
    {
        _viewportService = viewportService;
        _spriteBatchFactory = spriteBatchFactory;
        _weakRefrenceManager = weakRefrenceManager;
    }

    public void LoadContent()
    {
        _spriteBatchFactory.CreateSpriteBatch<DogsSpriteBatch>();
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = _spriteBatchFactory.GetSpriteBatch<DogsSpriteBatch>();

        int viewportWidth = _viewportService.GetViewportWidth();
        int viewportHeight = _viewportService.GetViewportHeight();

        float scaleX = (float)viewportWidth / 640;
        float scaleY = (float)viewportHeight / 480;
        Matrix matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

        spriteBatch.Begin(transformMatrix: matrix);

        var dogs = _weakRefrenceManager.Get<ISpriteBatchDrawable<DogsSpriteBatch>>();
        foreach (ISpriteBatchDrawable<DogsSpriteBatch> spriteBatchDrawable in dogs)
        {
            spriteBatchDrawable.Draw(gameTime, spriteBatch);
        }

        spriteBatch.End();
    }
}
