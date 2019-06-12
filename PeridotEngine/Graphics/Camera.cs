#nullable enable

using Microsoft.Xna.Framework;

namespace PeridotEngine.Graphics
{
    class Camera
    {
        /// <summary>
        /// The translation of the camera view.
        /// </summary>
        public Vector3 Translation { get; set; }
        /// <summary>
        /// The scale of the camera view.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Create a new camera object with default values. Translation = 0, Scale = 1
        /// </summary>
        public Camera()
        {
            Translation = new Vector3(0, 0, 0);
            Scale = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Create a new camera object.
        /// </summary>
        /// <param name="translation">The translation of the camera view</param>
        /// <param name="scale">The scale of the camera view</param>
        public Camera(Vector3 translation, Vector3 scale)
        {
            this.Translation = translation;
            this.Scale = scale;
        }

        /// <returns>Returns a matrix which represents the camera view</returns>
        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(Translation) *
                Matrix.CreateScale(Scale) *
                Matrix.CreateScale((float)Globals.Graphics.PreferredBackBufferHeight / 1080);
                //Matrix.CreateTranslation(new Vector3((float)(Globals.Graphics.GraphicsDevice.Viewport.Width / 2), (float)(Globals.Graphics.GraphicsDevice.Viewport.Height / 2), 0));
        }

        /// <summary>
        /// Returns a matrix of the camera view with the parallax factor factored in.
        /// </summary>
        /// <param name="parallax">The parallax factor</param>
        /// <returns>The matrix representing the camera view</returns>
        public Matrix GetMatrix(Vector3 parallax)
        {
            return Matrix.CreateTranslation(Translation * new Vector3(1 / parallax.X, 1, 1)) *
                Matrix.CreateTranslation(new Vector3(0, 150, 0)) *
                Matrix.CreateScale(Scale) *
                Matrix.CreateScale((float)Globals.Graphics.PreferredBackBufferHeight / 1080);
                //Matrix.CreateTranslation(new Vector3((float)(Globals.Graphics.GraphicsDevice.Viewport.Width / 2), (float)(Globals.Graphics.GraphicsDevice.Viewport.Height / 2), 0));
        }

        /// <summary>
        /// Focuses the camera on a position in the world.
        /// </summary>
        /// <param name="focusPos">The position to focus on</param>
        public void FocusOnPosition(Vector2 focusPos)
        {
            Translation = new Vector3(-focusPos, 0);
        }

        public Vector2 ScreenPosToWorldPos(Vector2 screenPos)
        {
            return new Vector2(
                (screenPos.X / Globals.Graphics.PreferredBackBufferWidth) * 1920 - Translation.X,
                (screenPos.Y / Globals.Graphics.PreferredBackBufferHeight) * 1080 - Translation.Y
            );
        }
    }
}