#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;

namespace PeridotEngine.World.WorldObjects.Entities
{
    class Player : Character
    {
        public override string Name { get; set; } = "Player";
        public override Level? Level { get; set; }

        public override void Initialize(Level level)
        {
            this.Level = level;
        }

        public static Player FromXML(XElement xEle, LazyLoadingTextureDictionary textures)
        {
            return new Player();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
