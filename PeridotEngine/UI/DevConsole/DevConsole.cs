#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Graphics;
using PeridotEngine.Resources;
using System;
using System.Linq;

namespace PeridotEngine.UI.DevConsole
{
    class DevConsole
    {
        public bool IsVisible { get; set; } = false;

        private string text = "";

        private Commands.Command[] commands = { new Commands.EditLvl()};

        public void Initialize()
        {
            EventInput.CharEntered += HandleKeyInput;
        }

        ushort counter = 0;
        public void Draw(SpriteBatch sb)
        {
            if (IsVisible)
            {
                sb.Begin();

                Utility.DrawRectangle(sb, new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, 30), Color.DimGray * 0.5f);

                string textToDraw = text;

                if (counter < 20)
                {
                    textToDraw += "_";
                }
                else if (counter > 40)
                {
                    counter = 0;
                }

                sb.DrawString(FontManager.Fonts.ChakraPetch.Regular, textToDraw, new Vector2(0, 0), Color.Black);

                sb.End();
                counter++;
            }
        }

        private KeyboardState lastKeyboardState;
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // switch dev console visibility
            if (keyboardState.IsKeyDown(Keys.F5) && lastKeyboardState.IsKeyUp(Keys.F5))
            {
                IsVisible = !IsVisible;
            }

            lastKeyboardState = keyboardState;
        }

        private void HandleKeyInput(object sender, CharacterEventArgs e)
        {
            if (e.Character == '\r')
            {
                InterpretCommand(text);
                IsVisible = false;
            } else if(e.Character == '\b')
            {
                if(text.Length > 0)
                {
                    text = text.Remove(text.Length - 1);
                }
            }
            else
            {
                text += e.Character;
            }
        }

        private void InterpretCommand(string cmdString)
        {
            foreach(Commands.Command cmd in commands)
            {
                if(cmdString.StartsWith(cmd.CommandString))
                {
                    cmd.ExecuteCommand(cmdString);
                    break;
                }
            }
        }
    }
}
