#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Editor.Forms;
using PeridotEngine.Resources;
using PeridotEngine.UI.DevConsole;
using PeridotEngine.World;

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
        }

        public override void Draw(SpriteBatch sb)
        {
            Level.Draw(sb);

            base.DrawUI(sb);
        }

        public override void Update(GameTime gameTime)
        {
            Level.Update(gameTime);
        }
    }
}
