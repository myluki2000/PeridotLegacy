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
        /// The size of the particle system's spawn area.
        /// </summary>
        public Point Size { get; set; } = new Point(1, 1);
        /// <summary>
        /// How many particles the system spawns per second.
        /// NOTE: This will at most spawn 60 particles per second (one per game update)
        /// </summary>
        public uint SpawnsPerSecond { get; set; }
        /// <summary>
        /// This array holds all textures which can be applied to the particles.
        /// </summary>
        public Texture2D[] PossibleTextures { get; set; }
        /// <summary>
        /// The lower bound for particle velocity when spawning.
        /// </summary>
        public Point ParticleVelocityLowest { get; set; }
        /// <summary>
        /// The upper bound for particle velocity when spawning.
        /// </summary>
        public Point ParticleVelocityHighest { get; set; }
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

        /// <summary>
        /// Create a new particle system without specific spawn area. Will spawn particles exactly at 
        /// the position of the particle system.
        /// </summary>
        /// <param name="position">The position of the particle system</param>
        public ParticleSystem(Vector2 position)
        {
            this.Position = position;
        }

        /// <summary>
        /// Create a new particle system with a specific spawn area. Particles will spawn randomly inside this area.
        /// </summary>
        /// <param name="position">The upper-left corner of the spawn area</param>
        /// <param name="size">The size of the spawn area</param>
        public ParticleSystem(Vector2 position, Point size)
        {
            this.Position = position;
            this.Size = size;
        }

        public void SpawnParticles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2 randPos;
                randPos.X = random.Next((int)Position.X, Size.X + 1);
                randPos.Y = random.Next((int)Position.Y, Size.Y + 1);

                Vector2 randVelocity;
                randVelocity.X = random.Next(ParticleVelocityLowest.X, ParticleVelocityHighest.X + 1);
                randVelocity.Y = random.Next(ParticleVelocityLowest.Y, ParticleVelocityHighest.Y + 1);
                int randomTextureIndex = random.Next(0, PossibleTextures.Length);
                particles.Add(new Particle(PossibleTextures[randomTextureIndex], Position, randVelocity, ParticleLifeTime, ParticleFadeTime));
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(Particle particle in particles)
            {
                particle.Draw(sb);
            }
        }

        uint intervalCounter = 0;
        public void Update(GameTime gameTime)
        {
            // automatic particle spawning
            if(SpawnsPerSecond > 0)
            {
                uint spawnInterval = 1000 / SpawnsPerSecond;

                intervalCounter += (uint)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(intervalCounter >= spawnInterval)
                {
                    SpawnParticles(1);
                    intervalCounter = 0;
                }
            }

            // update particles
            foreach(Particle particle in particles)
            {
                particle.Update(gameTime);
            }

            // remove particles which aren't alive
            particles.RemoveAll(x => !x.IsAlive);

            // fire ParticlesDespawned event if all particles have despawned
            if(particles.Count == 0)
            {
                ParticlesDespawned(this, null);
            }

        }
    }
}
