using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;

namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    /// <summary>
    /// Solids implementing this interface denote that they are textured, thus allowing the editor to automatically set their texture to
    /// the currently selected texture. Note that not implementing this interface doesn't automatically mean that the object is NOT textured.
    /// </summary>
    interface ITextured
    {
        /// <summary>
        /// The texture of the object.
        /// </summary>
        Material Material { get; set; }
    }
}
