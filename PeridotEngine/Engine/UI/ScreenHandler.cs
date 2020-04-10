#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics.Effects;

namespace PeridotEngine.Engine.UI
{
    static class ScreenHandler
    {
        private static Screen? selectedScreen;
        private static readonly DevConsole.DevConsole devConsole = new DevConsole.DevConsole();

        private static RenderTarget2D? scene = null;
        private static RenderTarget2D? glowMap = null;

        private static BlurEffect blurEffect;

        internal static Screen? SelectedScreen
        {
            get => selectedScreen;

            set
            {
                selectedScreen = value;
                
                // init screen
                selectedScreen?.Initialize();
            }
        }

        public static void Initialize()
        {
            scene = new RenderTarget2D(Globals.Graphics.GraphicsDevice, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);
            glowMap = new RenderTarget2D(Globals.Graphics.GraphicsDevice, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);

            blurEffect = new BlurEffect(Globals.Content.Load<Effect>("BlurEffect"));
        }

        public static void Draw(SpriteBatch sb)
        {
            Globals.Graphics.GraphicsDevice.SetRenderTargets(scene, glowMap);
            Globals.Graphics.GraphicsDevice.Clear(Color.Transparent);

            selectedScreen?.Draw(sb);

            Globals.Graphics.GraphicsDevice.SetRenderTarget(null);
            Globals.Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            blurEffect.Texture = scene;

            sb.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Immediate);
            sb.Draw(scene, new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight), Color.White);
            sb.End();
            sb.Begin(blendState: BlendState.Additive, sortMode: SpriteSortMode.Immediate, effect: blurEffect);
            sb.Draw(glowMap, new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight), Color.White * 0.9f);
            sb.End();

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
