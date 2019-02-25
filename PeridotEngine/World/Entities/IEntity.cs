using Microsoft.Xna.Framework;

namespace PeridotEngine.World.Entities
{
    interface IEntity
    {
        /// <summary>
        /// Unique name to identify the entity.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The position of the entity in the level.
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        /// The size of the entity.
        /// </summary>
        Vector2 Size { get; set; }
    }
}
