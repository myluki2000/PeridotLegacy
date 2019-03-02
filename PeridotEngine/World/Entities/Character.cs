using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;
using PeridotEngine.World.Physics;
using System.Xml.Linq;

namespace PeridotEngine.World.Entities
{
    abstract class Character : Sprite, IEntity, IPhysicsObject
    {
        public abstract string Name { get; set; }
        public abstract Level Level { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public bool HasPhysics { get; set; }

        public abstract void InitializeFromXML(XElement xEle, LazyLoadingTextureDictionary textures);

        public abstract void Update(GameTime gameTime);
    }
}
