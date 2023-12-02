using Game.Jam.StreetRaceFate.Engine.Builders;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Game.Jam.StreetRaceFate.Engine.Extensions;
public static class GameObjectOptionsBuilderExtensions
{
    public static void SetDrawPriority<TGameObject>(this GameObjectOptionsBuilder<TGameObject> builder, int priority) where TGameObject : IGameDrawable
    {
        builder.Services.TryAddSingleton<IDrawablePriorityProvider<TGameObject>>(sp =>
        {
            return new DrawablePriorityProvider<TGameObject>
            {
                Priority = priority,
            };
        });
    }
}
