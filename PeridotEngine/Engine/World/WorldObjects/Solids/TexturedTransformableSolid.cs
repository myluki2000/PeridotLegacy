using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    class TexturedTransformableSolid : ISolid, ITextured
    {
        /// <inheritdoc />
        public Vector2 Position
        {
            get => Quad.Point4;
            set
            {
                Vector2 difference = value - Position;
                Quad.Point4 = value;
                Quad.Point1 += difference;
                Quad.Point2 += difference;
                Quad.Point3 += difference;
            }
        }

        /// <inheritdoc />
        public Vector2 Size
        {
            get => size;
            set
            {
                Quad.Point2 = Quad.Point4 + (Quad.Point2 - Quad.Point4) / size * value;
                Quad.Point3 = Quad.Point4 + (Quad.Point3 - Quad.Point4) / size * value;
                Quad.Point1 = Quad.Point4 + (Quad.Point1 - Quad.Point4) / size * value;

                size = value;
            }
        }

        /// <inheritdoc />
        public sbyte ZIndex { get; set; }

        public Quad Quad { get; set; } = new Quad(new Vector2(0, 100), new Vector2(100, 100), new Vector2(100, 0), new Vector2(0, 0));

        public float ParallaxMultiplierTopLeft { get; set; } = 1.0f;
        public float ParallaxMultiplierTopRight { get; set; } = 1.0f;
        public float ParallaxMultiplierBottomLeft { get; set; } = 1.0f;
        public float ParallaxMultiplierBottomRight { get; set; } = 1.0f;

        /// <inheritdoc />
        public TextureDataBase Texture { get; set; }

        private readonly BasicEffect basicEffect = new BasicEffect(Globals.Graphics.GraphicsDevice);
        private Vector2 size = new Vector2(100, 100);

        /// <inheritdoc />
        public void Initialize(Level level) { }

        /// <inheritdoc />
        public void Update(GameTime gameTime) { }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera)
        {
            sb.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            basicEffect.World = Matrix.Identity;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(
                0.0f,
                Globals.Graphics.PreferredBackBufferWidth,
                Globals.Graphics.PreferredBackBufferHeight,
                0.0f,
                0.0f, 
                1.0f
            );
            basicEffect.View = Matrix.Identity;

            basicEffect.TextureEnabled = true;
            basicEffect.Texture = Texture.Texture;

            VertexPositionTexture[] verts = new VertexPositionTexture[6]
            {
                new VertexPositionTexture() {Position = new Vector3(Quad.Point4, 0).Transform(camera.GetMatrix(ParallaxMultiplierTopLeft)), TextureCoordinate = new Vector2(0, 0)},
                new VertexPositionTexture() {Position = new Vector3(Quad.Point3, 0).Transform(camera.GetMatrix(ParallaxMultiplierTopRight)), TextureCoordinate = new Vector2(1, 0)},
                new VertexPositionTexture() {Position = new Vector3(Quad.Point2, 0).Transform(camera.GetMatrix(ParallaxMultiplierBottomRight)), TextureCoordinate = new Vector2(1, 1)},
                new VertexPositionTexture() {Position = new Vector3(Quad.Point4, 0).Transform(camera.GetMatrix(ParallaxMultiplierTopLeft)), TextureCoordinate = new Vector2(0, 0)},
                new VertexPositionTexture() {Position = new Vector3(Quad.Point2, 0).Transform(camera.GetMatrix(ParallaxMultiplierBottomRight)), TextureCoordinate = new Vector2(1, 1)},
                new VertexPositionTexture() {Position = new Vector3(Quad.Point1, 0).Transform(camera.GetMatrix(ParallaxMultiplierBottomLeft)), TextureCoordinate = new Vector2(0, 1)}
            };

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                sb.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts, 0, verts.Length, Utility.Utility.GetIndicesArray(verts), 0, verts.Length / 3);
            }
        }

        /// <inheritdoc />
        public bool Contains(Point point)
        {
            return Quad.Contains(point);
        }

        /// <inheritdoc />
        public XElement ToXml(LazyLoadingTextureDictionary textureDictionary)
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }

        public static TexturedTransformableSolid FromXml(XElement xEle, LazyLoadingTextureDictionary textures)
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }



    }
}
