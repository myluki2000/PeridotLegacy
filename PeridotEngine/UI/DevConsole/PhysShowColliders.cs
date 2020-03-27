using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PeridotEngine.UI.DevConsole.Commands;
using PeridotEngine.World;

namespace PeridotEngine.UI.DevConsole
{
    class PhysShowColliders : Command
    {
        /// <inheritdoc />
        public string CommandString => "phys_show_colliders";

        /// <inheritdoc />
        public string HelpMessage => "When set to 1 outlines will be drawn around colliders.";

        /// <inheritdoc />
        public void ExecuteCommand(string cmd, DevConsole console)
        {
            string arg = cmd.Replace("phys_show_colliders ", "");

            if (ScreenHandler.SelectedScreen is LevelScreen levelScreen)
            {
                if (arg == "1")
                {
                    levelScreen.Level.Settings.DrawColliders = true;
                }
                else if (arg == "0")
                {
                    levelScreen.Level.Settings.DrawColliders = false;
                }
                else
                {
                    console.WriteLine("Argument must be of value 0 or 1");
                    return;
                }
            }
            else
            {
                console.WriteLine("This command can only be executed when currently in a live level (not the editor).");
                return;
            }
        }
    }
}
