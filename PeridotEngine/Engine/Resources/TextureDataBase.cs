using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Resources
{
    public abstract class TextureDataBase
    {
        public Texture2D Texture { get; set; }

        public Rectangle? SourceRect { get; set; }

        public virtual int Width => SourceRect?.Width ?? Texture.Width;

        public virtual int Height => SourceRect?.Height ?? Texture.Height;
    }
}
