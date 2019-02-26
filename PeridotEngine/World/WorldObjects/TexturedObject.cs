using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;

namespace PeridotEngine.World.WorldObjects
{
    class TexturedObject : Sprite, IWorldObject
    {
        public void Update(GameTime gameTime) { }

        public void InitializeFromXML(XElement xEle, LazyLoadingTextureDictionary textures)
        {
            // get pos from xml
            Vector2 pos;
            pos.X = float.Parse(xEle.Element("X").Value, CultureInfo.InvariantCulture.NumberFormat);
            pos.Y = float.Parse(xEle.Element("Y").Value, CultureInfo.InvariantCulture.NumberFormat);

            this.Position = pos;

            // get size from xml
            Vector2 size;
            size.X = float.Parse(xEle.Element("Width").Value, CultureInfo.InvariantCulture.NumberFormat);
            size.Y = float.Parse(xEle.Element("Height").Value, CultureInfo.InvariantCulture.NumberFormat);

            this.Size = size;

            // get texture from lazy loading dictionary provided by the LevelManager
            this.Texture = textures[xEle.Element("TexturePath").Value];

            // get rotation
            this.Rotation = float.Parse(xEle.Element("Rotation").Value, CultureInfo.InvariantCulture.NumberFormat);

            // get opacity
            this.Opacity = float.Parse(xEle.Element("Opacity").Value, CultureInfo.InvariantCulture.NumberFormat);


        }
    }
}
