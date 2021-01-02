using System;

namespace PeridotEngine
{
#if NETCOREAPP3_1
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new Main())
                game.Run();
        }
    }
#endif
}
