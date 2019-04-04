#nullable enable

using PeridotEngine.World;
using PeridotEngine.World.WorldObjects.Entities;
using System;
using System.Xml.Linq;
using PeridotEngine.World.WorldObjects.Solids;
using System.IO;
using PeridotEngine.World.WorldObjects;

namespace PeridotEngine.Resources
{
    static class LevelManager
    {
        public static Level LoadLevel(string path)
        {
            Level level = new Level();

            XElement rootEle = XElement.Load(path);

            LazyLoadingTextureDictionary textures = new LazyLoadingTextureDictionary(Path.Combine(Path.GetDirectoryName(path), rootEle.Element("TextureDirectory").Value));
            
            // loop through all solids, find their type with reflection, create a new instance of that type
            // and let it initialize itself with the provided xml.
            foreach(XElement xEle in rootEle.Element("Solids").Elements())
            {
                Type solidType = Type.GetType("PeridotEngine.World.WorldObjects.Solids." + xEle.Element("Type").Value);

                ISolid solid = (ISolid)solidType.GetMethod("FromXML").Invoke(null, new object[] { xEle, textures });

                level.Solids.Add(solid);
            }

            // do the same for entites
            foreach(XElement xEle in rootEle.Element("Entities").Elements())
            {
                Type entityType = Type.GetType("PeridotEngine.World.WorldObjects.Entities." + xEle.Element("Type").Value);

                IEntity entity = (IEntity)entityType.GetMethod("FromXML").Invoke(null, new object[] { xEle, textures });

                level.Entities.Add(entity);
            }

            return level;
        }
    }
}
