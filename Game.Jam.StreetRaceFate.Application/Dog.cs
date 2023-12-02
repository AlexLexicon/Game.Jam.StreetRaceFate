using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game.Jam.StreetRaceFate.Application;
public class Dog : IGameInitalizable, IGameLoadable, ISpriteBatchDrawable<DogsSpriteBatch>, IGameUpdatable
{
    private readonly IContentManagerService _contentManagerService;

    public Dog(IContentManagerService contentManagerService)
    {
        _contentManagerService = contentManagerService;
    }

    private Texture2D? DogTexture { get; set; }
    private Vector2 Position { get; set; }
    private Vector2 TargetPosition { get; set; }

    public void LoadContent()
    {
        DogTexture = _contentManagerService.LoadTexture2D("dog.tan");
    }

    public void Initalize()
    {
        int x = Random.Shared.Next(0, 600);
        int y = Random.Shared.Next(0, 400);
        Position = new Vector2(x, y);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(DogTexture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), Color.White);
    }

    public void Update(GameTime gameTime)
    {
        float dis = Vector2.Distance(Position, TargetPosition);

        if (dis  < 0.5f)
        {
            int rx = Random.Shared.Next(0, 600);
            int ry = Random.Shared.Next(0, 400);
            TargetPosition = new Vector2(rx, ry);
        }
        else
        {
            Position = Vector2.Lerp(Position, TargetPosition, 0.01f);
        }
    }
}
