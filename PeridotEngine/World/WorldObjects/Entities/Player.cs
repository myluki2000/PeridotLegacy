#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using PeridotEngine.Resources;

namespace PeridotEngine.World.WorldObjects.Entities
{
    class Player : Character
    {
        public override string Name { get; set; }
        public override Level Level { get; set; }

        public override void Initialize() { }

        public static Player FromXML(XElement xEle, LazyLoadingTextureDictionary textures)
        {
            return new Player();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
