#nullable enable

using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace PeridotEngine.Engine.Resources
{
    static class ConfigManager
    {
        private const string CONFIG_PATH = "Config/config.xml";

        public static Config CurrentConfig { get; set; }

        static ConfigManager()
        {
            LoadConfig();
        }

        private static void LoadConfig()
        {
            Config config = new Config();

            try
            {
                XElement rootEle = XElement.Load(CONFIG_PATH);

                config.WindowSize = new Point(
                int.Parse(rootEle.Element("WindowWidth").Value),
                int.Parse(rootEle.Element("WindowHeight").Value));

                config.IsDevModeActive = rootEle.Element("DevMode").Value.ToUpper() == "TRUE";

                CurrentConfig = config;
            }
            catch (DirectoryNotFoundException)
            {
                throw new Exception("Could not find config file. Is it missing?");
            }
            catch (XmlException)
            {
                throw new Exception("Error while reading config file. Is it misformed?");
            }
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
