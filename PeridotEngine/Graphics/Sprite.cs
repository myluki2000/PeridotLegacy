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
        public Texture2D Texture { get; set; }
        /// <summary>
        /// The position of the sprite in the current matrix.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The size of the sprite (width x height). Using texture size if null.
        /// </summary>
        public Vector2 Size { get; set; }
        /// <summary>
        /// The opacity of the sprite on a scale from 0.0 - 1.0. Default: 1.0
        /// </summary>
        public float Opacity { get; set; } = 1;

        /// <summary>
        /// Draw the sprite to the screen.
        /// </summary>
        /// <param name="sb">The SpriteBatch which is used to draw the sprite</param>
        public virtual void Draw(SpriteBatch sb)
        {
            Debug.Assert(sb != null);

            if (Texture != null)
            {
                if (Size != null)
                {
                    sb.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White * Opacity);
                }
                else
                {
                    sb.Draw(Texture, Position, Color.White);
                }
            }
            else
            {
                Utility.DrawOutline(sb, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.Red, 2);
            }
        }
    }
}
