using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IMovementService
{
    Vector2 MoveTo(GameTime gameTime, Vector2 currentPosition, Vector2 targetPosition, float maximumDistance, float speed);
    Vector2 MoveTo(GameTime gameTime, Vector2 currentPosition, Vector2 targetPosition, float maximumDistance, float speed, out bool reachedTarget);
}
internal class MovementService : IMovementService
{
    public Vector2 MoveTo(GameTime gameTime, Vector2 currentPosition, Vector2 targetPosition, float maximumDistance, float speed) => MoveTo(gameTime, currentPosition, targetPosition, maximumDistance, speed, out bool _);
    public Vector2 MoveTo(GameTime gameTime, Vector2 currentPosition, Vector2 targetPosition, float maximumDistance, float speed, out bool reachedTarget)
    {
        float distance = Vector2.Distance(currentPosition, targetPosition);

        reachedTarget = false;

        if (distance <= maximumDistance)
        {
            reachedTarget = true;

            return currentPosition;
        }

        float normalizedSpeed = speed;// * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        return Vector2.Lerp(currentPosition, targetPosition, normalizedSpeed);
    }
}
