using Game.Jam.StreetRaceFate.Engine.Exceptions;
using Game.Jam.StreetRaceFate.Engine.Services;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game.Jam.StreetRaceFate.Engine.Factories;
public interface ISpriteBatchFactory
{
    SpriteBatch CreateSpriteBatch<TSpriteBatch>();
    /// <exception cref="SpriteBatchDoesNotExistException"/>
    SpriteBatch GetSpriteBatch<TSpriteBatch>();
}
public class SpriteBatchFactory : ISpriteBatchFactory
{
    private readonly IGraphicsService _graphicsService;

    public SpriteBatchFactory(IGraphicsService graphicsService)
    {
        _graphicsService = graphicsService;

        TypeToSpriteBatches = new Dictionary<Type, SpriteBatch>();
    }

    private Dictionary<Type, SpriteBatch> TypeToSpriteBatches { get; }

    public SpriteBatch CreateSpriteBatch<TSpriteBatch>()
    {
        Type spriteBatchType = typeof(TSpriteBatch);
        if (TypeToSpriteBatches.TryGetValue(spriteBatchType, out SpriteBatch? spriteBatch))
        {
            return spriteBatch;
        }

        spriteBatch = _graphicsService.CreateSpriteBatch();

        TypeToSpriteBatches.Add(spriteBatchType, spriteBatch);

        return spriteBatch;
    }

    public SpriteBatch GetSpriteBatch<TSpriteBatch>()
    {
        Type spriteBatchType = typeof(TSpriteBatch);
        if (!TypeToSpriteBatches.TryGetValue(spriteBatchType, out SpriteBatch? spriteBatch))
        {
            throw new SpriteBatchDoesNotExistException(spriteBatchType);
        }

        return spriteBatch;
    }
}
