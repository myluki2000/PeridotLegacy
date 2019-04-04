#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.UI.UIElements
{
    abstract class UIElement
    {
        public Rectangle Rect { get; set; }
        public bool Visible { get; set; } = true;

        public abstract void Draw(SpriteBatch sb);

    }
}
