﻿#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Globalization;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.UI;
using PeridotEngine.Engine.UI.DevConsole;
using PeridotEngine.Engine.World;
using PeridotEngine.Game.UI;

namespace PeridotEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
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
            ScreenHandler.SelectedScreen = new StandardLevelScreen(Level.FromFile(@"World\level.plvl"));

            IsMouseVisible = true;
            
            Globals.Graphics.PreferredBackBufferWidth = ConfigManager.CurrentConfig.WindowSize.X;
            Globals.Graphics.PreferredBackBufferHeight = ConfigManager.CurrentConfig.WindowSize.Y;
            Globals.Graphics.ApplyChanges();

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

                Window.Title = (1.0d / gameTime.ElapsedGameTime.TotalSeconds).ToString(CultureInfo.InvariantCulture);

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            ScreenHandler.Draw(spriteBatch);

            GraphicsDevice.Present();
            base.Draw(gameTime);
        }
    }
}
