#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.World.WorldObjects
{
    public interface IWorldObject
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
        /// The z-index of the object in the level. 0 (zero) is the "play area".
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// Called upon initialization of the level the object is in.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Update method. Called once each game update.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws the object to the specified SpriteBatch.
        /// </summary>
        /// <param name="sb">The SpriteBatch.</param>
        void Draw(SpriteBatch sb);
    }
}
