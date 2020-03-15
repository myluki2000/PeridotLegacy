#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Misc;

namespace PeridotEngine.World.Physics.Colliders
{
    public class RectCollider : ICollider
    {
        public Rectangle Rect { get; set; }

        public bool IsColliding(Rectangle otherRect)
        {
            return Rect.Intersects(otherRect);
        }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb)
        {
            Utility.DrawOutline(sb, Rect, Color.Green, 1);
        }
    }
}
