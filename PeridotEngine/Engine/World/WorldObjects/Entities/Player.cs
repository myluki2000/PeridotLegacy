#nullable enable

using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World.WorldObjects.Entities
{
    abstract class Player : Character
    {
        public override string Name { get; set; } = "Player";
        public override Level? Level { get; set; }

        /// <inheritdoc />
        public override float MaxSpeed { get; set; } = 360.0f;

        /// <inheritdoc />
        public override float Drag { get; set; } = 20.0f;

        public override void Initialize(Level level)
        {
            this.Level = level;
            HasPhysics = true;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            HandleMovement(keyboardState);
        }

        protected abstract void HandleMovement(KeyboardState keyboardState);
    }
}
