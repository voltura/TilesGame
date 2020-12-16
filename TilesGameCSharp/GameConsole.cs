using System;

namespace TilesGameCSharp
{
    internal struct GameConsole
    {
        public static void ResetColors()
        {
            SetColors(ConsoleColor.White, ConsoleColor.Black);
        }

        public static void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        public static void Write(string text, bool frame = false)
        {
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