#nullable enable

using System;
using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;
using System.Globalization;
using System.Xml.Linq;

namespace PeridotEngine.World.WorldObjects.Solids
{
    class TexturedSolid : Sprite, ISolid, IParallaxable
    {
        /// <summary>
        /// The parallax multiplier of the object in the game world.
        /// </summary>
        public float ParallaxMultiplier { get; set; } = 1.0f;

        public void Update(GameTime gameTime) { }

        /// <inheritdoc />
        public XElement ToXml(LazyLoadingTextureDictionary textureDictionary)
        {
            XElement? texPathXEle = Texture != null ? new XElement("TexturePath", textureDictionary.GetTexturePathByName(Texture.Name)) : null;

            return new XElement("Solid",
                new XElement("Type", this.GetType().Name),
                new XElement("Position", Position.ToXml()),
                new XElement("Size", Size.ToXml()),
                texPathXEle,
                new XElement("Rotation", Rotation.ToString(CultureInfo.InvariantCulture)),
                new XElement("Opacity", Opacity.ToString(CultureInfo.InvariantCulture)),
                new XElement("Z-Index", ZIndex.ToString(CultureInfo.InvariantCulture)),
                new XElement("Parallax", ParallaxMultiplier.ToString(CultureInfo.InvariantCulture))
            );
        }

        public void Initialize(Level level) { }

        public static TexturedSolid FromXml(XElement xEle, LazyLoadingTextureDictionary textures)
        {
            TexturedSolid obj = new TexturedSolid();

            // get pos from xml
            obj.Position = new Vector2().FromXml(xEle.Element("Position"));

            // get size from xml
            obj.Size = new Vector2().FromXml(xEle.Element("Size"));

            // get z-index
            obj.ZIndex = sbyte.Parse(xEle.Element("Z-Index").Value);

            // get texture from lazy loading dictionary provided by the LevelManager
            obj.Texture = textures[xEle.Element("TexturePath").Value];

            // get rotation
            obj.Rotation = float.Parse(xEle.Element("Rotation").Value, CultureInfo.InvariantCulture.NumberFormat);

            // get opacity
            obj.Opacity = float.Parse(xEle.Element("Opacity").Value, CultureInfo.InvariantCulture.NumberFormat);

            // get parallax multiplier. If no parallax multiplier defined set to 1
            obj.ParallaxMultiplier = float.Parse(xEle.Element("Parallax")?.Value ?? "1", CultureInfo.InvariantCulture);
            

            return obj;
        }
    }
}
