#nullable enable

using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.World.WorldObjects.Entities;
using PeridotEngine.World.WorldObjects;
using System.Collections.Generic;
using PeridotEngine.World.WorldObjects.Solids;
using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;

namespace PeridotEngine.World
{
    class Level
    {
        /// <summary>
        /// Contains all WorldObjects placed in the level.
        /// </summary>
        public HashSet<ISolid> Solids { get; set; }
        /// <summary>
        /// Contains all entities in the level.
        /// </summary>
        public HashSet<IEntity> Entities { get; set; }

        public string TextureDirectory { get; set; } = "";

        public Camera Camera = new Camera();

        /// <summary>
        /// Create a new empty level.
        /// </summary>
        public Level()
        {
            this.Solids = new HashSet<ISolid>();
            this.Entities = new HashSet<IEntity>();
        }

        /// <summary>
        /// Create a level containing the specified WorldObjects and entities.
        /// </summary>
        /// <param name="solids">The WorldObjects</param>
        /// <param name="entities">The entities</param>
        public Level(HashSet<ISolid> solids, HashSet<IEntity> entities)
        {
            this.Solids = solids;
            this.Entities = entities;
        }

        /// <summary>
        /// First method of the class to be called. Initializes the level.
        /// </summary>
        public void Initialize()
        {
            foreach(ISolid solid in Solids)
                solid.Initialize();
            

            foreach (IEntity entity in Entities)
                entity.Initialize();
        }

        /// <summary>
        /// Draws the level and everything in it to the specified SpriteBatch.
        /// </summary>
        /// <param name="sb">The SpriteBatch</param>
        public void Draw(SpriteBatch sb)
        {
            List<IWorldObject> combinedObjects = new List<IWorldObject>(Solids.Count + Entities.Count);
            combinedObjects.AddRange(Solids);
            combinedObjects.AddRange(Entities);

            combinedObjects.Sort((x, y) => x.ZIndex.CompareTo(y.ZIndex));
            
            sb.Begin(transformMatrix: Camera.GetMatrix());

            foreach(IWorldObject obj in combinedObjects)
            {
                obj.Draw(sb);
            }

            sb.End();

        }

        /// <summary>
        /// Update loop of the level.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {

        }

    }
}
