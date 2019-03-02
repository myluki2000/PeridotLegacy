using Microsoft.Xna.Framework;

namespace PeridotEngine.World.Physics.Colliders
{
    interface ICollider
    {
        bool IsColliding(Rectangle otherRect);
    }
}
