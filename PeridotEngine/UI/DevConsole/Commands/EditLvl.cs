using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeridotEngine.UI.DevConsole.Commands
{
    class EditLvl : Command
    {
        public string CommandString => "editlvl";

        public string HelpMessage => "Opens a new or existing level in the level editor.\n" +
            "Syntax: editlvl <path>";

        public void ExecuteCommand(string cmd)
        {
            ScreenHandler.SelectedScreen = new EditorScreen(cmd.Replace("editlvl ", ""));
        }
    }
}
