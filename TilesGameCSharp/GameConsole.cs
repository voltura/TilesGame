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

        public static void Write(string text)
        {
            Console.Write(text);
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