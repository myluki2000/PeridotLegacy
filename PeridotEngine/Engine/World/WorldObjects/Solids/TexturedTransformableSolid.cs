using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;

namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    class TexturedTransformableSolid : TransformableSprite, ISolid, ITextured, IParallaxable
    {
        private Vector2 size;

        /// <inheritdoc />
        public Vector2 Position { get; set; }

        /// <inheritdoc />
        public Vector2 Size
        {
            get => size;
            set
            {
                size = value;

            }
        }

        /// <inheritdoc />
        public float ParallaxMultiplier { get; set; }

        /// <inheritdoc />
        public void Initialize(Level level) { }

        /// <inheritdoc />
        public void Update(GameTime gameTime) { }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera)
        {
            base.Draw(sb, camera.GetMatrix(new Vector3(ParallaxMultiplier, ParallaxMultiplier, 1)));
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
