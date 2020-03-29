#nullable enable

using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;
using PeridotEngine.World.Physics;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.World.WorldObjects.Entities
{
    abstract class Character : Sprite, IEntity, IPhysicsObject
    {
        public abstract string Name { get; set; }
        public abstract Level? Level { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        public abstract float MaxSpeed { get; set; }

        /// <inheritdoc />
        public abstract float Drag { get; set; }
        public bool HasPhysics { get; set; }

        /// <inheritdoc />
        public bool IsGrounded { get; set; }
        public Rectangle BoundingRect => new Rectangle(Position.ToPoint(), Size.ToPoint());

        public abstract void Initialize(Level level);
        public abstract void Update(GameTime gameTime);

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera)
        {
            base.Draw(sb);
        }

        /// <inheritdoc />
        public abstract XElement ToXml(LazyLoadingTextureDictionary textureDictionary);
    }
}
