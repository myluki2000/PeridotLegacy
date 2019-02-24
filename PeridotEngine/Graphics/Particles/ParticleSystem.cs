using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PeridotEngine.Graphics.Particles
{
    class ParticleSystem
    {
        /// <summary>
        /// The position of the particle system's center.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// This array holds all textures which can be applied to the particles.
        /// </summary>
        public Texture2D[] PossibleTextures { get; set; }
        /// <summary>
        /// The lower bound for particle velocity when spawning.
        /// </summary>
        public Vector2 ParticleVelocityLowest { get; set; }
        /// <summary>
        /// The upper bound for particle velocity when spawning.
        /// </summary>
        public Vector2 ParticleVelocityHighest { get; set; }
        /// <summary>
        /// Time in milliseconds each particle lives for. Default: 5000ms
        /// </summary>
        public int ParticleLifeTime { get; set; } = 5000;
        /// <summary>
        /// Time in milliseconds each particle fades for (from fully opaque to invisible). Default: 500ms
        /// </summary>
        public int ParticleFadeTime { get; set; } = 500;
        /// <summary>
        /// Raised when all particles of the system have despawned.
        /// </summary>
        public event EventHandler ParticlesDespawned;

        /// <summary>
        /// Contains all currently existing particles emitted from the system.
        /// </summary>
        private readonly List<Particle> particles = new List<Particle>();

        private readonly Random random = new Random();
    }
}
