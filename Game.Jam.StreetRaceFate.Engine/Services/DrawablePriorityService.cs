using System;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IDrawablePriorityService
{
    int GetDrawablePriority(IGameDrawable gameDrawable);
}
public class DrawablePriorityService : IDrawablePriorityService
{
    private readonly IServiceProvider _serviceProvider;

    public DrawablePriorityService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public int GetDrawablePriority(IGameDrawable gameDrawable)
    {
        Type gameDrawableType = gameDrawable.GetType();

        var DrawablePriorityProviderType = typeof(IDrawablePriorityProvider<>).MakeGenericType(gameDrawableType);

        var drawablePriorityProvider = (IDrawablePriorityProvider?)_serviceProvider.GetService(DrawablePriorityProviderType);

        if (drawablePriorityProvider is null)
        {
            return 0;
        }

        return drawablePriorityProvider.Priority;
    }
}
