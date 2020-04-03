using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeridotEngine.Engine.UI;
using PeridotEngine.Engine.World;
using PeridotEngine.Game.UI.UIElements;

namespace PeridotEngine.Game.UI
{
    class StandardLevelScreen : LevelScreen
    {
        /// <inheritdoc />
        public StandardLevelScreen(Level level) : base(level)
        {
            UIElements.Add(new StimulantUI());
        }


    }
}
