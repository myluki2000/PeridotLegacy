using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Editor.Forms.PropertiesForm;

namespace PeridotEngine.Engine.Resources
{
    [Editor(typeof(TextureEditor), typeof(UITypeEditor))]
    public abstract class TextureDataBase
    {
        /// <summary>
        /// Unique name of the TextureData object.
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// The texture data of this TextureData object.
        /// </summary>
        public virtual Texture2D Texture { get; protected set; }
        /// <summary>
        /// The glow map of the texture.
        /// </summary>
        public virtual Texture2D GlowMap { get; protected set; }
        /// <summary>
        /// If set to true the texture will be randomly rotated for each sprite it is used on.
        /// </summary>
        public virtual bool HasRandomTextureRotation { get; protected set; }
        /// <summary>
        /// Gets the width of the texture object. This does not necessarily maatch the width in pixels of the texture.
        /// </summary>
        public abstract int Width { get; set; }

        /// <summary>
        /// Gets the height of the texture object. This does not necessarily maatch the height in pixels of the texture.
        /// </summary>
        public abstract int Height { get; set; }
    }
}
