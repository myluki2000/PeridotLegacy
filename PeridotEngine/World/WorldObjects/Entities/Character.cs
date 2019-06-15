#nullable enable

using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;
using PeridotEngine.World.Physics;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PeridotEngine.World.WorldObjects.Entities
{
    abstract class Character : Sprite, IEntity, IPhysicsObject
    {
        public abstract string Name { get; set; }
        public abstract Level Level { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public bool HasPhysics { get; set; }
        public HashSet<Rectangle> BoundingRects {
            get
            {
                HashSet<Rectangle> rects = new HashSet<Rectangle>();

                rects.Add(new Rectangle(Position.ToPoint(), Size.ToPoint()));

                return rects;
            }

            set
            {

            }
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
    }
}
