#nullable enable

using Microsoft.Xna.Framework;

namespace PeridotEngine.Engine.Graphics
{
    public class Camera
    {
        /// <summary>
        /// The translation of the camera view.
        /// </summary>
        public Vector3 Translation
        {
            get => translation;
            set
            {
                translation = value;
                viewMatrixValid = false;
            }
        }

        /// <summary>
        /// The scale of the camera view.
        /// </summary>
        public Vector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                viewMatrixValid = false;
            }
        }

        private Vector3 translation;
        private Vector3 scale;

        private Matrix viewMatrix;
        private bool viewMatrixValid = false;
        

        /// <summary>
        /// Create a new camera object with default values. Translation = 0, Scale = 1
        /// </summary>
        public Camera()
        {
            Translation = new Vector3(0, 0, 0);
            Scale = new Vector3(1, 1, 1);
            viewMatrix = GetMatrix();
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
            if (!viewMatrixValid)
            {
                viewMatrix = Matrix.CreateTranslation(Translation)
                             * Matrix.CreateScale(Scale)
                             * Matrix.CreateScale((float)Globals.Graphics.PreferredBackBufferHeight / 1080);
                viewMatrixValid = true;
            }

            return viewMatrix;

        }

        /// <summary>
        /// Returns a matrix of the camera view with the parallax factor factored in.
        /// </summary>
        /// <param name="parallax">The parallax factor</param>
        /// <returns>The matrix representing the camera view</returns>
        public Matrix GetMatrix(Vector3 parallax)
        {
            return Matrix.CreateTranslation(Translation * new Vector3(1 / parallax.X, 1, 1)) *
                Matrix.CreateScale(Scale) *
                Matrix.CreateScale((float)Globals.Graphics.PreferredBackBufferHeight / 1080);
        }

        /// <summary>
        /// Focuses the camera on a position in the world.
        /// </summary>
        /// <param name="focusPos">The position to focus on</param>
        /// <param name="horizontalAlignment">The horizontal alignment of the position the camera should focus on.</param>
        /// <param name="verticalAlignment">The vertical alignment of the position the camera should focus on.</param>
        public void FocusOnPosition(Vector2 focusPos,
                                    HorizontalAlignment horizontalAlignment = HorizontalAlignment.CENTER,
                                    VerticalAlignment verticalAlignment = VerticalAlignment.MIDDLE)
        {
            Vector2 displacement = new Vector2(0, 0);
            displacement.X = horizontalAlignment switch
            {
                HorizontalAlignment.LEFT => 0,
                HorizontalAlignment.CENTER => (1920 / 2),
                HorizontalAlignment.RIGHT => 1920,
                _ => displacement.X
            };

            displacement.Y = verticalAlignment switch
            {
                VerticalAlignment.TOP => 0,
                VerticalAlignment.MIDDLE => (1080 / 2),
                VerticalAlignment.BOTTOM => 1080,
                _ => displacement.Y
            };

            Translation = new Vector3(-focusPos + displacement, 0);
        }

        public Vector2 ScreenPosToWorldPos(Vector2 screenPos)
        {
            return new Vector2(
                (screenPos.X / Globals.Graphics.PreferredBackBufferWidth) * 1920 - Translation.X,
                (screenPos.Y / Globals.Graphics.PreferredBackBufferHeight) * 1080 - Translation.Y
            );
        }

        public enum VerticalAlignment
        {
            TOP,
            MIDDLE,
            BOTTOM
        }

        public enum HorizontalAlignment
        {
            LEFT,
            CENTER,
            RIGHT
        }
    }
}