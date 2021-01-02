using System;
using System.Runtime.InteropServices;

namespace TilesGameCSharp
{
    internal struct GameConsole
    {
        public static void Write(string text, bool frame = false, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            if (frame)
            {
                Console.Write($"╔{"".PadLeft(text.Length, '═')}╗\n║");
            }
            Console.Write(text);
            if (frame)
            {
                Console.Write($"║\n╚{"".PadLeft(text.Length, '═')}╝");
            }
        }

        public static void Clear()
        {
            Console.Title = "Color Tiles Game";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WindowWidth = 60;
                Console.BufferWidth = 60;
                Console.BufferHeight = 30;
                Console.WindowHeight = 30;
                Console.WindowLeft = 0;
                Console.WindowTop = 0;
            }
            Console.Clear();
        }

        public static void ReadKey()
        {
            Console.ReadKey();
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}