#nullable enable

using Microsoft.Xna.Framework;
using PeridotEngine.World.Physics.Colliders;
using System.Collections.Generic;

namespace PeridotEngine.World.Physics
{
    class PhysicsManager
    {
        /// <summary>
        /// Contains all the physics colliders in the level.
        /// </summary>
        public HashSet<ICollider> Colliders { get; set; } = new HashSet<ICollider>();

        public HashSet<IPhysicsObject> PhysicsObjects { get; set; } = new HashSet<IPhysicsObject>();


        public void UpdatePhysics(GameTime gameTime)
        {
            // do a physics update for all objects affected by physics
            foreach (IPhysicsObject obj in PhysicsObjects)
            {
                if (obj.HasPhysics)
                {
                    DoObjectPhysicsUpdate(obj, gameTime);
                }
            }
        }

        /// <summary>
        /// Helper method to update an objects physics status. THE MAGIC HAPPENS HERE!
        /// </summary>
        /// <param name="obj">The object to which to apply the physics</param>
        /// <param name="gameTime">The current game time</param>
        private void DoObjectPhysicsUpdate(IPhysicsObject obj, GameTime gameTime)
        {
            // apply acceleration and gravity
            obj.Velocity += new Vector2(obj.Acceleration.X, obj.Acceleration.Y + (float)(700 * gameTime.ElapsedGameTime.TotalSeconds + 0.01));

            // the new position of the object after it has been moved by its velocity
            Vector2 posDelta = obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // apply velocity if object is not colliding with anything
            CollidingSide collidingSide = IsColliding(obj, posDelta);

            switch(collidingSide)
            {
                case CollidingSide.BOTTOM:
                case CollidingSide.TOP:
                    obj.Velocity = new Vector2(obj.Velocity.X, 0);
                    obj.Position += new Vector2(posDelta.X, 0);
                    break;

                case CollidingSide.LEFT:
                case CollidingSide.RIGHT:
                    obj.Velocity = new Vector2(0, obj.Velocity.Y);
                    obj.Position += new Vector2(0, posDelta.Y);
                    break;

                case CollidingSide.NONE:
                    obj.Position += posDelta;
                    break;
            }
        }

        /// <summary>
        /// Helper method to check if object collides with any collider in the level.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="posDelta">How much the object moves from its current position if it doesn't collide</param>
        /// <returns>True if colliding, false otherwise</returns>
        private CollidingSide IsColliding(IPhysicsObject obj, Vector2 posDelta)
        {
            // TODO: Handle very small floating point numbers (rounding errors)

            foreach (Rectangle rect in obj.BoundingRects)
            {
                Rectangle newRectX = new Rectangle(rect.Location + new Point((int)posDelta.X), rect.Size);
                Rectangle newRectY = new Rectangle(rect.Location + new Point(0, (int)posDelta.Y), rect.Size);

                foreach (ICollider collider in Colliders)
                {
                    if (posDelta.X != 0 && collider.IsColliding(newRectX))
                    {
                        return posDelta.X > 0 ? CollidingSide.RIGHT : CollidingSide.LEFT;
                    }

                    if (posDelta.Y != 0 && collider.IsColliding(newRectY))
                    {
                        return posDelta.Y > 0 ? CollidingSide.BOTTOM : CollidingSide.TOP;
                    }
                }
            }

            return CollidingSide.NONE;
        }

        private enum CollidingSide
        {
            TOP,
            RIGHT,
            BOTTOM,
            LEFT,
            NONE
        }
    }
}
