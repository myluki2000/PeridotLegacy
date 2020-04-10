#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Globalization;
using PeridotEngine.Engine.Graphics.Effects;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.UI;
using PeridotEngine.Engine.UI.DevConsole;
using PeridotEngine.Engine.World;
using PeridotEngine.Engine.World.WorldObjects.Solids;

namespace PeridotEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private SpriteBatch? spriteBatch;

        public Main()
        {
            Globals.Graphics = new GraphicsDeviceManager(this);
            EventInput.Initialize(Window);
            Globals.Content = Content;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            Globals.Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Globals.Graphics.ApplyChanges();

            ScreenHandler.SelectedScreen = new LevelScreen(Level.FromFile(@"World\level.plvl"));

            Globals.Graphics.PreferredBackBufferWidth = ConfigManager.CurrentConfig.WindowSize.X;
            Globals.Graphics.PreferredBackBufferHeight = ConfigManager.CurrentConfig.WindowSize.Y;
            Globals.Graphics.PreferMultiSampling = false;
            Globals.Graphics.ApplyChanges();

            

            ScreenHandler.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            FontManager.LoadFonts(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                ScreenHandler.Update(gameTime);

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            ScreenHandler.Draw(spriteBatch);

            Window.Title = (1000.0f / gameTime.ElapsedGameTime.TotalMilliseconds).ToString("0.00");

            base.Draw(gameTime);
        }
    }
}
