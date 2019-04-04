#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.UI
{
    static class ScreenHandler
    {
        private static Screen selectedScreen;
        internal static Screen SelectedScreen
        {
            get => selectedScreen;

            set
            {
                selectedScreen = value;
                
                // init screen
                if(selectedScreen != null)
                    selectedScreen.Initialize();
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            if (selectedScreen != null)
                selectedScreen.Draw(sb);
        }

        public static void Update(GameTime gameTime)
        {
            if (selectedScreen != null)
                selectedScreen.Update(gameTime);
        }
    }
}
