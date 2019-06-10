#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Editor.Forms;
using PeridotEngine.Resources;
using PeridotEngine.World;
using System.IO;

namespace PeridotEngine.UI
{
    class EditorScreen : Screen
    {
        private readonly ToolbarForm toolbarForm = new ToolbarForm();
        private readonly ToolboxForm toolboxForm = new ToolboxForm();

        private string levelPath;

        private Level _level;
        /// <summary>
        /// The level which is being edited.
        /// </summary>
        public Level Level
        {
            get => _level;

            set
            {
                _level = value;
                _level.Initialize();
            }
        }

        public EditorScreen(string lvlPath)
        {
            this.levelPath = lvlPath;

            // load level if it exists or create it if it doesn't
            if (File.Exists(lvlPath))
            {
                // load level
                _level = LevelManager.LoadLevel(lvlPath);
                _level.Initialize();
            }
            else
            {
                // create new level
                _level = new Level();
                _level.Initialize();
            }
        }

        public override void Initialize()
        {
            toolbarForm.Show();
            toolboxForm.Show();

            toolboxForm.PopulateSolidsFromTextureDirectory(Level.TextureDirectory);
        }

        public override void Draw(SpriteBatch sb)
        {
            Level.Draw(sb);

            base.DrawUI(sb);
        }

        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            Level.Update(gameTime);

            HandleCameraDrag(lastMouseState, mouseState);

            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
        }

        private void HandleCameraDrag(MouseState lastMouseState, MouseState mouseState)
        {
            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                Level.Camera.Translation = new Vector3(
                    Level.Camera.Translation.X + mouseState.Position.X - lastMouseState.Position.X,
                    Level.Camera.Translation.Y + mouseState.Position.Y - lastMouseState.Position.Y,
                    0
                );
            }
        }
    }
}
