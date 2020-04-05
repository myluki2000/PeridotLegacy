﻿#nullable enable

using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Utility;

namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    class TexturedSolid : Sprite, ISolid, IParallaxable, ITextured
    {
        /// <summary>
        /// The parallax multiplier of the object in the game world.
        /// </summary>
        public float ParallaxMultiplier { get; set; } = 1.0f;

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera)
        {
            base.Draw(sb);
        }

        /// <inheritdoc />
        public XElement ToXml(LazyLoadingTextureDictionary textureDictionary)
        {
            XElement? texPathXEle = Texture != null ? new XElement("TexturePath", textureDictionary.GetTexturePathByName(Texture.Name)) : null;

            return new XElement(this.GetType().Name,
                Position.ToXml("Position"),
                Size.ToXml("Size"),
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
