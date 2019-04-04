#nullable enable

using Microsoft.Xna.Framework;
using PeridotEngine.Resources;

namespace PeridotEngine.Graphics.Particles
{
    class Particle : Sprite
    {
        /// <summary>
        /// The current velocity of the particle.
        /// </summary>
        public Vector2 Velocity { get; set; }
        /// <summary>
        /// The max lifetime of the particle.
        /// </summary>
        public int LifeTime { get; set; }
        /// <summary>
        /// True when the particle is alive.
        /// </summary>
        public bool IsAlive { get; set; } = true;
        /// <summary>
        /// The time it takes for the particle to fade (from fully opaque to invisible).
        /// </summary>
        public int FadeTime { get; set; }
        /// <summary>
        /// True if the particle is affected by gravity.
        /// </summary>
        public bool IsAffectedByGravity { get; set; } = true;


        private int lifeTimeCounter = 0;


        public Particle(TextureData texture, Vector2 position, Vector2 velocity, int lifeTime, int fadeTime)
        {
            this.Texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            this.LifeTime = lifeTime;
            this.FadeTime = fadeTime;
        }

        public void Update(GameTime gameTime)
        {
            lifeTimeCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (IsAffectedByGravity)
            {
                Velocity += new Vector2(0, (float)(15 * gameTime.ElapsedGameTime.TotalSeconds));
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(lifeTimeCounter > LifeTime)
            {
                IsAlive = false;
            }

            if(lifeTimeCounter > LifeTime - FadeTime && FadeTime != 0)
            {
                Opacity -= (float)(1 / FadeTime * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }
    }
}
