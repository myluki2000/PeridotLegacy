using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeridotEngine.Graphics
{
    class Camera
    {
        public Vector3 Translation;
        public Vector3 Scale;

        public Camera()
        {
            Translation = new Vector3(0, 0, 0);
            Scale = new Vector3(1, 1, 1);
        }

        public Camera(Vector3 translation, Vector3 scale)
        {
            this.Translation = translation;
            this.Scale = scale;
        }

        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(Translation) *
                Matrix.CreateTranslation(new Vector3(0, 150, 0)) *
                Matrix.CreateScale(Scale) *
                Matrix.CreateScale((float)(Globals.graphics.PreferredBackBufferHeight / 1080)) *
                Matrix.CreateTranslation(new Vector3((float)(Globals.graphics.GraphicsDevice.Viewport.Width / 2), (float)(Globals.graphics.GraphicsDevice.Viewport.Height / 2), 0));
        }

        public Matrix GetMatrix(Vector3 parallax)
        {
            return Matrix.CreateTranslation(Translation * new Vector3(1 / parallax.X, 1, 1)) *
                Matrix.CreateTranslation(new Vector3(0, 150, 0)) *
                Matrix.CreateScale(Scale) *
                Matrix.CreateScale((float)(Globals.graphics.PreferredBackBufferHeight / 1080)) *
                Matrix.CreateTranslation(new Vector3((float)(Globals.graphics.GraphicsDevice.Viewport.Width / 2), (float)(Globals.graphics.GraphicsDevice.Viewport.Height / 2), 0));
        }

        public void FocusOnPosition(Vector2 focusPos)
        {
            Translation = new Vector3(-focusPos, 0);
        }
    }
}
