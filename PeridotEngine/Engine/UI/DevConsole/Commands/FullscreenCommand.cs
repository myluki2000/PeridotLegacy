namespace PeridotEngine.Engine.UI.DevConsole.Commands
{
    class FullscreenCommand : Command
    {
        /// <inheritdoc />
        public string CommandString => "fullscreen";

        /// <inheritdoc />
        public string HelpMessage => "When 1 the game is set to fullscreen, when 0 the game is set to windowed.";

        /// <inheritdoc />
        public void ExecuteCommand(string cmd, DevConsole console)
        {
            string arg = cmd.Substring(CommandString.Length + 1);

            switch (arg)
            {
                case "1":
                    Globals.Graphics.IsFullScreen = true;
                    break;
                case "0":
                    Globals.Graphics.IsFullScreen = false;
                    break;
                default:
                    console.WriteLine("Argument has to be either 1 or 0.");
                    return;
            }
            Globals.Graphics.ApplyChanges();
        }
    }
}
