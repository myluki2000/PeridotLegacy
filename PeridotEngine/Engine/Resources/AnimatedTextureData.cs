using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.Resources
{
    class AnimatedTextureData : TextureDataBase
    {
        public Frame[] Frames { get; private set; }

        public static AnimatedTextureData FromXml(XElement xEle)
        {
            AnimatedTextureData tex = new AnimatedTextureData()
            {
                Texture = TextureManager.LoadRawTexture(xEle.Element("Path").Value),
                SourceRect = xEle.Element("SourceRect") != null ? (Rectangle?)new Rectangle().FromXml(xEle.Element("SourceRect")) : null,
                Frames = xEle.Element("Animation").Elements("Frame").Select(x => new Frame()
                {
                    Duration = int.Parse(x.Attribute("Duration").Value),
                    Deviation = x.Attribute("Deviation") != null ? int.Parse(x.Attribute("Deviation").Value) : 0
                }).ToArray()
            };

            return tex;
        }

        public struct Frame
        {
            public int Duration { get; set; }
            public int Deviation { get; set; }
        }
    }
}
