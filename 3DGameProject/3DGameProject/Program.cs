using System;

namespace _3DGameProject
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MainLoop game = new MainLoop())
            {
                game.Run();
            }
        }
    }
#endif
}

