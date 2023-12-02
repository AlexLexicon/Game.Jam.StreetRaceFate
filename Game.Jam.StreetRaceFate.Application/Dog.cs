using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game.Jam.StreetRaceFate.Application;
public class Dog : IGameInitalizable, IGameLoadable, ISpriteBatchDrawable<DogsSpriteBatch>
{
    private readonly IContentManagerService _contentManagerService;

    public Dog(IContentManagerService contentManagerService)
    {
        _contentManagerService = contentManagerService;
    }

    private Texture2D? DogTexture { get; set; }
    private int X { get; set; }
    private int Y { get; set; }

    public void LoadContent()
    {
        DogTexture = _contentManagerService.Load("dog.tan");
    }

    public void Initalize()
    {
        X = Random.Shared.Next(0, 200);
        Y = Random.Shared.Next(0, 200);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(DogTexture, new Rectangle(X, Y, 32, 32), Color.White);
    }
}
