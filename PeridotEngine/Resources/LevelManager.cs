using PeridotEngine.World;
using PeridotEngine.World.Entities;
using PeridotEngine.World.WorldObjects;
using System;
using System.Xml.Linq;

namespace PeridotEngine.Resources
{
    static class LevelManager
    {
        public static Level LoadLevel(string path)
        {
            Level level = new Level();

            XElement rootEle = XElement.Load(path);

            LazyLoadingTextureDictionary textures = new LazyLoadingTextureDictionary();
            
            // loop through all world objects, find their type with reflection, create a new instance of that type
            // and let it initialize itself with the provided xml.
            foreach(XElement xEle in rootEle.Element("WorldObjects").Elements())
            {
                Type wObjType = Type.GetType("PeridotEngine.World.WorldObjects." + xEle.Attribute("Type").Value);

                IWorldObject wObj = (IWorldObject)Activator.CreateInstance(wObjType);

                wObj.InitializeFromXML(xEle, textures);

                level.WorldObjects.Add(wObj);
            }

            // do the same for entites
            foreach(XElement xEle in rootEle.Element("Entities").Elements())
            {
                Type entityType = Type.GetType("PeridotEngine.World.Entities." + xEle.Attribute("Type").Value);

                IEntity entity = (IEntity)Activator.CreateInstance(entityType);

                entity.InitializeFromXML(xEle, textures);

                level.Entities.Add(entity);
            }

            return level;
        }
    }
}
