using System;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface ISpriteBatchDrawablePriorityService
{
    int GetDrawablePriority(ISpriteBatchDrawable gameDraspriteBatchDrawablewable);
}
public class SpriteBatchDrawablePriorityService : ISpriteBatchDrawablePriorityService
{
    private readonly IServiceProvider _serviceProvider;

    public SpriteBatchDrawablePriorityService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public int GetDrawablePriority(ISpriteBatchDrawable spriteBatchDrawable)
    {
        Type spriteBatchDrawableType = spriteBatchDrawable.GetType();

        var spriteBatchDrawablePriorityProviderType = typeof(ISpriteBatchDrawablePriorityProvider<>).MakeGenericType(spriteBatchDrawableType);

        var spriteBatchDrawablePriorityProvider = (ISpriteBatchDrawablePriorityProvider?)_serviceProvider.GetService(spriteBatchDrawablePriorityProviderType);

        if (spriteBatchDrawablePriorityProvider is null)
        {
            return 0;
        }

        return spriteBatchDrawablePriorityProvider.Priority;
    }
}
