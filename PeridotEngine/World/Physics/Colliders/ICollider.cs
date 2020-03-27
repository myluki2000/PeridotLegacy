#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        /// <param name="point">The point to check.</param>
        /// <returns>Returns true if the specified parameter is inside the collider, false otherwise.</returns>
        bool Contains(Point point);
        /// <summary>
        /// Used to draw a graphical representation of the collider in the level during editing.
        /// </summary>
        /// <param name="sb">A sprite batch.</param>
        /// <param name="color">The color the representation of the collider should be in.</param>
        /// <param name="drawDragPoints">If "drag points" should be drawn, where the user can drag the collider to edit it.</param>
        void Draw(SpriteBatch sb, Color color, bool drawDragPoints);

        void HandleDraggingAndResizing(Level level, MouseState lastMouseState, MouseState mouseState);

        /// <summary>
        /// Serializes the collider to an xml format to later be read in again using FromXml().
        /// </summary>
        /// <returns></returns>
        XElement ToXml();
    }
}
