using Microsoft.Xna.Framework;
using PeridotEngine.Resources;
using System.Xml.Linq;

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
        /// <summary>
        /// Reference to the level the entity is in.
        /// </summary>
        Level Level { get; set; }


        /// <summary>
        /// Used when the objects in the level are loaded from xml. Provide an implementation
        /// which loads all properties you need into the object.
        /// </summary>
        /// <param name="xEle">The XML node containing the data</param>
        /// <param name="textures">A dictionary containing textures</param>
        void InitializeFromXML(XElement xEle, LazyLoadingTextureDictionary textures);

        /// <summary>
        /// Usually called once per game update. Used to update object state.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        void Update(GameTime gameTime);
    }
}
