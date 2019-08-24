#nullable enable

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Resources;
using Color = Microsoft.Xna.Framework.Color;

namespace PeridotEngine.Graphics
{
    class TransformableSprite
    {
        /// <summary>
        /// The texture of this sprite. Gets drawn to the screen when Sprite.Draw() is called. If null a dummy outline is drawn.
        /// </summary>
        public TextureData? Texture { get; set; }
        /// <summary>
        /// The position of the top-left corner of the sprite.
        /// </summary>
        public Vector2 TopLeft { get; set; }
        /// <summary>
        /// The position of the top-right corner of the sprite.
        /// </summary>
        public Vector2 TopRight { get; set; }
        /// <summary>
        /// The position of the bottom-left corner of the sprite.
        /// </summary>
        public Vector2 BottomLeft { get; set; }
        /// <summary>
        /// The position of the bottom-right corner of the sprite.
        /// </summary>
        public Vector2 BottomRight { get; set; }
        /// <summary>
        /// Z-index of the sprite. A larger z-index draws this sprite in front of others.
        /// </summary>
        public int ZIndex { get; set; }
        /// <summary>
        /// The opacity of the sprite on a scale from 0.0 - 1.0. Default: 1.0
        /// </summary>
        public float Opacity { get; set; } = 1;

        private readonly BasicEffect basicEffect = new BasicEffect(Globals.Graphics.GraphicsDevice)
        {
            //TextureEnabled = true,
            VertexColorEnabled = true
        };

        /// <summary>
        /// Creates a new empty transformable sprite object.
        /// </summary>
        public TransformableSprite() { }

        public TransformableSprite(TextureData texture, Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft)
        {
            this.Texture = texture;
            this.TopLeft = topLeft;
            this.TopRight = topRight;
            this.BottomRight = bottomRight;
            this.BottomLeft = bottomLeft;
        }

        public void Draw(SpriteBatch sb, Matrix viewMatrix)
        {
            sb.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            basicEffect.World = Matrix.Identity;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0.0f,
                                                                        Globals.Graphics.PreferredBackBufferWidth,
                                                                        Globals.Graphics.PreferredBackBufferHeight,
                                                                        0.0f,
                                                                        0.0f,
                                                                        1.0f);


            basicEffect.View = viewMatrix;

            // TODO: convert this to array for extra performance. We know how many verts we have
            List<VertexPositionColor> verts = new List<VertexPositionColor>
            {
                new VertexPositionColor() {Position = new Vector3(TopLeft, 0), Color = Color.Red},
                new VertexPositionColor() {Position = new Vector3(TopRight, 0), Color = Color.Yellow},
                new VertexPositionColor() {Position = new Vector3(BottomRight, 0), Color = Color.Green},
                new VertexPositionColor() {Position = new Vector3(TopLeft, 0), Color = Color.Magenta},
                new VertexPositionColor() {Position = new Vector3(BottomRight, 0), Color = Color.DarkGreen},
                new VertexPositionColor() {Position = new Vector3(BottomLeft, 0), Color = Color.Blue}
            };

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                sb.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts.ToArray(), 0, verts.Count, Utility.GetIndicesArray(verts), 0, verts.Count / 3);
            }
        }
    }
}
