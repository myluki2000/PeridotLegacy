using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Resources;
using System;

namespace PeridotEngine.Graphics
{
    class Sprite
    {
        /// <summary>
        /// The texture of this sprite. Gets drawn to the screen when Sprite.Draw() is called. If null a dummy outline is drawn.
        /// </summary>
        public TextureObject Texture { get; set; }
        /// <summary>
        /// The position of the sprite in the current matrix.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The size of the sprite (width x height). Using texture size if null.
        /// </summary>
        public Vector2 Size { get; set; }
        /// <summary>
        /// The rotation of the sprite in radians.
        /// </summary>
        public float Rotation { get; set; } = 0;
        /// <summary>
        /// The opacity of the sprite on a scale from 0.0 - 1.0. Default: 1.0
        /// </summary>
        public float Opacity { get; set; } = 1;


        public Sprite()
        {
            RotateRandomly();
        }

        /// <summary>
        /// Draw the sprite to the screen.
        /// </summary>
        /// <param name="sb">The SpriteBatch which is used to draw the sprite</param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (Texture != null)
            {
                Rectangle destRect;

                if (Size != null)
                {
                    destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
                }
                else
                {
                    destRect = new Rectangle((int)Position.X, (int)Position.Y, Texture.Texture.Width, Texture.Texture.Height);
                }


                sb.Draw(Texture.Texture, destRect, null, Color.White, Rotation, Vector2.Zero, SpriteEffects.None, 0);
            }
            else
            {
                Utility.DrawOutline(sb, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.Red, 2);
            }
        }

        /// <summary>
        /// Rotates the sprite randomly in 90° steps if Texture.HasRandomTextureRotation == true.
        /// </summary>
        private void RotateRandomly()
        {
            if(Texture.HasRandomTextureRotation)
            {
                Rotation = (float)(Globals.Random.Next(0, 4) * Math.PI / 2);
            }
        }
    }
}
