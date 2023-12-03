using Microsoft.Xna.Framework;

namespace Game.Jam.StreetRaceFate.Engine;
public interface IGameScene
{
    void Initalize();
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);
}
