using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Graphics.Effects;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    class TexturedTransformableSolid : ISolid, ITextured, IRenderedObject
    {
        /// <inheritdoc />
        public string? Id { get; set; }
        /// <inheritdoc />
        public string? Class { get; set; }
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

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Quad Quad { get; set; } = new Quad(new Vector2(0, 100), new Vector2(100, 100), new Vector2(100, 0), new Vector2(0, 0));

        public float ParallaxMultiplierTopLeft { get; set; } = 1.0f;
        public float ParallaxMultiplierTopRight { get; set; } = 1.0f;
        public float ParallaxMultiplierBottomLeft { get; set; } = 1.0f;
        public float ParallaxMultiplierBottomRight { get; set; } = 1.0f;

        /// <inheritdoc />
        public Material Material
        {
            get => material;
            set
            {
                material = value;
                currentFrameIndices = new int[Material.GetHighestTextureTypeValue() + 1];
                timeOnFrames = new float[Material.GetHighestTextureTypeValue() + 1];
            }
        }

        /// <inheritdoc />
        public bool DisableBatching => true;

        private static readonly QuadEffect quadEffect;
        private Vector2 size = new Vector2(100, 100);

        private int[] currentFrameIndices;
        private float[] timeOnFrames;
        private Material material;

        static TexturedTransformableSolid()
        {
            quadEffect = new QuadEffect();
        }

        /// <inheritdoc />
        public void Initialize(Level level) { }

        /// <inheritdoc />
        public void Update(GameTime gameTime)
        {
            if (Material != null)
            {
                // update times for animated textures
                for (int texType = 0; texType < Material.Textures.Length; texType++)
                {
                    if (Material.Textures[texType] is AnimatedTextureData aTex)
                    {
                        timeOnFrames[texType] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
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

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera, Material.TextureType texType = Material.TextureType.Diffuse)
        {
            if (Material?.Textures[(int) texType] == null) return;

            int frameWidth = Material.Textures[(int)texType].Width / (Material.Textures[(int)texType] is AnimatedTextureData atd
                ? atd.Frames.Length
                : 1);

            Draw(sb, camera, Material.Textures[(int)texType].Texture,
                new Rectangle((Material.Textures[(int)texType].SourceRect?.X ?? 0) + frameWidth * currentFrameIndices[(int)texType],
                    Material.Textures[(int)texType].SourceRect?.Y ?? 0,
                    frameWidth,
                    Material.Textures[(int)texType].Height));
        }

        private void Draw(SpriteBatch sb, Camera camera, Texture2D tex, Rectangle srcRect)
        {
            // TODO: Make z-index work

            sb.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            quadEffect.WorldViewProj = Matrix.CreateOrthographicOffCenter(
                0.0f,
                Globals.Graphics.PreferredBackBufferWidth,
                Globals.Graphics.PreferredBackBufferHeight,
                0.0f,
                0.0f,
                1.0f
            );

            quadEffect.Texture = tex;

            Vector2 p1 = Quad.Point1.Transform(camera.GetMatrix(ParallaxMultiplierBottomLeft));
            Vector2 p2 = Quad.Point2.Transform(camera.GetMatrix(ParallaxMultiplierBottomRight));
            Vector2 p3 = Quad.Point3.Transform(camera.GetMatrix(ParallaxMultiplierTopRight));
            Vector2 p4 = Quad.Point4.Transform(camera.GetMatrix(ParallaxMultiplierTopLeft));

            float[] qs = CalcQn(p1, p2, p3, p4);

            VertexPositionTexture3D[] verts = new VertexPositionTexture3D[6]
            {
                new VertexPositionTexture3D() {Position = new Vector3(p4, 0), TexCoord = new Vector3((float)srcRect.Left / tex.Width * qs[3], (float)srcRect.Top / tex.Height * qs[3], qs[3])},
                new VertexPositionTexture3D() {Position = new Vector3(p3, 0), TexCoord = new Vector3((float)srcRect.Right / tex.Width * qs[2], (float)srcRect.Top / tex.Height * qs[2], qs[2])},
                new VertexPositionTexture3D() {Position = new Vector3(p2, 0), TexCoord = new Vector3((float)srcRect.Right / tex.Width * qs[1], (float)srcRect.Bottom / tex.Height * qs[1], qs[1])},
                new VertexPositionTexture3D() {Position = new Vector3(p4, 0), TexCoord = new Vector3((float)srcRect.Left / tex.Width * qs[3], (float)srcRect.Top / tex.Height * qs[3], qs[3])},
                new VertexPositionTexture3D() {Position = new Vector3(p2, 0), TexCoord = new Vector3((float)srcRect.Right / tex.Width * qs[1], (float)srcRect.Bottom / tex.Height * qs[1], qs[1])},
                new VertexPositionTexture3D() {Position = new Vector3(p1, 0), TexCoord = new Vector3((float)srcRect.Left / tex.Width * qs[0], (float)srcRect.Bottom / tex.Height * qs[0], qs[0])}
            };

            foreach (EffectPass pass in quadEffect.Techniques[0].Passes)
            {
                pass.Apply();
                sb.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts, 0, verts.Length, Utility.Utility.GetIndicesArray(verts), 0, verts.Length / 3);
            }
        }

        /// <inheritdoc />
        public void DrawOutline(SpriteBatch sb, Color color, Camera camera)
        {
            Vector2 p1 = Quad.Point1.Transform(camera.GetMatrix(ParallaxMultiplierBottomLeft));
            Vector2 p2 = Quad.Point2.Transform(camera.GetMatrix(ParallaxMultiplierBottomRight));
            Vector2 p3 = Quad.Point3.Transform(camera.GetMatrix(ParallaxMultiplierTopRight));
            Vector2 p4 = Quad.Point4.Transform(camera.GetMatrix(ParallaxMultiplierTopLeft));
            new Quad(p1, p2, p3, p4).Draw(sb, color);
        }

        /// <inheritdoc />
        public bool ContainsPointOnScreen(Point point, Camera camera)
        {
            Vector2 p1 = Quad.Point1.Transform(camera.GetMatrix(ParallaxMultiplierBottomLeft));
            Vector2 p2 = Quad.Point2.Transform(camera.GetMatrix(ParallaxMultiplierBottomRight));
            Vector2 p3 = Quad.Point3.Transform(camera.GetMatrix(ParallaxMultiplierTopRight));
            Vector2 p4 = Quad.Point4.Transform(camera.GetMatrix(ParallaxMultiplierTopLeft));
            return new Quad(p1, p2, p3, p4).Contains(point);
        }

        /// <inheritdoc />
        public XElement ToXml(LazyLoadingMaterialDictionary materialDictionary)
        {
            if (Material != null)
                materialDictionary.LoadMaterial(Material.Path);
            XElement texPathXEle = Material != null ? new XElement("TexturePath", materialDictionary.GetTexturePathByName(Material.Name)) : null;

            XElement result = new XElement(this.GetType().Name,
                Quad.ToXml("Quad"),
                texPathXEle,
                new XElement("Z-Index", ZIndex.ToString(CultureInfo.InvariantCulture)),
                new XElement("ParallaxTopLeft", ParallaxMultiplierTopLeft.ToString(CultureInfo.InvariantCulture)),
                new XElement("ParallaxTopRight", ParallaxMultiplierTopRight.ToString(CultureInfo.InvariantCulture)),
                new XElement("ParallaxBottomLeft", ParallaxMultiplierBottomLeft.ToString(CultureInfo.InvariantCulture)),
                new XElement("ParallaxBottomRight", ParallaxMultiplierBottomRight.ToString(CultureInfo.InvariantCulture))
            );

            if (!string.IsNullOrEmpty(Id)) result.Add(new XAttribute("Id", Id));

            if (!string.IsNullOrEmpty(Class)) result.Add(new XAttribute("Class", Class));

            return result;
        }

        public static TexturedTransformableSolid FromXml(XElement xEle, LazyLoadingMaterialDictionary materials)
        {
            return new TexturedTransformableSolid()
            {
                Material = materials[xEle.Element("TexturePath").Value],
                Quad = Quad.FromXml(xEle.Element("Quad")),
                ZIndex = sbyte.Parse(xEle.Element("Z-Index").Value),
                ParallaxMultiplierTopLeft = float.Parse(xEle.Element("ParallaxTopLeft").Value, CultureInfo.InvariantCulture),
                ParallaxMultiplierTopRight = float.Parse(xEle.Element("ParallaxTopRight").Value, CultureInfo.InvariantCulture),
                ParallaxMultiplierBottomLeft = float.Parse(xEle.Element("ParallaxBottomLeft").Value, CultureInfo.InvariantCulture),
                ParallaxMultiplierBottomRight = float.Parse(xEle.Element("ParallaxBottomRight").Value, CultureInfo.InvariantCulture)
            };
        }

        private float[] CalcQn(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float[] result = { 1.0f, 1.0f, 1.0f, 1.0f };

            float ax = p2.X - p0.X;
            float ay = p2.Y - p0.Y;
            float bx = p3.X - p1.X;
            float by = p3.Y - p1.Y;

            float cross = ax * by - ay * bx;

            if (cross != 0)
            {
                float cy = p0.Y - p1.Y;
                float cx = p0.X - p1.X;

                float s = (ax * cy - ay * cx) / cross;

                if (s > 0 && s < 1)
                {
                    float t = (bx * cy - by * cx) / cross;

                    if (t > 0 && t < 1)
                    {

                        result[0] = 1 / (1 - t);
                        result[1] = 1 / (1 - s);
                        result[2] = 1 / t;
                        result[3] = 1 / s;
                    }
                }
            }

            return result;
        }

        private struct VertexPositionTexture3D : IVertexType
        {
            public Vector3 Position;
            public Vector3 TexCoord;

            /// <inheritdoc />
            private static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
            );

            VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
        }
    }
}
