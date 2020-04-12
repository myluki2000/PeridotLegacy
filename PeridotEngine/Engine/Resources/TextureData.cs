using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.Resources
{
    public class TextureData : TextureDataBase
    {
        public static TextureData FromXml(XElement xEle)
        {
            return new TextureData()
            {
                Texture = TextureManager.LoadRawTexture(xEle.Element("Path").Value),
                SourceRect = xEle.Element("SourceRect") != null ? (Rectangle?)new Rectangle().FromXml(xEle.Element("SourceRect")) : null
            };
        }
    }
}
