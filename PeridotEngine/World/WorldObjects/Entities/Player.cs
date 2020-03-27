#nullable enable

using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;

namespace PeridotEngine.World.WorldObjects.Entities
{
    class Player : Character
    {
        public override string Name { get; set; } = "Player";
        public override Level? Level { get; set; }

        /// <inheritdoc />
        public override float MaxSpeed { get; set; } = 360.0f;

        /// <inheritdoc />
        public override float Drag { get; set; } = 20.0f;

        public override void Initialize(Level level)
        {
            this.Level = level;
            HasPhysics = true;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            HandleMovement(keyboardState);
        }

        private void HandleMovement(KeyboardState keyboardState)
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
            else
            {
                Drag = 12.0f;
                Acceleration = new Vector2(0, Acceleration.Y);
            }
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
