#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.UI
{
    static class ScreenHandler
    {
        private static Screen? selectedScreen;
        private static readonly DevConsole.DevConsole devConsole = new DevConsole.DevConsole();

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

        public static void Draw(SpriteBatch sb)
        {
            selectedScreen?.Draw(sb);

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
