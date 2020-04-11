#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Resources;

namespace PeridotEngine.Engine.World.WorldObjects
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
        sbyte ZIndex { get; set; }

        /// <summary>
        /// Called upon initialization of the level the object is in.
        /// </summary>
        /// <param name="level">The level the object is in</param>
        void Initialize(Level level);

        /// <summary>
        /// Update method. Called once each game update.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws the object to the specified SpriteBatch.
        /// </summary>
        /// <param name="sb">The SpriteBatch.</param>
        /// <param name="camera">The camera of the level.</param>
        void Draw(SpriteBatch sb, Camera camera);

        /// <summary>
        /// Draws the object's glow map.
        /// </summary>
        /// <param name="sb">The SpriteBatch.</param>
        /// <param name="camera">The camera of the level.</param>
        void DrawGlowMap(SpriteBatch sb, Camera camera);

        /// <summary>
        /// Draws an outline around the object.
        /// </summary>
        /// <param name="sb">The SpriteBatch.</param>
        /// <param name="camera">The camera of the level.</param>
        void DrawOutline(SpriteBatch sb, Color color, Camera camera);

        bool Contains(Point point);

        /// <summary>
        /// Serializes this object to xml.
        /// </summary>
        /// <param name="materialDictionary">Texture dictionary used for this object</param>
        /// <returns>XElement representing the object as xml</returns>
        XElement ToXml(LazyLoadingMaterialDictionary materialDictionary);
    }
}
