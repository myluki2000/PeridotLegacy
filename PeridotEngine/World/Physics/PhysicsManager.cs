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
        public HashSet<ICollider> Colliders { get; set; }

        public HashSet<IPhysicsObject> PhysicsObjects { get; set; }


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
            // the new position of the object after it has been moved by its velocity
            Vector2 newPos = obj.Position + obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // apply gravity
            obj.Acceleration += new Vector2(0, (float)(700 * gameTime.ElapsedGameTime.TotalSeconds + 0.01));

            // The new rect of the object after it has been moved by its velocity
            Rectangle objRect = new Rectangle(newPos.ToPoint(), obj.Size.ToPoint());

            // apply velocity if object is not colliding with anything
            if (!IsColliding(objRect)) {
                obj.Position += obj.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                obj.Velocity = Vector2.Zero;
            }
        }

        /// <summary>
        /// Helper method to check if rect collides with any collider in the level.
        /// </summary>
        /// <param name="rect">The rect to check</param>
        /// <returns>True if colliding, false otherwise</returns>
        private bool IsColliding(Rectangle rect)
        {
            foreach (ICollider collider in Colliders)
            {
                if (collider.IsColliding(rect))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
