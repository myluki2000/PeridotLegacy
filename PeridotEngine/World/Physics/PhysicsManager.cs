#nullable enable

using System;
using Microsoft.Xna.Framework;
using PeridotEngine.World.Physics.Colliders;
using System.Collections.Generic;
using System.Diagnostics;

namespace PeridotEngine.World.Physics
{
    public static class PhysicsHelper
    {
        public static void UpdatePhysics(Level level, GameTime gameTime)
        {
            // do a physics update for all objects affected by physics
            foreach (IPhysicsObject obj in level.PhysicsObjects)
            {
                if (obj.HasPhysics)
                {
                    DoObjectPhysicsUpdate(level, obj, gameTime);
                }
            }
        }

        /// <summary>
        /// Helper method to update an objects physics status. THE MAGIC HAPPENS HERE!
        /// </summary>
        /// <param name="level">The level to do the physics update for</param>
        /// <param name="obj">The object to which to apply the physics</param>
        /// <param name="gameTime">The current game time</param>
        private static void DoObjectPhysicsUpdate(Level level, IPhysicsObject obj, GameTime gameTime)
        {
            obj.Velocity -= new Vector2(
                (obj.Acceleration.X == 0) ? Math.Sign(obj.Velocity.X) * obj.Drag : 0,
                0
            );

            // apply acceleration and gravity
            obj.Velocity += new Vector2(obj.Acceleration.X, obj.Acceleration.Y + (float)(700 * gameTime.ElapsedGameTime.TotalSeconds + 0.01));

            // the new position of the object after it has been moved by its velocity
            Point posDelta = (obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds).ToPoint();
            
            // apply velocity if object is not colliding with anything
            CollidingSide collidingSide = IsColliding(level, obj, posDelta);

            obj.Velocity = new Vector2(
                (!collidingSide.HasFlag(CollidingSide.LEFT) && !collidingSide.HasFlag(CollidingSide.RIGHT)) ? obj.Velocity.X : 0,
                (!collidingSide.HasFlag(CollidingSide.BOTTOM) && !collidingSide.HasFlag(CollidingSide.TOP)) ? obj.Velocity.Y : 0
            );

            obj.Position += new Vector2(
                (!collidingSide.HasFlag(CollidingSide.LEFT) && !collidingSide.HasFlag(CollidingSide.RIGHT)) ? posDelta.X : 0,
                (!collidingSide.HasFlag(CollidingSide.BOTTOM) && !collidingSide.HasFlag(CollidingSide.TOP)) ? posDelta.Y : 0
            );
        }

        /// <summary>
        /// Helper method to check if object collides with any collider in the level.
        /// </summary>
        /// <param name="level">The level to do the collision check in</param>
        /// <param name="obj">The object to check</param>
        /// <param name="posDelta">How much the object moves from its current position if it doesn't collide</param>
        /// <returns>True if colliding, false otherwise</returns>
        private static CollidingSide IsColliding(Level level, IPhysicsObject obj, Point posDelta)
        {
            Rectangle newRectX = new Rectangle(obj.BoundingRect.Location + new Point(posDelta.X, 0), obj.BoundingRect.Size);
            Rectangle newRectY = new Rectangle(obj.BoundingRect.Location + new Point(0, posDelta.Y), obj.BoundingRect.Size);


            CollidingSide result = CollidingSide.NONE;

            foreach (ICollider collider in level.Colliders)
            {
                if (collider.IsColliding(newRectY))
                {
                    result |= posDelta.Y > 0 ? CollidingSide.BOTTOM : CollidingSide.TOP;
                }

                if (collider.IsColliding(newRectX))
                {
                    result |= posDelta.X > 0 ? CollidingSide.RIGHT : CollidingSide.LEFT;
                }
            }
            

            return result;
        }

        [Flags]
        private enum CollidingSide
        {
            NONE = 0,
            TOP = 1,
            RIGHT = 2,
            BOTTOM = 4,
            LEFT = 8
        }
    }
}
