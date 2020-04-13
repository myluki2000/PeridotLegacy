#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;
using PeridotEngine.Engine.World.Physics;

namespace PeridotEngine.Engine.World.WorldObjects.Entities
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
        public new abstract void Update(GameTime gameTime);

        public virtual bool ContainsPointOnScreen(Point point, Camera camera)
        {
            return BoundingRect.Contains(point.Transform(camera.GetMatrix().Invert()));
        }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera)
        {
            base.Draw(sb);
        }

        /// <inheritdoc />
        public virtual void DrawGlowMap(SpriteBatch sb, Camera camera) { }

        /// <inheritdoc />
        public virtual void DrawOutline(SpriteBatch sb, Color color, Camera camera)
        {
            Utility.Utility.DrawOutline(sb, BoundingRect.Transform(camera.GetMatrix()), color, 1);
        }

        /// <inheritdoc />
        public abstract XElement ToXml(LazyLoadingMaterialDictionary materialDictionary);
    }
}
