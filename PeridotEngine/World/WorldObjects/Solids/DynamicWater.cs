#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.UI;

namespace PeridotEngine.World.WorldObjects.Solids
{
    /// <summary>
    /// A dynamic water object which can be placed in a level.
    /// </summary>
    class DynamicWater : ISolid
    {
        /// <inheritdoc />
        public float ParallaxMultiplier { get; set; }
        /// <inheritdoc />
        public Vector2 Position { get; set; }
        /// <inheritdoc />
        public Vector2 Size { get; set; }
        /// <inheritdoc />
        public int ZIndex { get; set; }

        public int Resolution { get; set; } = 10;

        private const float SPRING_CONSTANT = 0.02f;
        private const float AMPLIFICATION = 2.0f;
        private const float POINT_MASS = 1.0f;
        private const float DAMPING = 0.97f;

        private float displacementX = 0.0f;

        private Level? parentLevel;

        private readonly List<WaterPoint> waterPoints = new List<WaterPoint>();
        private readonly BasicEffect basicEffect = new BasicEffect(Globals.Graphics.GraphicsDevice)
        {
            VertexColorEnabled = true
        };

        public void Initialize(Level level)
        {
            parentLevel = level;

            for(int i = 0; i < (int)(Size.X / Resolution); i++)
            {
                waterPoints.Add(new WaterPoint() { NormalPositionY = Size.Y, Position = new Vector2(i * Resolution, Size.Y) });
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            basicEffect.World = Matrix.Identity;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0.0f,
                                                                        Globals.Graphics.PreferredBackBufferWidth,
                                                                        Globals.Graphics.PreferredBackBufferHeight,
                                                                        0.0f,
                                                                        0.0f,
                                                                        1.0f);


            Debug.Assert(parentLevel != null, nameof(parentLevel) + " != null");
            basicEffect.View = parentLevel.Camera.GetMatrix();

            List<VertexPositionColor> verts = new List<VertexPositionColor>();

            for(int i = waterPoints.Count - 1; i >= 0; i--)
            {
                if(i != 0)
                {
                    // bottom left
                    verts.Add(new VertexPositionColor()
                    {
                        Position = new Vector3(Position.X + waterPoints[i].Position.X - Resolution, Position.Y + Size.Y, 0),
                        Color = Color.LightBlue
                    });

                    // bottom right
                    verts.Add(new VertexPositionColor()
                    {
                        Position = new Vector3(Position.X + waterPoints[i].Position.X, Position.Y + Size.Y, 0),
                        Color = Color.LightBlue
                    });

                    // top right
                    verts.Add(new VertexPositionColor()
                    {
                        Position = new Vector3(Position.X + waterPoints[i].Position.X, Position.Y + Size.Y - waterPoints[i].Position.Y + WaveFunction(i), 0),
                        Color = Color.LightBlue
                    });
                }

                if(i != waterPoints.Count - 1)
                {
                    // top right
                    verts.Add(new VertexPositionColor()
                    {
                        Position = new Vector3(Position.X + waterPoints[i].Position.X, Position.Y + Size.Y - waterPoints[i].Position.Y + WaveFunction(i), 0),
                        Color = Color.LightBlue
                    });

                    // bottom right
                    verts.Add(new VertexPositionColor()
                    {
                        Position = new Vector3(Position.X + waterPoints[i].Position.X, Position.Y + Size.Y, 0),
                        Color = Color.LightBlue
                    });

                    // top right of next one
                    verts.Add(new VertexPositionColor()
                    {
                        Position = new Vector3(Position.X + waterPoints[i + 1].Position.X, Position.Y + Size.Y - waterPoints[i + 1].Position.Y + WaveFunction(i + 1), 0),
                        Color = Color.LightBlue
                    });
                }
            }

            foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                sb.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts.ToArray(), 0, verts.Count, GetIndicesArray(verts), 0, verts.Count / 3);
            }
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < waterPoints.Count; i++)
            {
                // calculate force to base position
                // force to later calculate the acceleration
                float force = 0;
                // delta from actual height to height it should be
                float deltaY = waterPoints[i].Position.Y - waterPoints[i].NormalPositionY;
                // spring equation: F = -k * x
                force += -SPRING_CONSTANT * deltaY;

                // calculate force from neighboring point (left)
                if(i == 0)
                {
                    // edge case left side
                    force += SPRING_CONSTANT * (waterPoints.Last().Position.Y - waterPoints[i].Position.Y);
                } else
                {
                    // standard case
                    force += SPRING_CONSTANT * (waterPoints[i - 1].Position.Y - waterPoints[i].Position.Y);
                }

                // calculate force from neighboring point (right)
                if(i == waterPoints.Count - 1)
                {
                    // edge case right side
                    force += SPRING_CONSTANT * (waterPoints.Last().Position.Y - waterPoints[i].Position.Y);
                } else
                {
                    // standard case
                    force += SPRING_CONSTANT * (waterPoints[i + 1].Position.Y - waterPoints[i].Position.Y);
                }

                // update velocity and position of water point
                float accel = force / POINT_MASS;
                waterPoints[i].VelocityY = DAMPING * waterPoints[i].VelocityY + accel;
                waterPoints[i].Position += new Vector2(0, waterPoints[i].VelocityY);
            }

            displacementX += 0.08f;
        }

        /// <summary>
        /// Creates a splash in the water.
        /// 
        /// Creates a splash at the position specified in world pixels from the left edge of the water.
        /// </summary>
        /// <param name="position">Position in world pixels from the left edge of the water object</param>
        /// <param name="strength">Strength of the splash. A decently sized splash has a value of 20</param>
        public void Splash(float position, float strength)
        {
            // Checks whether the position is within the bounds of the water object
            Debug.Assert(position >= 0 && position <= Size.X);

            waterPoints[(int)Math.Round(position / Resolution)].Position += new Vector2(0, -strength);
        }

        private float WaveFunction(float x)
        {
            float result = 0;
            result += (float)(2 * Math.Sin(((x - displacementX) * 8) / Resolution));
            result += (float)(1 * Math.Sin(((x + displacementX) * 5) / Resolution));
            result += (float)(0.5 * Math.Sin(((x + displacementX) * 15) / Resolution));
            result *= AMPLIFICATION;

            return result;
        }

        private int[] GetIndicesArray(List<VertexPositionColor> verts)
        {
            int[] result = new int[verts.Count];

            for(int i = 0; i < verts.Count; i++)
            {
                result[i] = i;
            }

            return result;
        }

        private class WaterPoint
        {
            public Vector2 Position { get; set; }
            public float NormalPositionY { get; set; }
            public float VelocityY { get; set; }
        }

        
    }
}
