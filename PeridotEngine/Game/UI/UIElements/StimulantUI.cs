using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.UI.UIElements;

namespace PeridotEngine.Game.UI.UIElements
{
    class StimulantUI : UIElement
    {
        private Sprite timeStimulantSprite;

        public StimulantUI()
        {
            /*timeStimulantSprite = new Sprite(
                TextureManager.LoadTexture("Content/UI/Stimulants/time"),
                new Vector2(0, 0)
            );*/
        }

        /// <inheritdoc />
        public override void Draw(SpriteBatch sb)
        {
            if (Visible)
            {
                
            }
        }
    }
}
