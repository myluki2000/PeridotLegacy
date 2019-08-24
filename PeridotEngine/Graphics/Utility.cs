#nullable enable

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Graphics
{
    class Utility
    {
        private static readonly Texture2D dummyTexture = new Texture2D(Globals.Graphics.GraphicsDevice, 1, 1);

        static Utility()
        {
            dummyTexture.SetData(new Color[] { Color.White });
        }

        public static void DrawRectangle(SpriteBatch sb, Rectangle rect, Color color)
        {
            sb.Draw(dummyTexture, rect, color);
        }

        public static void DrawOutline(SpriteBatch sb, Rectangle rect, Color color, int thickness)
        {
            DrawRectangle(sb, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color); // Outline Top
            DrawRectangle(sb, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color); // Outline Bottom
            DrawRectangle(sb, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color); // Outline Left
            DrawRectangle(sb, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color); // Outline Right
        }

        /// <summary>
        /// Returns an array which contains indices of all elements in the verts list. Basically just 0,1,2,3,4,...,(n-1) if n = verts.Count
        /// </summary>
        /// <param name="verts"></param>
        /// <returns></returns>
        public static int[] GetIndicesArray(List<VertexPositionColor> verts)
        {
            int[] result = new int[verts.Count];

            for (int i = 0; i < verts.Count; i++)
            {
                result[i] = i;
            }

            return result;
        }
    }
}
