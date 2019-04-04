#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.UI.UIElements;
using System.Collections.Generic;

namespace PeridotEngine.UI
{
    abstract class Screen
    {
        public HashSet<UIElement> UIElements { get; set; } = new HashSet<UIElement>();

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch sb);

        public void DrawUI(SpriteBatch sb)
        {
            sb.Begin();
            foreach(UIElement element in UIElements)
            {
                element.Draw(sb);
            }
            sb.End();
        }
    }
}
