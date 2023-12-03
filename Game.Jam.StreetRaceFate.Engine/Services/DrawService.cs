using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Jam.StreetRaceFate.Engine.Services;
public interface IDrawService
{
    void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float? rotation = null, float scale = 1f);
    void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color);
}
public class DrawService : IDrawService
{
    public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float? rotation = null, float scale = 1f)
    {
        float halfWidth = texture.Width / 2;
        float halfHeight = texture.Height / 2;

        spriteBatch.Draw(texture, new Vector2(position.X + halfWidth, position.Y + halfHeight), null, Color.White, rotation ?? 0, new Vector2(halfWidth, halfHeight), scale, SpriteEffects.None, 1f);
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color)
    {
        Vector2 size = spriteFont.MeasureString(text);

        spriteBatch.DrawString(spriteFont, text, new Vector2(position.X - size.X / 2, position.Y - size.Y / 2), color);
    }
}
