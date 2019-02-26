using Microsoft.Xna.Framework;

namespace PeridotEngine.World.WorldObjects
{
    abstract class FuncObject : IWorldObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public abstract void Update(GameTime gameTime);
    }
}
