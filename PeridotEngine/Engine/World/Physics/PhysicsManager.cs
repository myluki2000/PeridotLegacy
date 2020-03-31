#nullable enable

using System;
using Microsoft.Xna.Framework;
using PeridotEngine.Engine.World.Physics.Colliders;

namespace PeridotEngine.Engine.World.Physics
{
    public static class PhysicsHelper
    {
        public static bool IsGravityEnabled { get; set; } = true;

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

            // apply acceleration and gravity if activated
            obj.Velocity += new Vector2(obj.Acceleration.X, obj.Acceleration.Y + (IsGravityEnabled ? (float)(700 * gameTime.ElapsedGameTime.TotalSeconds + 0.01) : 0));

            // the new position of the object after it has been moved by its velocity
            Point posDelta = (obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds).ToPoint();

            int xHeightDelta = 0;
            // apply velocity if object is not colliding with anything
            CollidingSide collidingSide = IsColliding(level, obj, ref posDelta, ref xHeightDelta);

            obj.Velocity = new Vector2(
                (!collidingSide.HasFlag(CollidingSide.LEFT) && !collidingSide.HasFlag(CollidingSide.RIGHT)) ? obj.Velocity.X : 0,
                (!collidingSide.HasFlag(CollidingSide.BOTTOM) && !collidingSide.HasFlag(CollidingSide.TOP)) ? obj.Velocity.Y : 0
            );

            obj.Position += new Vector2(
                (!collidingSide.HasFlag(CollidingSide.LEFT) && !collidingSide.HasFlag(CollidingSide.RIGHT)) ? posDelta.X : 0,
                (!collidingSide.HasFlag(CollidingSide.BOTTOM) && !collidingSide.HasFlag(CollidingSide.TOP)) ? posDelta.Y :
                    (collidingSide.HasFlag(CollidingSide.SLOPE)
                        && !collidingSide.HasFlag(CollidingSide.LEFT)
                        && !collidingSide.HasFlag(CollidingSide.RIGHT) ? -xHeightDelta : 0)
            );
        }

        /// <summary>
        /// Helper method to check if object collides with any collider in the level.
        /// </summary>
        /// <param name="level">The level to do the collision check in</param>
        /// <param name="obj">The object to check</param>
        /// <param name="posDelta">How much the object moves from its current position if it doesn't collide</param>
        /// <returns>True if colliding, false otherwise</returns>
        private static CollidingSide IsColliding(Level level, IPhysicsObject obj, ref Point posDelta, ref int xHeightDelta)
        {
            Rectangle newRectX = new Rectangle(obj.BoundingRect.Location + new Point(posDelta.X, 0), obj.BoundingRect.Size);
            Rectangle newRectY = new Rectangle(obj.BoundingRect.Location + new Point(0, posDelta.Y), obj.BoundingRect.Size);
            Rectangle groundedRect = new Rectangle(obj.BoundingRect.Location + new Point(0, 1), obj.BoundingRect.Size);

            CollidingSide result = CollidingSide.NONE;

            obj.IsGrounded = false;

            foreach (ICollider collider in level.Colliders)
            {
                if (collider.IsColliding(newRectY))
                {
                    result |= posDelta.Y > 0 ? CollidingSide.BOTTOM : CollidingSide.TOP;
                }

                if (collider.IsColliding(newRectX))
                {
                    CollidingSide tmpResult = posDelta.X > 0 ? CollidingSide.RIGHT : CollidingSide.LEFT;

                    // try moving upwards until we don't collide anymore for 4 steps, so that player can walk up slopes
                    for (int i = 0; i < 4; i++)
                    {
                        newRectX = new Rectangle(obj.BoundingRect.Location + new Point(posDelta.X, -i), obj.BoundingRect.Size);
                        if (!collider.IsColliding(newRectX))
                        {
                            xHeightDelta = i;
                            tmpResult &= ~CollidingSide.LEFT;
                            tmpResult &= ~CollidingSide.RIGHT;
                            tmpResult |= CollidingSide.SLOPE;
                            break;
                        }
                    }

                    result |= tmpResult;
                }

                if (collider.IsColliding(groundedRect))
                {
                    obj.IsGrounded = true;
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
            LEFT = 8,
            SLOPE = 16
        }
    }
}
