using System.Collections.Generic;

namespace PeridotEngine.World
{
    class Level
    {
        /// <summary>
        /// Contains all WorldObjects placed in the level.
        /// </summary>
        public List<WorldObject> WorldObjects { get; set; }
        /// <summary>
        /// Contains all entities in the level.
        /// </summary>
        public List<Entity> Entities { get; set; }


        /// <summary>
        /// Create a new empty level.
        /// </summary>
        public Level()
        {
            this.WorldObjects = new List<WorldObject>();
            this.Entities = new List<Entity>();
        }

        /// <summary>
        /// Create a level containing the specified WorldObjects and entities.
        /// </summary>
        /// <param name="worldObjects">The WorldObjects</param>
        /// <param name="entities">The entities</param>
        public Level(List<WorldObject> worldObjects, List<Entity> entities)
        {
            this.WorldObjects = worldObjects;
            this.Entities = entities;
        }
    }
}
