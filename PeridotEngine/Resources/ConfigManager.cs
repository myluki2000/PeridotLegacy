#nullable enable

using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace PeridotEngine.Resources
{
    static class ConfigManager
    {
        private const string CONFIG_PATH = "/config/config.xml";

        public static Config CurrentConfig { get; set; }

        static ConfigManager()
        {
            LoadConfig();
        }

        private static void LoadConfig()
        {
            Config config = new Config();

            XElement rootEle = XElement.Load(CONFIG_PATH);


            config.WindowSize = new Point(
                int.Parse(rootEle.Element("WindowWidth").Value),
                int.Parse(rootEle.Element("WindowHeight").Value));

            config.IsDevModeActive = rootEle.Element("DevMode").Value.ToUpper() == "TRUE";

            CurrentConfig = config;
        }

        public class Config
        {
            /// <summary>
            /// True if the developer mode active flag is set to true in the config, false otherwise
            /// </summary>
            public bool IsDevModeActive { get; set; }
            /// <summary>
            /// The size of the game window
            /// </summary>
            public Point WindowSize { get; set; }
        }
    }

}
