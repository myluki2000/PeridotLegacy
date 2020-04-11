#nullable enable

using System;
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
        /// The texture data of this TextureData object.
        /// </summary>
        public TextureDataBase Diffuse { get; private set; }
        /// <summary>
        /// The glow map of the texture. NULL if material does not have a glow map.
        /// </summary>
        public TextureDataBase? GlowMap { get; private set; }
        /// <summary>
        /// If set to true the material will be randomly rotated for each sprite it is used on.
        /// </summary>
        public bool HasRandomTextureRotation { get; private set; }
        /// <summary>
        /// Gets the width of the material object. This does not necessarily match the width in pixels of the texture.
        /// </summary>
        public int Width
        {
            get => width ?? Diffuse.Width;
            set => width = value;
        }
        /// <summary>
        /// Gets the height of the texture object. This does not necessarily match the height in pixels of the texture.
        /// </summary>
        public int Height
        {
            get => height ?? Diffuse.Height;
            set => height = value;
        }

        private int? width = null;
        private int? height = null;

        public Material(string name, bool hasRandomTextureRotation, TextureDataBase diffuse, TextureDataBase? glowMap = null)
        {
            this.Name = name;
            this.HasRandomTextureRotation = hasRandomTextureRotation;

            this.Diffuse = diffuse;
            this.GlowMap = glowMap;
        }

        public static Material FromXml(XElement xEle)
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

            TextureDataBase? diffuse = null, glowMap = null;
            // load textures of the material
            foreach (XElement texEle in xEle.Element("Textures").Elements())
            {
                TextureDataBase tex = texEle.Element("Animation") != null
                    ? (TextureDataBase)AnimatedTextureData.FromXml(texEle)
                    : (TextureDataBase)TextureData.FromXml(texEle);

                switch (texEle.Name.LocalName)
                {
                    case "Diffuse":
                        diffuse = tex;
                        break;
                    case "Glow":
                        glowMap = tex;
                        break;
                    default:
                        throw new Exception("Error while parsing material file: Unknown texture type.");
                }
            }

            if(diffuse == null)
                throw new Exception("Error while parsing material file: Diffuse texture missing.");

            Material mat = new Material(
                xEle.Element("Name").Value,
                randomTextureRot,
                diffuse,
                glowMap
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

            return mat;
        }
    }
}
