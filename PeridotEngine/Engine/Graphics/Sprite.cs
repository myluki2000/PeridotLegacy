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
        public Material? Material
        {
            get => material;
            set
            {
                material = value;
                currentFrameIndices = new int[Material.GetHighestTextureTypeValue() + 1];
                timeOnFrames = new float[Material.GetHighestTextureTypeValue() + 1];
            }
        }

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

        private int[] currentFrameIndices;
        private float[] timeOnFrames;
        private Material material;

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
                // update times for animated textures
                for (int texType = 0; texType < Material.Textures.Length; texType++)
                {
                    if (Material.Textures[texType] is AnimatedTextureData aTex)
                    {
                        timeOnFrames[texType] += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (timeOnFrames[texType] > aTex.Frames[currentFrameIndices[texType]].Duration)
                        {
                            currentFrameIndices[texType] = ++currentFrameIndices[texType] % aTex.Frames.Length;

                            // set the start time to the random deviation time of the frame
                            int deviation = aTex.Frames[currentFrameIndices[texType]].Deviation;
                            timeOnFrames[texType] = Globals.Random.Next(-deviation, deviation);
                        }
                    }    
                }
            }
        }

        /// <summary>
        /// Draw the sprite to the screen.
        /// </summary>
        /// <param name="sb">The SpriteBatch which is used to draw the sprite</param>
        public void Draw(SpriteBatch sb, Material.TextureType texType = Material.TextureType.Diffuse)
        {
            if (Material?.Textures[(int) texType] == null) return;

            int frameWidth = Material.Textures[(int)texType].Width / (Material.Textures[(int)texType] is AnimatedTextureData animatedTexture
                                 ? animatedTexture.Frames.Length
                                 : 1);
            sb.Draw(Material.Textures[(int)texType].Texture,
                new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                new Rectangle((Material.Textures[(int)texType].SourceRect?.X ?? 0) + currentFrameIndices[(int)texType] * frameWidth,
                    Material.Textures[(int)texType].SourceRect?.X ?? 0,
                    frameWidth,
                    Material.Textures[(int)texType].Height),
                Color.White * Opacity,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                ZIndex.Map(-128, 127, 0, 1));
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
