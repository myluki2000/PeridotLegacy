using Microsoft.Xna.Framework;
using PeridotEngine.Resources;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PeridotEngine.World.WorldObjects
{
    interface IWorldObject
    {
        /// <summary>
        /// The position of the sprite in the current matrix.
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        /// The size of the sprite (width x height). Using texture size if null.
        /// </summary>
        Vector2 Size { get; set; }


        /// <summary>
        /// Update method. Called once each game update.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Used when the objects in the level are loaded from xml. Provide an implementation
        /// which loads all properties you need into the object.
        /// </summary>
        /// <param name="xEle"></param>
        /// <param name="textures">A dictionary containing textures</param>
        /// <returns></returns>
        void InitializeFromXML(XElement xEle, LazyLoadingTextureDictionary textures);
    }
}
