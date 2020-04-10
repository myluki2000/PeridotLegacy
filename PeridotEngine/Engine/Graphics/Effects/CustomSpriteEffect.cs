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
        private Viewport lastViewport;
        private Matrix projection;

        public Matrix? TransformMatrix { get; set; }

        /// <inheritdoc />
        public CustomSpriteEffect() : base(Globals.Content.Load<Effect>("CustomSpriteEffect"))
        {
            matrixParam = Parameters["MatrixTransform"];
        }

        /// <inheritdoc />
        protected override void OnApply()
        {
            Viewport vp = GraphicsDevice.Viewport;
            if ((vp.Width != lastViewport.Width) || (vp.Height != lastViewport.Height))
            {
                // Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
                // sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
                // --> We get the correct matrix with near plane 0 and far plane -1.
                Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0, -1, out projection);

            
                projection.M41 += -0.5f * projection.M11;
                projection.M42 += -0.5f * projection.M22;
                

                lastViewport = vp;
            }

            if (TransformMatrix.HasValue)
                matrixParam.SetValue(TransformMatrix.GetValueOrDefault() * projection);
            else
                matrixParam.SetValue(projection);
        }
    }
}
