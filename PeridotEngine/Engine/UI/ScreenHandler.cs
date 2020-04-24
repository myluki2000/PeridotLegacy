#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Engine.Graphics.Effects;

namespace PeridotEngine.Engine.UI
{
    static class ScreenHandler
    {
        public static Screen? SelectedScreen
        {
            get => selectedScreen;

            set
            {
                selectedScreen = value;

                // init screen
                selectedScreen?.Initialize();
            }
        }

        public static PostProcessingEffect? PostProcessingEffect { get; set; } = new GlowEffect();

        private static Screen? selectedScreen;
        private static readonly DevConsole.DevConsole devConsole = new DevConsole.DevConsole();

        private static RenderTarget2D? scene = null;


        public static void Initialize()
        {
            scene = new RenderTarget2D(Globals.Graphics.GraphicsDevice, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);
        }

        public static void Draw(SpriteBatch sb)
        {
            Globals.Graphics.GraphicsDevice.SetRenderTarget(PostProcessingEffect != null ? scene : null);
            Globals.Graphics.GraphicsDevice.Clear(selectedScreen?.BackgroundColor ?? Color.CornflowerBlue);

            selectedScreen?.Draw(sb);

            PostProcessingEffect?.Apply(sb, scene, selectedScreen);

            devConsole.Draw(sb);
        }

        public static void Update(GameTime gameTime)
        {
            if (!devConsole.IsVisible)
            {
                selectedScreen?.Update(gameTime);
            }

            devConsole.Update(gameTime);
        }
    }
}
