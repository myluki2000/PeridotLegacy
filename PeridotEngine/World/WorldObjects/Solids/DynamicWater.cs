﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Graphics;
using PeridotEngine.UI;

namespace PeridotEngine.World.WorldObjects.Solids
{
    class DynamicWater : ISolid
    {
        public float ParallaxMultiplier { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public int ZIndex { get; set; }

        public int Resolution { get; set; } = 10;

        private const float SPRING_CONSTANT = 0.02f;
        private const float AMPLIFICATION = 2.0f;
        private const float POINT_MASS = 1.0f;
        private const float DAMPING = 0.97f;

        private float displacementX = 0.0f;

        private List<WaterPoint> waterPoints = new List<WaterPoint>();
        private BasicEffect basicEffect = new BasicEffect(Globals.Graphics.GraphicsDevice)
        {
            VertexColorEnabled = true
        };

        public void Initialize()
        {
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

            // TODO: Maybe pass a reference to the level to the draw method?
            if (ScreenHandler.SelectedScreen.GetType() == typeof(LevelScreen))
            {
                basicEffect.View = ((LevelScreen)ScreenHandler.SelectedScreen).Level.Camera.GetMatrix();
            } else if(ScreenHandler.SelectedScreen.GetType() == typeof(EditorScreen))
            {
                basicEffect.View = ((EditorScreen)ScreenHandler.SelectedScreen).Level.Camera.GetMatrix();
            } else
            {
                throw new Exception("Dynamic water can only be rendered in levels.");
            }

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
            }

            displacementX += 0.08f;
        }

        public void Splash(float relativePosition)
        {
            waterPoints[waterPoints.Count / 2].Position += new Vector2(0, -30);
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
