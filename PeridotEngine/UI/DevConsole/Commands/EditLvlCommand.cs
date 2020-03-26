using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeridotEngine.UI.DevConsole.Commands
{
    class EditLvlCommand : Command
    {
        public string CommandString => "editlvl";

        public string HelpMessage => "Opens a new or existing level in the level editor.\n" +
            "Syntax: editlvl <path>";

        public void ExecuteCommand(string cmd, DevConsole console)
        {
            string lvlPath = cmd.Replace("editlvl ", "");
            if (!lvlPath.EndsWith(".plvl"))
            {
                console.WriteLine("Error: Level files have to end with \".plvl\"");
                return;
            }

            ScreenHandler.SelectedScreen = new EditorScreen(lvlPath);
            console.WriteLine("Loading level...");
        }
    }
}
