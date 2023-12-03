using Game.Jam.StreetRaceFate.Engine.Extensions;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Game.Jam.StreetRaceFate.Engine.Builders;
public class SpriteBatchDrawableGameObjectOptionsBuilder<TGameObject> : GameObjectOptionsBuilder<TGameObject> where TGameObject : IGameObject, ISpriteBatchDrawable
{
    public SpriteBatchDrawableGameObjectOptionsBuilder(IServiceCollection services) : base(services)
    {
    }

    public void SetSbDrawPrioirty(int priority)
    {
        Services.TryAddSingleton<ISpriteBatchDrawablePriorityProvider<TGameObject>>(sp =>
        {
            return new SpriteBatchDrawablePriorityProvider<TGameObject>
            {
                Priority = priority,
            };
        });
    }
    public void AddToSpriteBatch<TSpriteBatch>(Action<DrawableGameObjectOptionsBuilder<TSpriteBatch>>? configure = null) where TSpriteBatch : ISpriteBatch
    {
        var builder = new DrawableGameObjectOptionsBuilder<TSpriteBatch>(Services);

        configure?.Invoke(builder);

        Services.TryAdd(new ServiceDescriptor(typeof(TSpriteBatch), (sp) =>
        {
            return sp.ActivateGameInstance<TSpriteBatch>();
        }, ServiceLifetime.Singleton));
        Services.AddSingleton<ISpriteBatch>(sp =>
        {
            return sp.GetRequiredService<TSpriteBatch>();
        });
    }
}
