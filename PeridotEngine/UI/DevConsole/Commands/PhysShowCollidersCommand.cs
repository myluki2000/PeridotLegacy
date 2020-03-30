namespace PeridotEngine.UI.DevConsole.Commands
{
    class PhysShowCollidersCommand : Command
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
