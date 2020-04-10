using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Graphics.Effects
{
    class CustomSpriteEffect : Effect
    {
        private readonly EffectParameter matrixParam;

        /// <inheritdoc />
        public CustomSpriteEffect() : base(Globals.Content.Load<Effect>("CustomSpriteEffect"))
        {
            matrixParam = Parameters["MatrixTransform"];
        }

        /// <inheritdoc />
        protected override void OnApply()
        {
            var viewport = GraphicsDevice.Viewport;

            var projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            var halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            matrixParam.SetValue(halfPixelOffset * projection);

            base.OnApply();
        }
    }
}
