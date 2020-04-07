#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.Graphics
{
    class Sprite
    {
        /// <summary>
        /// The texture of this sprite. Gets drawn to the screen when Sprite.Draw() is called. If null a dummy outline is drawn.
        /// </summary>
        public TextureDataBase? Texture { get; set; }
        /// <summary>
        /// The position of the sprite in the current matrix.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The size of the sprite (width x height).
        /// </summary>
        public Vector2 Size { get; set; }
        /// <summary>
        /// Z-index of the sprite. A larger z-index draws this sprite in front of others.
        /// </summary>
        public sbyte ZIndex { get; set; }
        /// <summary>
        /// The rotation of the sprite in radians.
        /// </summary>
        public float Rotation { get; set; } = 0;
        /// <summary>
        /// The opacity of the sprite on a scale from 0.0 - 1.0. Default: 1.0
        /// </summary>
        public float Opacity { get; set; } = 1.0f;

        private int currentFrameIndex = 0;
        private float timeOnFrame;

        /// <summary>
        /// Create a new empty sprite object.
        /// </summary>
        public Sprite()
        {
            RotateRandomly();
        }

        /// <summary>
        /// Create a new sprite with the specified parameters.
        /// </summary>
        /// <param name="texture">The texture of the sprite</param>
        /// <param name="position">The position of the sprite</param>
        /// <param name="size">The size of the sprite.</param>
        public Sprite(TextureDataBase texture, Vector2 position, Vector2? size = null)
        {
            this.Texture = texture;
            this.Position = position;

            if (size == null)
            {
                this.Size = new Vector2(texture.Width, texture.Height);
            }
            else
            {
                this.Size = (Vector2)size;
            }

            RotateRandomly();
        }

        public void Update(GameTime gameTime)
        {
            if (Texture is AnimatedTextureData animatedTexture)
            {
                timeOnFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeOnFrame >= animatedTexture.FrameDurations[currentFrameIndex])
                {
                    timeOnFrame = 0;
                    currentFrameIndex = ++currentFrameIndex % animatedTexture.FrameCount;
                }
            }
        }

        /// <summary>
        /// Draw the sprite to the screen.
        /// </summary>
        /// <param name="sb">The SpriteBatch which is used to draw the sprite</param>
        public void Draw(SpriteBatch sb)
        {
            if (Texture != null)
            {
                int frameWidth = Texture.Texture.Width / (Texture is AnimatedTextureData animatedTexture
                    ? animatedTexture.FrameCount
                    : 1);
                sb.Draw(Texture.Texture,
                    new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                    new Rectangle(currentFrameIndex * frameWidth, 0, frameWidth, Texture.Texture.Height),
                    Color.White * Opacity,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    ZIndex.Map(-128, 127, 0, 1));
            }
            else
            {
                Utility.Utility.DrawOutline(sb, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.Red, 2);
            }
        }

        /// <summary>
        /// Rotates the sprite randomly in 90° steps if Texture.HasRandomTextureRotation == true.
        /// </summary>
        private void RotateRandomly()
        {
            if (Texture != null)
            {
                if (Texture.HasRandomTextureRotation)
                {
                    Rotation = (float)(Globals.Random.Next(0, 4) * Math.PI / 2);
                }
            }
        }
    }
}
