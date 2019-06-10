#nullable enable

using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Resources
{
    public class TextureData
    {
        /// <summary>
        /// Unique name of the TextureData object.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The texture data of this TextureData object.
        /// </summary>
        public Texture2D Texture { get; private set; }
        /// <summary>
        /// If set to true the texture will be randomly rotated for each sprite it is used on.
        /// </summary>
        public bool HasRandomTextureRotation { get; private set; }


        public TextureData(string name, Texture2D texture, bool hasRandomTextureRotation)
        {
            this.Name = name;
            this.Texture = texture;
            this.HasRandomTextureRotation = hasRandomTextureRotation;
        }
    }
}
