using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;

namespace PeridotEngine.World.WorldObjects.Solids
{
    class TexturedTransformableSolid : TransformableSprite, ISolid
    {
        /// <inheritdoc />
        public Vector2 Position { get; set; }

        /// <inheritdoc />
        public Vector2 Size { get; set; }

        /// <inheritdoc />
        public void Initialize(Level level)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public XElement ToXml(LazyLoadingTextureDictionary textureDictionary)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public float ParallaxMultiplier { get; set; }
    }
}
