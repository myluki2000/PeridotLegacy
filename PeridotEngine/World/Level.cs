using PeridotEngine.World.Entities;
using PeridotEngine.World.WorldObjects;
using System.Collections.Generic;

namespace PeridotEngine.World
{
    class Level
    {
        /// <summary>
        /// Contains all WorldObjects placed in the level.
        /// </summary>
        public HashSet<IWorldObject> WorldObjects { get; set; }
        /// <summary>
        /// Contains all entities in the level.
        /// </summary>
        public HashSet<IEntity> Entities { get; set; }

        /// <summary>
        /// Create a new empty level.
        /// </summary>
        public Level()
        {
            this.WorldObjects = new HashSet<IWorldObject>();
            this.Entities = new HashSet<IEntity>();
        }

        /// <summary>
        /// Create a level containing the specified WorldObjects and entities.
        /// </summary>
        /// <param name="worldObjects">The WorldObjects</param>
        /// <param name="entities">The entities</param>
        public Level(HashSet<IWorldObject> worldObjects, HashSet<IEntity> entities)
        {
            this.WorldObjects = worldObjects;
            this.Entities = entities;
        }


    }
}
