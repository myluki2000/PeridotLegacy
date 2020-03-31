namespace PeridotEngine.Engine.World.WorldObjects.Solids
{
    interface IParallaxable
    {
        /// <summary>
        /// The parallax multiplier of the object in the game world.
        /// </summary>
        float ParallaxMultiplier { get; set; }
    }
}
