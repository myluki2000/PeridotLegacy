#nullable enable

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Resources
{
    /// <summary>
    /// Loads and gives access to the fonts used throughout the game.
    /// </summary>
    static class FontManager
    {
        /// <summary>
        /// Fonts you can use.
        /// </summary>
        public static class Fonts
        {
            public static class ChakraPetch
            {
                public static SpriteFont Regular { get; set; }
            }
        }

        /// <summary>
        /// Loads the fonts from font files in the game directory.
        /// </summary>
        /// <param name="content">The game's content manager</param>
        public static void LoadFonts(ContentManager content)
        {
            Fonts.ChakraPetch.Regular = content.Load<SpriteFont>(@"Fonts/ChakraPetch/ChakraPetch-Normal");
        }

    }
}
