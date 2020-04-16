using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World.WorldObjects.Entities
{
    class TopDownPlayer : Player
    {
        public static Player FromXml(XElement xEle, LazyLoadingMaterialDictionary materials)
        {
            return new TopDownPlayer()
            {
                Position = new Vector2().FromXml(xEle.Element("Position")),
                Size = new Vector2().FromXml(xEle.Element("Size")),
                Material = xEle.Element("TexturePath") != null ? materials[xEle.Element("TexturePath").Value] : null,
                ZIndex = sbyte.Parse(xEle.Element("Z-Index").Value),
                Rotation = float.Parse(xEle.Element("Rotation").Value, CultureInfo.InvariantCulture.NumberFormat),
                Opacity = float.Parse(xEle.Element("Opacity").Value, CultureInfo.InvariantCulture.NumberFormat),
            };
        }

        protected override void HandleMovement(KeyboardState keyboardState)
        {
            Drag = 0.0f;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (Velocity.X > -350.0f)
                {
                    Acceleration = new Vector2(-20.0f, Acceleration.Y);
                }
                else
                {
                    Acceleration = new Vector2(0, Acceleration.Y);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                if (Velocity.X < 350.0f)
                {
                    Acceleration = new Vector2(20.0f, Acceleration.Y);
                }
                else
                {
                    Acceleration = new Vector2(0, Acceleration.Y);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.W))
            {
                if (Velocity.Y > 350.0f)
                {
                    Acceleration = new Vector2(Acceleration.X, -20.0f);
                }
                else
                {
                    Acceleration = new Vector2(Acceleration.X, 0);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                if (Velocity.Y < 350.0f)
                {
                    Acceleration = new Vector2(Acceleration.X, 20.0f);
                }
                else
                {
                    Acceleration = new Vector2(Acceleration.X, 0);
                }
            }
            else
            {
                Drag = 12.0f;
                Acceleration = new Vector2(0, Acceleration.Y);
            }
        }

        public override XElement ToXml(LazyLoadingMaterialDictionary materialDictionary)
        {
            if (Material != null)
                materialDictionary.LoadMaterial(Material.Path);
            XElement? texPathXEle = Material != null ? new XElement("TexturePath", materialDictionary.GetTexturePathByName(Material.Name)) : null;

            return new XElement(this.GetType().Name,
                Position.ToXml("Position"),
                Size.ToXml("Size"),
                texPathXEle,
                new XElement("Rotation", Rotation.ToString(CultureInfo.InvariantCulture)),
                new XElement("Opacity", Opacity.ToString(CultureInfo.InvariantCulture)),
                new XElement("Z-Index", ZIndex.ToString(CultureInfo.InvariantCulture))
            );
        }
    }
}
