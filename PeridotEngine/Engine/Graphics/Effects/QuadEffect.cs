using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Graphics.Effects
{
    class QuadEffect : Effect
    {
        private readonly EffectParameter worldViewProjParam;
        private readonly EffectParameter textureParam;
        private readonly EffectParameter glowMapParam;

        public Matrix WorldViewProj
        {
            get => worldViewProjParam.GetValueMatrix();
            set => worldViewProjParam.SetValue(value);
        }

        public Texture2D Texture
        {
            get => textureParam.GetValueTexture2D();
            set => textureParam.SetValue(value);
        }

        public Texture2D GlowMap
        {
            get => glowMapParam.GetValueTexture2D();
            set => glowMapParam.SetValue(value);
        }

        /// <inheritdoc />
        public QuadEffect() : base(Globals.Content.Load<Effect>("QuadEffect"))
        {
            worldViewProjParam = Parameters["WorldViewProj"];
            textureParam = Parameters["Texture"];
            glowMapParam = Parameters["GlowMap"];
        }
    }
}
