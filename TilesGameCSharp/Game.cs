using GC = TilesGameCSharp.GameConsole;

namespace TilesGameCSharp
{
    internal class Game
    {
        private const int MAX_LEVELS = 10;
        private readonly Level[] Levels = new Level[MAX_LEVELS];
        private const int USER_GAVE_UP = -1;
        private const int USER_WANTS_HELP = -2;

        public Game()
        {
            for (int i = 0; i < MAX_LEVELS; i++)
            {
                Levels[i] = new Level(i + 1);
            }
        }

        public void Play()
        {
            ShowIntro();
            int input, numberOfMoves = 0, totalNumberOfMoves = 0, level = 0;
            while (true)
            {
                Levels[level].Paint();
                input = GetUserInput();
                if (input == USER_GAVE_UP)
                {
                    GC.Write($"\nYou gave up after {totalNumberOfMoves + numberOfMoves} moves - game ended!");
                    return;
                }
                if (input == USER_WANTS_HELP)
                {
                    ShowIntro();
                    continue;
                }
                numberOfMoves += 1;
                Levels[level].SelectColor(input);
                if (GameFinished())
                {
                    GC.Write($"\nYou finished the game in {totalNumberOfMoves + numberOfMoves} moves!");
                    return;
                }
                if (Levels[level].Finished())
                {
                    Levels[level].Paint();
                    GC.Write($"\nYou finished level {Levels[level].Number} in {numberOfMoves} moves!");
                    level += 1;
                    totalNumberOfMoves += numberOfMoves;
                    numberOfMoves = 0;
                    if (level >= Levels.Length)
                    {
                        GC.Write("\nGame Over - No more levels!");
                        return;
                    }
                    GC.Write("\nPress any key to advance to the next level!");
                    GC.ReadKey();
                }
            }
        }

        private void ShowIntro()
        {
            GC.ResetColors();
            GC.Clear();
            GC.Write("===[Tiles game]===\nRemember the numbers for each color!\n");
            for (int c = 1; c < 15; c++)
            {
                GC.SetColors(System.ConsoleColor.Black, (System.ConsoleColor)c);
                GC.Write(System.Enum.GetName(typeof(System.ConsoleColor), c).PadRight(20));
                GC.ResetColors();
                GC.Write("= " + c + "\n");
            }
            GC.Write("\nWhen ready press any key when ready to play!\n");
            GC.ReadKey();
            GC.Clear();
        }

        private bool GameFinished()
        {
            foreach (Level level in Levels)
            {
                if (!level.Finished())
                {
                    return false;
                }
            }

            return true;
        }

        private int GetUserInput()
        {
            string input = GC.ReadLine();
            int maxNum = 14;
            GC.Write(@$"
Please enter color number (1-{maxNum}) that is displayed in map above then press [ENTER]
Press H and [ENTER] for help with color numbers.
Press any other key and [ENTER] to quit.\n> ");
            if (input.ToUpper() == "H")
            {
                return USER_WANTS_HELP;
            }
            if (int.TryParse(input, out int num) && num > 0 && num < maxNum + 1)
            {
                return num;
            }

            return USER_GAVE_UP;
        }
    }
}