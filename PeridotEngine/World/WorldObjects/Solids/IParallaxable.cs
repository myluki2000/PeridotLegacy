﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeridotEngine.World.WorldObjects.Solids
{
    interface IParallaxable
    {
        /// <summary>
        /// The parallax multiplier of the object in the game world.
        /// </summary>
        float ParallaxMultiplier { get; set; }
    }
}