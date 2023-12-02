using System;

namespace Game.Jam.StreetRaceFate.Engine.Exceptions;
public class SpriteBatchDoesNotExistException : Exception
{
    public SpriteBatchDoesNotExistException(Type? spriteBatchType) : base($"The SpriteBatch for the type '{spriteBatchType?.FullName ?? "null"}' does not exist.")
    {
    }
}
