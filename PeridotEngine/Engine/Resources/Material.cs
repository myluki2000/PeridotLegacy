#nullable enable

using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Resources
{
    public sealed class Material
    {
        /// <summary>
        /// Unique name of the TextureData object.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Path to the material file.
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// The textures of this material.
        /// </summary>
        public TextureDataBase[] Textures { get; private set; }
        /// <summary>
        /// If set to true the material will be randomly rotated for each sprite it is used on.
        /// </summary>
        public bool HasRandomTextureRotation { get; private set; }
        /// <summary>
        /// Gets the width of the material object. This does not necessarily match the width in pixels of the texture.
        /// </summary>
        public int Width
        {
            get => width ?? Textures[(int)TextureType.Diffuse].Width;
            set => width = value;
        }
        /// <summary>
        /// Gets the height of the texture object. This does not necessarily match the height in pixels of the texture.
        /// </summary>
        public int Height
        {
            get => height ?? Textures[(int)TextureType.Diffuse].Height;
            set => height = value;
        }

        private int? width = null;
        private int? height = null;

        public Material(string name, bool hasRandomTextureRotation, TextureDataBase[] textures)
        {
            this.Name = name;
            this.HasRandomTextureRotation = hasRandomTextureRotation;

            this.Textures = textures;
        }

        public static Material FromXml(string path)
        {
            XElement xEle = XElement.Load(path);
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

            TextureDataBase[] textures = new TextureDataBase[GetHighestTextureTypeValue() + 1];
            // load textures of the material
            foreach (XElement texEle in xEle.Element("Textures").Elements())
            {
                TextureDataBase tex = texEle.Element("Animation") != null
                    ? (TextureDataBase)AnimatedTextureData.FromXml(texEle)
                    : (TextureDataBase)TextureData.FromXml(texEle);


                if (!Enum.TryParse(texEle.Name.LocalName, out TextureType texType))
                {
                    throw new Exception("Error while parsing material file: Unknown texture type.");
                }

                textures[(int)texType] = tex;
            }

            // each material has to have at least a diffuse texture
            if (textures[(int)TextureType.Diffuse] == null)
                throw new Exception("Error while parsing material file: Diffuse texture missing.");

            Material mat = new Material(
                xEle.Element("Name").Value,
                randomTextureRot,
                textures
            );


            // set texture size if set in .pmat file
            XElement xEleWidth = xEle.Element("Width");
            if (xEleWidth != null)
            {
                mat.Width = int.Parse(xEleWidth.Value);
            }

            XElement xEleHeight = xEle.Element("Height");
            if (xEleHeight != null)
            {
                mat.Height = int.Parse(xEleHeight.Value);
            }

            mat.Path = path;

            return mat;
        }


        public static int GetHighestTextureTypeValue()
        {
            return Enum.GetValues(typeof(TextureType)).Cast<int>().Max();
        }

        public enum TextureType
        {
            Diffuse,
            Glow
        }
    }
}
