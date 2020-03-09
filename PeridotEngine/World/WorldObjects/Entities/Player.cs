#nullable enable

using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;

namespace PeridotEngine.World.WorldObjects.Entities
{
    class Player : Character
    {
        public override string Name { get; set; } = "Player";
        public override Level? Level { get; set; }

        public override void Initialize(Level level)
        {
            this.Level = level;
            HasPhysics = true;
        }

        public override void Update(GameTime gameTime)
        {

        }

        /// <inheritdoc />
        public override XElement ToXml(LazyLoadingTextureDictionary textureDictionary)
        {
            XElement? texPathXEle = Texture != null ? new XElement("TexturePath", textureDictionary.GetTexturePathByName(Texture.Name)) : null;

            return new XElement("Entity",
                new XElement("Type", this.GetType().Name),
                new XElement("Position", Position.ToXml()),
                new XElement("Size", Size.ToXml()),
                texPathXEle,
                new XElement("Rotation", Rotation.ToString(CultureInfo.InvariantCulture)),
                new XElement("Opacity", Opacity.ToString(CultureInfo.InvariantCulture)),
                new XElement("Z-Index", ZIndex.ToString(CultureInfo.InvariantCulture))
            );
        }

        public static Player FromXml(XElement xEle, LazyLoadingTextureDictionary textures)
        {
            return new Player()
            {
                Position = new Vector2().FromXml(xEle.Element("Position")),
                Size = new Vector2().FromXml(xEle.Element("Size")),
                Texture = xEle.Element("TexturePath") != null ? textures[xEle.Element("TexturePath").Value] : null,
                ZIndex = sbyte.Parse(xEle.Element("Z-Index").Value),
                Rotation = float.Parse(xEle.Element("Rotation").Value, CultureInfo.InvariantCulture.NumberFormat),
                Opacity = float.Parse(xEle.Element("Opacity").Value, CultureInfo.InvariantCulture.NumberFormat),
        };
        }


    }
}
