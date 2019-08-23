#nullable enable

namespace PeridotEngine.World.WorldObjects.Solids
{
    public interface ISolid : IWorldObject
    {
        /// <summary>
        /// The parallax multiplier of the object in the game world.
        /// </summary>
        float ParallaxMultiplier { get; set; }
    }
}
