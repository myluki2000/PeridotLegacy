namespace PeridotEngine.Engine.UI.DevConsole.Commands
{
    interface Command
    {
        /// <summary>
        /// The command string which one has to type in the dev console to execute this command.
        /// </summary>
        public string CommandString { get; }
        /// <summary>
        /// A help message describing this command.
        /// </summary>
        public string HelpMessage { get; }
        /// <summary>
        /// The method which is executed when the command is typed into the dev console.
        /// </summary>
        /// <param name="cmd">The command string which was typed into the console</param>
        public void ExecuteCommand(string cmd, DevConsole console);
    }
}
