using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    class TexturedTransformableSolid : ISolid, ITextured, IParallaxable
    {
        /// <inheritdoc />
        public Vector2 Position {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public Vector2 Size
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public sbyte ZIndex { get; set; }

        public Quad Quad { get; set; } = new Quad();

        /// <inheritdoc />
        public float ParallaxMultiplier { get; set; }

        /// <inheritdoc />
        public TextureDataBase Texture { get; set; }

        /// <inheritdoc />
        public void Initialize(Level level) { }

        /// <inheritdoc />
        public void Update(GameTime gameTime) { }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera)
        {
            //base.Draw(sb, camera.GetMatrix(new Vector3(ParallaxMultiplier, ParallaxMultiplier, 1)));
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
