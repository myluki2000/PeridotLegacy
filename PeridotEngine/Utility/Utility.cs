#nullable enable

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Utility
{
    class Utility
    {
        private static readonly Texture2D dummyTexture = new Texture2D(Globals.Graphics.GraphicsDevice, 1, 1);
        private static readonly BasicEffect basicEffect = new BasicEffect(Globals.Graphics.GraphicsDevice);

        static Utility()
        {
            dummyTexture.SetData(new Color[] { Color.White });

            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(
                0, 
                Globals.Graphics.GraphicsDevice.Viewport.Width,  
                Globals.Graphics.GraphicsDevice.Viewport.Height,
                0,
                0,
                1
            );
        }

        public static void DrawLineStrip(SpriteBatch sb, Vector2[] points, Color color)
        {
            DrawLineStrip(sb, points, color, Matrix.Identity);
        }

        public static void DrawLineStrip(SpriteBatch sb, Vector2[] points, Color color, Matrix viewMatrix)
        {
            basicEffect.View = viewMatrix;

            VertexPositionColor[] verts = new VertexPositionColor[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                verts[i] = new VertexPositionColor(new Vector3(points[i], 0), color);
            }

            basicEffect.CurrentTechnique.Passes[0].Apply();
            sb.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, verts, 0, verts.Length - 1);
        }

        public static void DrawRectangle(SpriteBatch sb, Rectangle rect, Color color)
        {
            sb.Draw(dummyTexture, rect, color);
        }

        public static void DrawOutline(SpriteBatch sb, Rectangle rect, Color color, int thickness)
        {
            if (rect.Width < 0)
            {
                rect.Width = -rect.Width;
                rect.X = rect.X - rect.Width;
            }

            if (rect.Height < 0)
            {
                rect.Height = -rect.Height;
                rect.Y = rect.Y - rect.Height;
            }

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
        public static int[] GetIndicesArray<T>(List<T> verts)
        {
            int[] result = new int[verts.Count];

            for (int i = 0; i < verts.Count; i++)
            {
                result[i] = i;
            }

            return result;
        }

        /// <summary>
        /// Returns an array which contains indices of all elements in the verts list. Basically just 0,1,2,3,4,...,(n-1) if n = verts.Count
        /// </summary>
        /// <param name="verts"></param>
        /// <returns></returns>
        public static int[] GetIndicesArray<T>(T[] verts)
        {
            int[] result = new int[verts.Length];

            for (int i = 0; i < verts.Length; i++)
            {
                result[i] = i;
            }

            return result;
        }
    }
}
