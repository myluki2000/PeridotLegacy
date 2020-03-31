#nullable enable

namespace PeridotEngine.Engine.World.WorldObjects.Entities
{
    public interface IEntity : IWorldObject
    {
        /// <summary>
        /// Unique name to identify the entity.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Reference to the level the entity is in.
        /// </summary>
        Level? Level { get; set; }
    }
}
