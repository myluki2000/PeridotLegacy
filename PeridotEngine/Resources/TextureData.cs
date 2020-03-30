#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Resources
{
    public sealed class TextureData : TextureDataBase
    {
        /// <summary>
        /// Gets the width of the texture object. This does not necessarily match the width in pixels of the texture.
        /// </summary>
        public override int Width
        {
            get => width ?? Texture.Width;
            set => width = value;
        }
        /// <summary>
        /// Gets the height of the texture object. This does not necessarily match the height in pixels of the texture.
        /// </summary>
        public override int Height
        {
            get => height ?? Texture.Height;
            set => height = value;
        }

        private int? width = null;
        private int? height = null;

        public TextureData(string name, Texture2D texture, bool hasRandomTextureRotation)
        {
            this.Name = name;
            this.Texture = texture;
            this.HasRandomTextureRotation = hasRandomTextureRotation;
        }

        public static TextureData FromXml(XElement xEle)
        {
            string randomTextureRotString = xEle.Element("RandomTextureRotation").Value.ToUpper();
            bool randomTextureRot;

            // check and throw exception in case the file is broken
            switch (randomTextureRotString)
            {
                case "TRUE":
                    randomTextureRot = true;
                    break;
                case "FALSE":
                    randomTextureRot = false;
                    break;
                default:
                    throw new System.Exception("Error while parsing texture data: Invalid xml value for random texture rotation for texture " + xEle.Element("Name").Value);
            }

            TextureData tex = new TextureData(
                xEle.Element("Name").Value,
                TextureManager.LoadRawTexture(xEle.Element("ImagePath").Value),
                randomTextureRot
            );

            // set texture size if set in .ptex file
            XElement xEleWidth = xEle.Element("Width");
            if (xEleWidth != null)
            {
                tex.Width = int.Parse(xEleWidth.Value);
            }

            XElement xEleHeight = xEle.Element("Height");
            if (xEleHeight != null)
            {
                tex.Height = int.Parse(xEleHeight.Value);
            }

            return tex;
        }
    }
}
