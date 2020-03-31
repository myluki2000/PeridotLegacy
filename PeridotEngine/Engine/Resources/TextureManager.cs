#nullable enable

using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Resources
{
    static class TextureManager
    {
        /// <summary>
        /// Load a "raw" Texture2D without any metadata from the specified path.
        /// </summary>
        /// <param name="contentPath">The relative or absolute path without a file extension</param>
        /// <returns>The Texture2D which has been loaded</returns>
        public static Texture2D LoadRawTexture(string contentPath)
        {
            string path = contentPath + ".png";
            FileStream fs = new FileStream(path, FileMode.Open);
            Texture2D tex = Texture2D.FromStream(Globals.Graphics.GraphicsDevice, fs);
            fs.Dispose();
            return tex;
        }

        /// <summary>
        /// Load a texture and its metadata from the specified path and return it.
        /// </summary>
        /// <param name="contentPath">The relative or absolute path without a file extension</param>
        /// <returns>A TextureData object containing the texture and its metadata</returns>
        public static TextureDataBase LoadTexture(string contentPath)
        {
            XElement rootEle = XElement.Load(contentPath + ".ptex");

            switch (rootEle.Name.LocalName)
            {
                case "Texture":
                    return TextureData.FromXml(rootEle);
                case "AnimatedTexture":
                    return AnimatedTextureData.FromXml(rootEle);
                default:
                    throw new Exception("TextureData type mentioned in texture xml file " + contentPath + " not supported.");
            }
        }
    }
}
