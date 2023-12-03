using System;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IOscillateService
{
    void Oscillate(int min, int max, Action<int> action);
}
public class OscillateService : IOscillateService
{
    public OscillateService()
    {
        ActionToCount = new Dictionary<Action<int>, int>();
    }

    private Dictionary<Action<int>, int> ActionToCount { get; set; }

    public void Oscillate(int min, int max, Action<int> action)
    {
        if (!ActionToCount.TryGetValue(action, out int count))
        {
            count = 0;
        }

        count++;

        int range = max - min;
        int x = count + range;
        int y = range * 2;
        int z = x % y;
        int abs = Math.Abs(z - range);
        int result = min + abs;

        if (ActionToCount.ContainsKey(action))
        {
            ActionToCount[action] = count;
        }
        else
        {
            ActionToCount.Add(action, count);
        }

        action.Invoke(result);
    }
}
