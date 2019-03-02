using Microsoft.Xna.Framework;

namespace PeridotEngine.World.Physics.Colliders
{
    class RectCollider : ICollider
    {
        Rectangle Rect { get; set; }

        public bool IsColliding(Rectangle otherRect)
        {
            return Rect.Intersects(otherRect);
        }
    }
}
