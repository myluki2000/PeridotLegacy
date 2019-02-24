using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PeridotEngine.Graphics
{
    class Sprite
    {
        /// <summary>
        /// The texture of this sprite. Gets drawn to the screen when Sprite.Draw() is called. If null a dummy outline is drawn.
        /// </summary>
        public Texture2D Texture;
        /// <summary>
        /// The position of the sprite in the current matrix.
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// The size of the sprite (width x height). Using texture size if null.
        /// </summary>
        public Vector2 Size;

        public virtual void Draw(SpriteBatch sb)
        {
            Debug.Assert(sb != null);

            if (Texture != null)
            {
                sb.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);
            }
            else
            {
                sb.Draw(Texture, Position, Color.White);
            }
        }
    }
}
