using System;

namespace TilesGameCSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Game game = new Game();
            game.ShowIntro();
            game.Play();
        }
    }

    public class Game
    {
        private readonly Level[] Levels = new Level[10];
        private const int USER_GAVE_UP = -1;
        private const int USER_WANTS_HELP = -2;

        public Game()
        {
            for (int i = 0; i < 10; i++)
            {
                Levels[i] = new Level() { Number = i + 1 };
                Levels[i].SetRandomColors(i + 1);
            }
        }

        public void ShowIntro()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===[Tiles game]===\n");
            Console.WriteLine("Remember the numbers for each color!\n");
            for (int c = 1; c < 15; c++)
            {
                Console.BackgroundColor = (ConsoleColor)c;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(Enum.GetName(typeof(ConsoleColor), c).PadRight(20));
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("= {0}\n", c);
            }
            Console.WriteLine("\nWhen ready press any key when ready to play!");
            Console.ReadKey();
            Console.Clear();
        }

        public void Play()
        {
            int input;
            int numberOfMoves = 0;
            int totalNumberOfMoves = 0;
            int level = 0;
            do
            {
                DrawLevel(level);
                Console.Write("\nPlease enter color number (1-14) that exist in map above then press [ENTER]\n" +
                              "Press H and [ENTER] for help with color numbers.\n" +
                              "Press any other key and [ENTER] to quit.\n> ");
                input = GetUserInput(Console.ReadLine(), 14);
                if (input == USER_GAVE_UP)
                {
                    Console.WriteLine("\nYou gave up after {0} moves - game ended!", totalNumberOfMoves + numberOfMoves);
                    return;
                }
                if (input == USER_WANTS_HELP)
                {
                    ShowIntro();
                    continue;
                }
                numberOfMoves += 1;
                SelectColors(input, level);
                if (GameFinished())
                {
                    totalNumberOfMoves += numberOfMoves;
                    Console.WriteLine("\nYou finished the game in " +
                                      totalNumberOfMoves + " moves!");
                    return;
                }
                if (LevelFinished(level))
                {
                    DrawLevel(level);
                    Console.WriteLine("\nYou finished level " + level + 1 +
                                      " in " + numberOfMoves + " moves!");
                    level += 1;
                    totalNumberOfMoves += numberOfMoves;
                    numberOfMoves = 0;
                    if (level > Levels.Length - 1)
                    {
                        Console.WriteLine("\nGame Over - No more levels!");
                        return;
                    }
                    Console.WriteLine("\nPress any key to advance to the next level!");
                    Console.ReadKey();
                }
            }
            while (input < 15);
        }

        private void SelectColors(int selectColor, int activeLevel)
        {
            Level level = Levels[activeLevel];
            for (int y = level.Map.GetLowerBound(0); y < level.Map.GetUpperBound(0); y++)
            {
                for (int x = level.Map.GetLowerBound(1); x < level.Map.GetUpperBound(1); x++)
                {
                    if ((level.Map[y, x].Color == (ConsoleColor)selectColor))
                    {
                        level.Map[y, x].Selected = true;
                    }
                }
            }
        }

        private void DrawLevel(int activeLevel)
        {
            Console.Clear();
            Level level = Levels[activeLevel];
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===[Level #{0}]===\n", level.Number + 1);
            for (int y = level.Map.GetLowerBound(0); y < level.Map.GetUpperBound(0); y++)
            {
                for (int x = level.Map.GetLowerBound(1); x < level.Map.GetUpperBound(1); x++)
                {
                    Console.BackgroundColor = (level.Map[y, x].Selected) ? ConsoleColor.Green : ConsoleColor.Black;
                    Console.ForegroundColor = level.Map[y, x].Color;
                    Console.Write("* ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }

        private bool LevelFinished(int activeLevel)
        {
            Level level = Levels[activeLevel];
            for (int y = level.Map.GetLowerBound(0); y < level.Map.GetUpperBound(0); y++)
            {
                for (int x = level.Map.GetLowerBound(1); x < level.Map.GetUpperBound(1); x++)
                {
                    if (!level.Map[y, x].Selected)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool GameFinished()
        {
            for (int level = 0; level < Levels.Length - 1; level++)
            {
                if (!LevelFinished(level))
                {
                    return false;
                }
            }

            return true;
        }

        private int GetUserInput(string input, int maxNum)
        {
            if (input.ToUpper() == "H")
            {
                return USER_WANTS_HELP;
            }

            if (int.TryParse(input, out int num))
            {
                if (num > 0 && num < (maxNum + 1))
                {
                    return num;
                }
                else
                {
                    return USER_GAVE_UP;
                }
            }
            else
            {
                return USER_GAVE_UP;
            }
        }

        private class Level
        {
            public int Number { get; set; }
            public Tile[,] Map = new Tile[9, 9];

            public void SetRandomColors(int maxNumberOfColors)
            {
                Random rnd = new Random();
                int[] levelColors = new int[maxNumberOfColors + 1];
                for (int c = 0; c <= maxNumberOfColors; c++)
                {
                    levelColors[c] = rnd.Next(1, 15);
                }

                for (int y = 0; y < Map.GetUpperBound(0); y++)
                {
                    for (int x = 0; x < Map.GetUpperBound(1); x++)
                    {
                        Map[y, x] = new Tile() { Color = (ConsoleColor)levelColors[rnd.Next(0, maxNumberOfColors)] };
                    }
                }
            }
        }

        private class Tile
        {
            public ConsoleColor Color { get; set; }
            public bool Selected { get; set; }
        }
    }
}