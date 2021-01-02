using System;
using System.Collections.Generic;
using GC = TilesGameCSharp.GameConsole;

namespace TilesGameCSharp
{
    internal class Level
    {
        public int Number { get; set; }
        public Tile[,] Map = new Tile[9, 9];

        public Level(int levelNumber)
        {
            Number = levelNumber;
            Random rnd = new Random();
            int[] levelColors = new int[levelNumber + 1];
            for (int c = 0; c <= levelNumber; c++)
            {
                levelColors[c] = rnd.Next(1, GlobalConstants.MAX_COLORS + 1);
            }

            for (int y = 0; y < Map.GetUpperBound(0); y++)
            {
                for (int x = 0; x < Map.GetUpperBound(1); x++)
                {
                    Map[y, x] = new Tile() { Color = (ConsoleColor)levelColors[rnd.Next(0, levelNumber)] };
                }
            }
        }

        public void Reset()
        {
            for (int y = 0; y < Map.GetUpperBound(0); y++)
            {
                for (int x = 0; x < Map.GetUpperBound(1); x++)
                {
                    Map[y, x].Selected = false;
                }
            }
        }

        public bool Finished()
        {
            for (int y = 0; y < Map.GetUpperBound(0); y++)
            {
                for (int x = 0; x < Map.GetUpperBound(1); x++)
                {
                    if (!Map[y, x].Selected)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool SelectColor(int color)
        {
            bool match = false;
            for (int y = 0; y < Map.GetUpperBound(0); y++)
            {
                for (int x = 0; x < Map.GetUpperBound(1); x++)
                {
                    if (Map[y, x].Color == (ConsoleColor)color)
                    {
                        Map[y, x].Selected = true;
                        match = true;
                    }
                }
            }
            return match;
        }

        public void Paint(string input, bool match, Dictionary<string, int> gameColors)
        {
            GC.Clear();
            GC.Write($" Level {Number:00} ", true);
            GC.Write($"\n\n");
            if (input != string.Empty)
            {
                GC.Write($"You entered ", false, match ? ConsoleColor.White : ConsoleColor.Red);
                if (gameColors.ContainsKey(input))
                {
                    GC.Write($"{input} ", false, (ConsoleColor)gameColors.GetValueOrDefault(input));
                }
                else
                {
                    GC.Write($"{input} ", false, match ? ConsoleColor.White : ConsoleColor.Red);
                }
                if (int.TryParse(input, out int inputInt))
                {
                    ConsoleColor? color = (ConsoleColor)inputInt;
                    if (color != null)
                    {
                        string colorName = $"({Enum.GetName(typeof(ConsoleColor), color).ToUpper()}) ";
                        GC.Write($"{colorName}", false, (ConsoleColor)color);
                    }
                }

                GC.Write($"which was {(match ? "" : "not ")}found!", false, match ? ConsoleColor.White : ConsoleColor.Red);
            }
            GC.Write($"\n\n┌{"".PadLeft(Map.GetUpperBound(1) * 2 + 2, '─')}┐\n");
            for (int y = 0; y < Map.GetUpperBound(0); y++)
            {
                GC.Write("│ ");
                for (int x = 0; x < Map.GetUpperBound(1); x++)
                {
                    GC.Write((Map[y, x].Selected) ? "■■" : "██", false, Map[y, x].Color, ConsoleColor.Black);
                }
                GC.Write(" │\r\n");
            }
            GC.Write($"└{"".PadLeft(Map.GetUpperBound(1) * 2 + 2, '─')}┘");
        }
    }
}