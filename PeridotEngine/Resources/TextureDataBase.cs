using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Resources
{
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
