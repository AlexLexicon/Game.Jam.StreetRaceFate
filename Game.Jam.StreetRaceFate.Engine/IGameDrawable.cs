using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine;
public interface IGameDrawable : IGameObject
{
    void Draw(GameTime gameTime);
}
