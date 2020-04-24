using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.UI;

namespace PeridotEngine.Engine.Graphics.Effects
{
    class GlowEffect : PostProcessingEffect
    {
        private readonly RenderTarget2D glowRt;
        private readonly BlurEffect blurEffect;

        public GlowEffect()
        {
            glowRt = new RenderTarget2D(Globals.Graphics.GraphicsDevice, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight);
            blurEffect = new BlurEffect();
        }

        /// <inheritdoc />
        public override void Apply(SpriteBatch sb, Texture2D scene, Screen screen)
        {
            Globals.Graphics.GraphicsDevice.SetRenderTarget(glowRt);
            Globals.Graphics.GraphicsDevice.Clear(Color.Black);

            screen?.DrawGlowMap(sb);

            Globals.Graphics.GraphicsDevice.SetRenderTarget(null);

            sb.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Immediate);
            sb.Draw(scene, new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight), Color.White);
            sb.End();

            blurEffect.Texture = glowRt;

            sb.Begin(blendState: BlendState.Additive, sortMode: SpriteSortMode.Immediate, effect: blurEffect);
            sb.Draw(glowRt,
                new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth,
                    Globals.Graphics.PreferredBackBufferHeight), Color.White * 0.9f);
            sb.End();
        }
    }
}
