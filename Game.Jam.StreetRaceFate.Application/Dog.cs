using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game.Jam.StreetRaceFate.Application;
public class Dog : IGameLoadable, IGameDrawable
{
    private readonly IContentManagerService _contentManagerService;
    private readonly IGraphicsService _graphicsService;
    private readonly IViewportService _viewportService;

    public Dog(
        IContentManagerService contentManagerService,
        IGraphicsService graphicsService,
        IViewportService viewportService)
    {
        _contentManagerService = contentManagerService;
        _graphicsService = graphicsService;
        _viewportService = viewportService;
    }

    private SpriteBatch? SpriteBatch { get; set; }
    private Texture2D? DogTexture { get; set; }

    public void LoadContent()
    {
        SpriteBatch = _graphicsService.CreateSpriteBatch();

        DogTexture = _contentManagerService.Load("dog.tan");

        x = Random.Shared.Next(0, 100);
        y = Random.Shared.Next(0, 100);
    }

    private int x { get; set; }
    private int  y { get; set; }

    public void Draw(GameTime gameTime)
    {
        //_graphicsService.Clear(Color.Black);

        if (SpriteBatch is not null && DogTexture is not null)
        {
            int viewportWidth = _viewportService.GetViewportWidth();
            int viewportHeight = _viewportService.GetViewportHeight();

            float scaleX = (float)viewportWidth / 640;
            float scaleY = (float)viewportHeight / 480;
            Matrix matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            SpriteBatch.Begin(transformMatrix: matrix);

            SpriteBatch.Draw(DogTexture, new Rectangle(x, y, 32, 32), Color.White);

            SpriteBatch.End();
        }
    }
}
