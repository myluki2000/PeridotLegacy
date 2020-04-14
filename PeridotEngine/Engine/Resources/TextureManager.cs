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
        /// Loads a "raw" Texture2D without any metadata from the specified path.
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
        /// <param name="contentPath">The relative or absolute path of the material file.</param>
        /// <returns>A Material object containing the textures and its metadata</returns>
        public static Material LoadMaterial(string contentPath)
        {
            if (!contentPath.EndsWith(".pmat"))
                contentPath = contentPath + ".pmat";

            return Material.FromXml(contentPath);
        }
    }
}
