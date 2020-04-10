using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Graphics.Effects
{
    class BlurEffect : Effect
    {
        private readonly EffectParameter textureParam;
        private readonly EffectParameter sampleOffsetsParam;
        private readonly EffectParameter sampleWeightsParam;
        private readonly EffectParameter matrixParam;

        public Texture2D Texture
        {
            get => textureParam.GetValueTexture2D();
            set
            {
                textureParam.SetValue(value);

                float texelWidth = 1.0f / value.Width * 1.5f;
                float texelHeight = 1.0f / value.Height * 1.5f;

                sampleOffsetsParam.SetValue(new[]
                {
                    new Vector2(-texelWidth * 2, -texelHeight * 2),
                    new Vector2(-texelWidth, -texelHeight * 2),
                    new Vector2(0, -texelHeight * 2),
                    new Vector2(texelWidth, -texelHeight * 2),
                    new Vector2(texelWidth * 2, -texelHeight * 2),

                    new Vector2(-texelWidth * 2, -texelHeight),
                    new Vector2(-texelWidth, -texelHeight),
                    new Vector2(0, -texelHeight),
                    new Vector2(texelWidth, -texelHeight),
                    new Vector2(texelWidth * 2, -texelHeight),

                    new Vector2(-texelWidth * 2, 0),
                    new Vector2(-texelWidth, 0),
                    new Vector2(0, 0),
                    new Vector2(texelWidth, 0),
                    new Vector2(texelWidth * 2, 0),

                    new Vector2(-texelWidth * 2, texelHeight),
                    new Vector2(-texelWidth, texelHeight),
                    new Vector2(0, texelHeight),
                    new Vector2(texelWidth, texelHeight),
                    new Vector2(texelWidth * 2, texelHeight),

                    new Vector2(-texelWidth * 2, texelHeight * 2),
                    new Vector2(-texelWidth, texelHeight * 2),
                    new Vector2(0, texelHeight * 2),
                    new Vector2(texelWidth, texelHeight * 2),
                    new Vector2(texelWidth * 2, texelHeight * 2),
                });
            }
        }

        /// <inheritdoc />
        public BlurEffect(Effect cloneSource) : base(cloneSource)
        {
            textureParam = Parameters["Texture"];
            matrixParam = Parameters["MatrixTransform"];

            sampleOffsetsParam = Parameters["SampleOffsets"];
            sampleWeightsParam = Parameters["SampleWeights"];

            sampleWeightsParam.SetValue(new[]
            {
                1 / 273.0f, 4 / 273.0f, 7 / 273.0f, 4 / 273.0f, 1 / 273.0f,
                4 / 273.0f, 16 / 273.0f, 26 / 273.0f, 16 / 273.0f, 4 / 273.0f,
                7 / 273.0f, 26 / 273.0f, 41 / 273.0f, 26 / 273.0f, 7 / 273.0f,
                4 / 273.0f, 16 / 273.0f, 26 / 273.0f, 16 / 273.0f, 4 / 273.0f,
                1 / 273.0f, 4 / 273.0f, 7 / 273.0f, 4 / 273.0f, 1 / 273.0f
            });
        }

        /// <inheritdoc />
        protected override void OnApply()
        {
            Viewport vp = GraphicsDevice.Viewport;

            // Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
            // sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
            // --> We get the correct matrix with near plane 0 and far plane -1.
            matrixParam.SetValue(Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0, -1));

        }
    }
}
