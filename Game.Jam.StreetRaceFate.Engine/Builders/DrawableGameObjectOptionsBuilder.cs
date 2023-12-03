using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Game.Jam.StreetRaceFate.Engine.Builders;
public class DrawableGameObjectOptionsBuilder<TGameObject> : GameObjectOptionsBuilder<TGameObject> where TGameObject : IGameDrawable
{
    public DrawableGameObjectOptionsBuilder(IServiceCollection services) : base(services)
    {
    }

    public void SetDrawPriority(int priority)
    {
        Services.TryAddSingleton<IDrawablePriorityProvider<TGameObject>>(sp =>
        {
            return new DrawablePriorityProvider<TGameObject>
            {
                Priority = priority,
            };
        });
    }
}
