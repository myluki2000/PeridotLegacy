#nullable enable

using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;
using System.Globalization;
using System.Xml.Linq;

namespace PeridotEngine.World.WorldObjects.Solids
{
    class TexturedObject : Sprite, ISolid
    {
        /// <summary>
        /// The parallax multiplier of the object in the game world.
        /// </summary>
        public float ParallaxMultiplier { get; set; } = 1.0f;

        public void Update(GameTime gameTime) { }

        public void Initialize(Level level) { }

        public static TexturedObject FromXML(XElement xEle, LazyLoadingTextureDictionary textures)
        {
            TexturedObject obj = new TexturedObject();

            // get pos from xml
            Vector2 pos;
            pos.X = float.Parse(xEle.Element("X").Value, CultureInfo.InvariantCulture.NumberFormat);
            pos.Y = float.Parse(xEle.Element("Y").Value, CultureInfo.InvariantCulture.NumberFormat);

            obj.Position = pos;

            // get size from xml
            Vector2 size;
            size.X = float.Parse(xEle.Element("Width").Value, CultureInfo.InvariantCulture.NumberFormat);
            size.Y = float.Parse(xEle.Element("Height").Value, CultureInfo.InvariantCulture.NumberFormat);

            obj.Size = size;

            // get z-index
            obj.ZIndex = int.Parse(xEle.Element("Z-Index").Value);

            // get texture from lazy loading dictionary provided by the LevelManager
            obj.Texture = textures[xEle.Element("TexturePath").Value];

            // get rotation
            obj.Rotation = float.Parse(xEle.Element("Rotation").Value, CultureInfo.InvariantCulture.NumberFormat);

            // get opacity
            obj.Opacity = float.Parse(xEle.Element("Opacity").Value, CultureInfo.InvariantCulture.NumberFormat);

            return obj;
        }
    }
}
