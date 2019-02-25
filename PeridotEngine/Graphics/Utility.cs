﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Graphics
{
    class Utility
    {
        private static Texture2D DummyTexture = new Texture2D(Globals.Graphics.GraphicsDevice, 1, 1);

        static Utility()
        {
            DummyTexture.SetData(new Color[] { Color.White });
        }

        public static void DrawRectangle(SpriteBatch sb, Rectangle rect, Color color)
        {
            sb.Draw(DummyTexture, rect, color);
        }

        public static void DrawOutline(SpriteBatch sb, Rectangle rect, Color color, int thickness)
        {
            DrawRectangle(sb, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color); // Outline Top
            DrawRectangle(sb, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color); // Outline Bottom
            DrawRectangle(sb, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color); // Outline Left
            DrawRectangle(sb, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color); // Outline Right
        }
    }
}
