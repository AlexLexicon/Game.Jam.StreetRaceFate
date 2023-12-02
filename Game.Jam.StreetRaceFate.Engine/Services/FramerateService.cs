using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public class FramerateService : IGameLoadable, IGameUpdatable, ISpriteBatchDrawable<FramerateSpriteBatch>
{
    public const int MAXIMUM_SAMPLES = 100;

    private readonly IContentManagerService _contentManagerService;

    public FramerateService(IContentManagerService contentManagerService)
    {
        SampleBuffer = new Queue<float>();
        _contentManagerService = contentManagerService;
    }

    private SpriteFont SpriteFont { get; set; }
    private Queue<float> SampleBuffer { get; }
    public long TotalFrames { get; private set; }
    public float TotalSeconds { get; private set; }
    public float AverageFramesPerSecond { get; private set; }
    public float CurrentFramesPerSecond { get; private set; }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        CurrentFramesPerSecond = 1.0f / deltaTime;

        SampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (SampleBuffer.Count > MAXIMUM_SAMPLES)
        {
            SampleBuffer.Dequeue();
            AverageFramesPerSecond = SampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = CurrentFramesPerSecond;
        }

        TotalFrames++;
        TotalSeconds += deltaTime;

        var fps = string.Format("FPS: {0}", AverageFramesPerSecond);

        spriteBatch.DrawString(SpriteFont, fps, new Vector2(1, 1), Color.Black);

        // other draw code here
    }

    public void LoadContent()
    {
        SpriteFont = _contentManagerService.LoadSpriteFont("MyFont");
    }
}
