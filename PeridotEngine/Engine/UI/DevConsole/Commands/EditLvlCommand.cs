using PeridotEngine.Engine.Editor;

namespace PeridotEngine.Engine.UI.DevConsole.Commands
{
    class EditLvlCommand : Command
    {
        public string CommandString => "edit_lvl";

        public string HelpMessage => "Opens a new or existing level in the level editor.\n" +
            "Syntax: edit_lvl <path>";

        public void ExecuteCommand(string cmd, DevConsole console)
        {
            string lvlPath = cmd.Replace("edit_lvl ", "");
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
