using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;

namespace PeridotEngine.Engine.World.WorldObjects
{
    interface IRenderedObject
    {
        /// <summary>
        ///  If set to true the renderer will do a draw call only containing this object.
        /// </summary>
        bool DisableBatching { get; }
        /// <summary>
        /// Draws the object to the specified SpriteBatch.
        /// </summary>
        /// <param name="sb">The SpriteBatch.</param>
        /// <param name="camera">The camera of the level.</param>
        /// <param name="textureType">The texture of the material that should be drawn.</param>
        void Draw(SpriteBatch sb, Camera camera, Material.TextureType textureType = Material.TextureType.Diffuse);
    }
}
