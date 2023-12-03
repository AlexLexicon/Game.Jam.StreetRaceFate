using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IDelayService
{
    void Delay(GameTime gameTime, float secondsDelay, Action action);
}
public class DelayService : IDelayService
{
    public DelayService()
    {
        ActionToRemainingSeconds = new Dictionary<Action, float>();
    }

    private Dictionary<Action, float> ActionToRemainingSeconds { get; set; }

    public void Delay(GameTime gameTime, float secondsDelay, Action action)
    {
        float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!ActionToRemainingSeconds.TryGetValue(action, out float remainingSeconds))
        {
            remainingSeconds = secondsDelay;
        }

        remainingSeconds -= elapsedSeconds;

        if (remainingSeconds is <= 0)
        {
            action.Invoke();

            remainingSeconds = secondsDelay;
        }

        if (ActionToRemainingSeconds.ContainsKey(action))
        {
            ActionToRemainingSeconds[action] = remainingSeconds;
        }
        else
        {
            ActionToRemainingSeconds.Add(action, remainingSeconds);
        }
    }
}
