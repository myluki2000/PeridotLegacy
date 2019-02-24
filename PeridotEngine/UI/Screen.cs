using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.UI.UIElements;
using System.Collections.Generic;

namespace PeridotEngine.UI
{
    abstract class Screen
    {
        public List<UIElement> UIElements { get; set; } = new List<UIElement>();

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
        }
    }
}
