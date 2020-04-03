#nullable enable

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.UI.UIElements;

namespace PeridotEngine.Engine.UI
{
    abstract class Screen
    {
        public HashSet<UIElement> UIElements { get; set; } = new HashSet<UIElement>();

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch sb);

        public void DrawUI(SpriteBatch sb)
        {
            sb.Begin(transformMatrix: Camera.GetViewportMatrix());
            foreach(UIElement element in UIElements)
            {
                element.Draw(sb);
            }
            sb.End();
        }
    }
}
