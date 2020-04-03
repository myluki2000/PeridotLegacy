using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.UI.UIElements;

namespace PeridotEngine.Game.UI.UIElements
{
    class StimulantUI : UIElement
    {
        private readonly Sprite timeStimulantSprite;

        public StimulantUI()
        {
            timeStimulantSprite = new Sprite(
                TextureManager.LoadTexture("Content/UI/Stimulants/time"),
                new Vector2(30, 1080 - 130),
                new Vector2(100, 100)
            );
        }

        /// <inheritdoc />
        public override void Draw(SpriteBatch sb)
        {
            if (Visible)
            {
               timeStimulantSprite.Draw(sb); 
            }
        }
    }
}
