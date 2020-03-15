#nullable enable

using Microsoft.Xna.Framework;

namespace PeridotEngine.World.Physics.Colliders
{
    public interface ICollider
    {
        bool IsColliding(Rectangle otherRect);
    }
}
