#nullable enable

using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml.Linq;

namespace PeridotEngine.Resources
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
        public static TextureData LoadTexture(string contentPath)
        {
            XElement rootEle = XElement.Load(contentPath + ".ptex");

            string randomTextureRotString = rootEle.Element("RandomTextureRotation").Value.ToUpper();
            bool randomTextureRot;

            // check and throw exception in case the file is broken
            switch(randomTextureRotString)
            {
                case "TRUE":
                    randomTextureRot = true;
                    break;
                case "FALSE":
                    randomTextureRot = false;
                    break;
                default:
                    throw new System.Exception("Error while parsing texture data: Invalid xml element value in file " + contentPath);
            }



            TextureData newTexture = new TextureData(rootEle.Element("Name").Value,
                                                     LoadRawTexture(contentPath),
                                                     randomTextureRot);

            return newTexture;
        }
    }
}
