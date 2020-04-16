using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Graphics.Effects;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    class ProcedualFog : ISolid, IParallaxable
    {
        /// <inheritdoc />
        public string Id { get; set; }
        /// <inheritdoc />
        public string Class { get; set; }
        /// <inheritdoc />
        public Vector2 Position { get; set; }
        /// <inheritdoc />
        public Vector2 Size { get; set; }
        /// <inheritdoc />
        public sbyte ZIndex { get; set; }
        /// <inheritdoc />
        public float ParallaxMultiplier { get; set; } = 1.0f;

        private static readonly FogEffect fogEffect = new FogEffect();

        /// <inheritdoc />
        public void Initialize(Level level) { }

        /// <inheritdoc />
        public void Update(GameTime gameTime)
        {
            fogEffect.Time = (float)gameTime.TotalGameTime.TotalSeconds;
        }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera)
        {
            sb.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            fogEffect.WorldViewProjection =
                camera.GetMatrix()
                * Matrix.CreateOrthographicOffCenter(0.0f,
                    Globals.Graphics.PreferredBackBufferWidth,
                    Globals.Graphics.PreferredBackBufferHeight,
                    0.0f,
                    0.0f,
                    1.0f);
                

            VertexPositionTexture[] verts =
            {
                new VertexPositionTexture(new Vector3(Position.X + Size.X, Position.Y, 0), new Vector2(1, 0)), // top right
                new VertexPositionTexture(new Vector3(Position.X + Size.X, Position.Y + Size.Y, 0), new Vector2(1, 1)), // bottom right
                new VertexPositionTexture(new Vector3(Position.X, Position.Y + Size.Y, 0), new Vector2(0, 1)), // bottom left

                new VertexPositionTexture(new Vector3(Position.X, Position.Y, 0), new Vector2(0, 0)), // top left
                new VertexPositionTexture(new Vector3(Position.X + Size.X, Position.Y, 0), new Vector2(1, 0)), // top right
                new VertexPositionTexture(new Vector3(Position.X, Position.Y + Size.Y, 0), new Vector2(0, 1)), // bottom left
            };

            foreach (EffectPass pass in fogEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                sb.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);
            }
        }

        /// <inheritdoc />
        public void DrawGlowMap(SpriteBatch sb, Camera camera) { }

        /// <inheritdoc />
        public void DrawOutline(SpriteBatch sb, Color color, Camera camera)
        {
            Utility.Utility.DrawOutline(sb, new Rectangle(Position.ToPoint(), Size.ToPoint()).Transform(camera.GetMatrix(ParallaxMultiplier)), color);
        }

        /// <inheritdoc />
        public bool ContainsPointOnScreen(Point point, Camera camera)
        {
            return new Rectangle(Position.ToPoint(), Size.ToPoint()).Contains(point.Transform(camera.GetMatrix(ParallaxMultiplier).Invert()));
        }

        /// <inheritdoc />
        public XElement ToXml(LazyLoadingMaterialDictionary materialDictionary)
        {
            XElement result = new XElement(this.GetType().Name,
                Position.ToXml("Position"),
                Size.ToXml("Size"),
                new XElement("Z-Index", ZIndex)
            );

            if (!string.IsNullOrEmpty(Id)) result.Add(new XAttribute("Id", Id));

            if (!string.IsNullOrEmpty(Class)) result.Add(new XAttribute("Class", Class));

            return result;
        }

        public static ProcedualFog FromXml(XElement xEle, LazyLoadingMaterialDictionary materials)
        {
            return new ProcedualFog()
            {
                Id = xEle.Attribute("Id")?.Value,
                Class = xEle.Attribute("Class")?.Value,
                Position = new Vector2().FromXml(xEle.Element("Position")),
                Size = new Vector2().FromXml(xEle.Element("Size")),
                ZIndex = sbyte.Parse(xEle.Element("Z-Index").Value)
            };
        }
    }
}
