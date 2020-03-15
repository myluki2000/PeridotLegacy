#nullable enable

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PeridotEngine.World.Physics
{
    public interface IPhysicsObject
    {
        /// <summary>
        /// The position of the object in the level.
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        /// The size of the object.
        /// </summary>
        Vector2 Size { get; set; }
        /// <summary>
        /// A hashset containing all retangles used for collision detection.
        /// </summary>
        HashSet<Rectangle> BoundingRects { get; set; }
        /// <summary>
        /// The velocity of the object.
        /// </summary>
        Vector2 Velocity { get; set; }
        /// <summary>
        /// The acceleration of the object.
        /// </summary>
        Vector2 Acceleration { get; set; }

        /// <summary>
        /// True if object should be affected by physics.
        /// </summary>
        bool HasPhysics { get; set; }
    }
}
