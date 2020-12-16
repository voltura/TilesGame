using System;
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

        public void SelectColor(int color)
        {
            for (int y = 0; y < Map.GetUpperBound(0); y++)
            {
                for (int x = 0; x < Map.GetUpperBound(1); x++)
                {
                    if (Map[y, x].Color == (ConsoleColor)color)
                    {
                        Map[y, x].Selected = true;
                    }
                }
            }
        }

        public void Paint()
        {
            GC.ResetColors();
            GC.Clear();
            GC.Write($"===[Level #{Number}]===\n\n");
            for (int y = 0; y < Map.GetUpperBound(0); y++)
            {
                for (int x = 0; x < Map.GetUpperBound(1); x++)
                {
                    GC.SetColors((Map[y, x].Selected) ? ConsoleColor.Black : Map[y, x].Color, ConsoleColor.Black);
                    GC.Write("██");
                    GC.ResetColors();
                }
                GC.Write("\r\n");
            }
        }
    }
}