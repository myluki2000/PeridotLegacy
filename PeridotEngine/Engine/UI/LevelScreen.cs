#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.World;

namespace PeridotEngine.Engine.UI
{
    class LevelScreen : Screen
    {
        private Level level;
        /// <summary>
        /// The level the screen is drawing.
        /// </summary>
        public Level Level
        {
            get => level;

            set
            {
                level = value;
                level.Initialize();
            }
        }

        public LevelScreen(Level level)
        {
            this.level = level;
            level.Initialize();
        }

        /// <inheritdoc />
        public override Color BackgroundColor => Level.BackgroundColor;

        public override void Initialize()
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            Level.Draw(sb);

            base.DrawUI(sb);
        }

        /// <inheritdoc />
        public override void DrawGlowMap(SpriteBatch sb)
        {
            Level.DrawGlowMap(sb);
        }

        public override void Update(GameTime gameTime)
        {
            Level.Update(gameTime);
        }
    }
}
