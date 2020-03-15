#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.World.Physics.Colliders
{
    public interface ICollider
    {
        /// <summary>
        /// This method is used to determine if the collider is intersecting with a rectangle. Returns true if it does, false otherwise.
        /// </summary>
        /// <param name="otherRect">The rectangle to check the collision with.</param>
        /// <returns>Returns true if it does, false otherwise.</returns>
        bool IsColliding(Rectangle otherRect);
        /// <summary>
        /// Used to draw a graphical representation of the collider in the level during editing.
        /// </summary>
        /// <param name="sb">A sprite batch.</param>
        void Draw(SpriteBatch sb);
    }
}
