using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.UI;

namespace PeridotEngine.Engine.Graphics.Effects
{
    abstract class PostProcessingEffect
    {
        public abstract void Apply(SpriteBatch sb, Texture2D scene, Screen screen);
    }
}
