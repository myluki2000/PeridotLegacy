using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Graphics.Effects
{
    class FogEffect : Effect
    {

        private readonly EffectParameter matrixParameter;
        private readonly EffectParameter timeParameter;

        public Matrix WorldViewProjection
        {
            get => matrixParameter.GetValueMatrix();
            set => matrixParameter.SetValue(value);
        }

        public float Time
        {
            get => timeParameter.GetValueSingle();
            set => timeParameter.SetValue(value);
        }

        /// <inheritdoc />
        public FogEffect() : base(Globals.Content.Load<Effect>("FogEffect"))
        {
            matrixParameter = Parameters["WorldViewProjection"];
            timeParameter = Parameters["Time"];
        }
    }
}
