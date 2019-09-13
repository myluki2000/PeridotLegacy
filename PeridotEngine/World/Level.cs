#nullable enable

using System;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.World.WorldObjects.Entities;
using PeridotEngine.World.WorldObjects;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using PeridotEngine.World.WorldObjects.Solids;
using Microsoft.Xna.Framework;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;

namespace PeridotEngine.World
{
    public class Level
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
                solid.Initialize(this);
            

            foreach (IEntity entity in Entities)
                entity.Initialize(this);
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
            foreach(ISolid obj in Solids)
            {
                obj.Update(gameTime);
            }

            foreach(IEntity obj in Entities)
            {
                obj.Update(gameTime);
            }
        }

        public static Level FromXML(string path)
        {
            Level level = new Level();

            XElement rootEle = XElement.Load(path);

            level.TextureDirectory = Path.Combine(Path.GetDirectoryName(path), rootEle.Element("TextureDirectory").Value);

            LazyLoadingTextureDictionary textures = new LazyLoadingTextureDictionary(level.TextureDirectory);

            // loop through all solids, find their type with reflection, create a new instance of that type
            // and let it initialize itself with the provided xml.
            foreach (XElement xEle in rootEle.Element("Solids").Elements())
            {
                Type solidType = Type.GetType("PeridotEngine.World.WorldObjects.Solids." + xEle.Element("Type").Value);

                ISolid solid = (ISolid)solidType.GetMethod("FromXML").Invoke(null, new object[] { xEle, textures });

                level.Solids.Add(solid);
            }

            // do the same for entites
            foreach (XElement xEle in rootEle.Element("Entities").Elements())
            {
                Type entityType = Type.GetType("PeridotEngine.World.WorldObjects.Entities." + xEle.Element("Type").Value);

                IEntity entity = (IEntity)entityType.GetMethod("FromXML").Invoke(null, new object[] { xEle, textures });

                level.Entities.Add(entity);
            }

            return level;
        }
    }
}
