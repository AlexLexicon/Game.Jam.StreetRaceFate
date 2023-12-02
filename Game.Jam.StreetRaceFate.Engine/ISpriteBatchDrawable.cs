using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Engine;
public interface ISpriteBatchDrawable<TSpriteBatch>
{
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}
