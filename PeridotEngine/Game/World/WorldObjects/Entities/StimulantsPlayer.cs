using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Engine.World.WorldObjects.Entities;

namespace PeridotEngine.Game.World.WorldObjects.Entities
{
    class StimulantsPlayer : Player
    {
        private uint timeStimulantsAmount;

        private KeyboardState lastKeyboardState;

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.D1) && lastKeyboardState.IsKeyUp(Keys.D2))
            {

            }

            lastKeyboardState = keyboardState;
        }
    }
}
