﻿#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.World;

namespace PeridotEngine.UI
{
    class LevelScreen : Screen
    {
        private Level _level;
        /// <summary>
        /// The level the screen is drawing.
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

        public LevelScreen(Level level)
        {
            this._level = level;
            _level.Initialize();
        }

        public override void Initialize()
        {
            // This should not be necessary anymore because levels automatically get inited when set
            //Level.Initialize();
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
