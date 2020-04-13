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
        public Material? Material { get; set; }
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

        private int diffuseCurrentFrameIndex = 0;
        private float diffuseTimeOnFrame;

        private int glowCurrentFrameIndex = 0;
        private float glowTimeOnFrame;

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
        /// <param name="material">The texture of the sprite</param>
        /// <param name="position">The position of the sprite</param>
        /// <param name="size">The size of the sprite.</param>
        public Sprite(Material material, Vector2 position, Vector2? size = null)
        {
            this.Material = material;
            this.Position = position;

            if (size == null)
            {
                this.Size = new Vector2(material.Width, material.Height);
            }
            else
            {
                this.Size = (Vector2)size;
            }

            RotateRandomly();
        }

        public void Update(GameTime gameTime)
        {
            if (Material != null)
            {
                // update diffuse animation
                if (Material.Diffuse is AnimatedTextureData a1)
                {
                    diffuseTimeOnFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (diffuseTimeOnFrame >= a1.Frames[diffuseCurrentFrameIndex].Duration)
                    {
                        diffuseCurrentFrameIndex = ++diffuseCurrentFrameIndex % a1.Frames.Length;

                        // set the start time to the random deviation time of the frame
                        diffuseTimeOnFrame = Globals.Random.Next(-a1.Frames[diffuseCurrentFrameIndex].Deviation, a1.Frames[diffuseCurrentFrameIndex].Deviation);
                        
                    }
                }

                // update glow map animation
                if (Material.GlowMap is AnimatedTextureData a2)
                {
                    glowTimeOnFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (glowTimeOnFrame >= a2.Frames[glowCurrentFrameIndex].Duration)
                    {
                        glowCurrentFrameIndex = ++glowCurrentFrameIndex % a2.Frames.Length;

                        // set the start time to the random deviation time of the frame
                        glowTimeOnFrame = Globals.Random.Next(-a2.Frames[glowCurrentFrameIndex].Deviation, a2.Frames[glowCurrentFrameIndex].Deviation);
                    }
                }
            }
        }

        /// <summary>
        /// Draw the sprite to the screen.
        /// </summary>
        /// <param name="sb">The SpriteBatch which is used to draw the sprite</param>
        public void Draw(SpriteBatch sb)
        {
            if (Material != null)
            {
                int frameWidth = Material.Diffuse.Width / (Material.Diffuse is AnimatedTextureData animatedTexture
                    ? animatedTexture.Frames.Length
                    : 1);
                sb.Draw(Material.Diffuse.Texture,
                    new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                    new Rectangle((Material.Diffuse.SourceRect?.X ?? 0) + diffuseCurrentFrameIndex * frameWidth, Material.Diffuse.SourceRect?.X ?? 0, frameWidth, Material.Diffuse.Height),
                    Color.White * Opacity,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    ZIndex.Map(-128, 127, 0, 1));
            }
        }

        public void DrawGlowMap(SpriteBatch sb)
        {
            if (Material?.GlowMap != null)
            {
                int frameWidth = Material.GlowMap.Width / (Material.GlowMap is AnimatedTextureData animatedTexture
                                     ? animatedTexture.Frames.Length
                                     : 1);
                sb.Draw(Material.GlowMap.Texture,
                    new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                    new Rectangle((Material.GlowMap.SourceRect?.X ?? 0) + glowCurrentFrameIndex * frameWidth, Material.GlowMap.SourceRect?.Y ?? 0, frameWidth, Material.GlowMap.Height),
                    Color.White * Opacity,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    ZIndex.Map(-128, 127, 0, 1));
            }
        }

        /// <summary>
        /// Rotates the sprite randomly in 90° steps if Texture.HasRandomTextureRotation == true.
        /// </summary>
        private void RotateRandomly()
        {
            if (Material != null)
            {
                if (Material.HasRandomTextureRotation)
                {
                    Rotation = (float)(Globals.Random.Next(0, 4) * Math.PI / 2);
                }
            }
        }
    }
}
