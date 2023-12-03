using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Engine;
public interface ISpriteBatchDrawable
{
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}
public interface ISpriteBatchDrawable<TSpriteBatch> : ISpriteBatchDrawable
{
}
