using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeridotEngine.World.Physics;

namespace PeridotEngine.UI.DevConsole.Commands
{
    class PhysGravityEnabledCommand : Command
    {
        /// <inheritdoc />
        public string CommandString => "phys_gravity_enabled";

        /// <inheritdoc />
        public string HelpMessage => "If set to 1 gravity is enabled, if set to zero it is disabled.";

        /// <inheritdoc />
        public void ExecuteCommand(string cmd, DevConsole console)
        {
            string arg = cmd.Replace(CommandString + " ", "");

            if (arg == "1")
            {
                PhysicsHelper.IsGravityEnabled = true;
            } else if (arg == "0")
            {
                PhysicsHelper.IsGravityEnabled = false;
            }
            else
            {
                console.WriteLine("Argument must be of value 1 or 0.");
                return;
            }

            console.WriteLine("Successfully set gravity.");
        }
    }
}
