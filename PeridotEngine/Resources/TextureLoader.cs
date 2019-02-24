using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace PeridotEngine.Resources
{
    static class TextureLoader
    {
        public static Texture2D Load(string contentPath)
        {
            string path = contentPath + ".png";
            FileStream fs = new FileStream(path, FileMode.Open);
            Texture2D tex = Texture2D.FromStream(Globals.graphics.GraphicsDevice, fs);
            fs.Dispose();
            return tex;
        }
    }
}
