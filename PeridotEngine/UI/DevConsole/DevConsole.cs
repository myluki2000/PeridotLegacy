#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Graphics;
using PeridotEngine.Misc;
using PeridotEngine.Resources;

namespace PeridotEngine.UI.DevConsole
{
    class DevConsole
    {
        private bool isVisible = false;
        public bool IsVisible
        {
            get => isVisible;

            set
            {
                isVisible = value;
                if(isVisible)
                {
                    EventInput.CharEntered += HandleKeyInput;
                } else
                {
                    EventInput.CharEntered -= HandleKeyInput;
                }
            }
        }

        private string inputText = "";
        private string outputText = "";

        private readonly Commands.Command[] commands = { new Commands.EditLvlCommand() };

        public void Initialize()
        {

        }

        ushort counter = 0;
        public void Draw(SpriteBatch sb)
        {
            if (IsVisible)
            {
                sb.Begin();

                // output box background
                Utility.DrawRectangle(sb, new Rectangle(0, 0, Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight / 2), new Color(110, 110, 110, 200));

                // input box background
                Utility.DrawRectangle(sb, new Rectangle(0, Globals.Graphics.PreferredBackBufferHeight / 2, Globals.Graphics.PreferredBackBufferWidth, 30), new Color(70, 70, 70, 200));

                string textToDraw = inputText;

                // blinking cursor
                if (counter < 20)
                {
                    textToDraw += "_";
                }
                else if (counter > 40)
                {
                    counter = 0;
                }

                // output box text
                sb.DrawString(FontManager.Fonts.ChakraPetch.Regular, outputText, new Vector2(0, 0), Color.Black);

                // input box text
                sb.DrawString(FontManager.Fonts.ChakraPetch.Regular, textToDraw, new Vector2(0, Globals.Graphics.PreferredBackBufferHeight / 2.0f), Color.Black);

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

        public void WriteLine(string text)
        {
            Write(text + "\n");
        }

        public void Write(string text)
        {
            outputText += text;

            // remove old text if output text is too long
            while (FontManager.Fonts.ChakraPetch.Regular.MeasureString(outputText).Y > Globals.Graphics.PreferredBackBufferHeight / 2)
            {
                outputText = outputText.Remove(0, 1);
            }
        }

        private void HandleKeyInput(object sender, CharacterEventArgs e)
        {
            if (e.Character == '\r')
            {
                InterpretCommand(inputText);
            }
            else if (e.Character == '\b')
            {
                if (inputText.Length > 0)
                {
                    inputText = inputText.Remove(inputText.Length - 1);
                }
            }
            else
            {
                if (FontManager.Fonts.ChakraPetch.Regular.Characters.Contains(e.Character))
                {
                    inputText += e.Character;
                }
            }
        }

        private void InterpretCommand(string cmdString)
        {
            WriteLine("> " + cmdString);

            if (cmdString == "help")
            {
                // print help message
                foreach (Commands.Command cmd in commands)
                {
                    WriteLine(cmd.CommandString + ": " + cmd.HelpMessage);
                }

                return;
            }
            else
            {
                // search fitting command and execute it
                foreach (Commands.Command cmd in commands)
                {
                    if (cmdString.StartsWith(cmd.CommandString))
                    {
                        cmd.ExecuteCommand(cmdString, this);
                        return;
                    }
                }
            }

            // no suitable command found if we reach this point
            WriteLine("This command does not exist.");
        }
    }
}
