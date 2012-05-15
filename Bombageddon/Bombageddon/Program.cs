using System;

namespace Bombageddon
{
#if WINDOWS
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Bombageddon game = new Bombageddon())
            {
                game.Run();
            }
        }
    }
#endif
}

