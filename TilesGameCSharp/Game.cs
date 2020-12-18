using System;
using System.Collections.Generic;
using G = TilesGameCSharp.GlobalConstants;
using GC = TilesGameCSharp.GameConsole;

namespace TilesGameCSharp
{
    internal class Game
    {
        private readonly Level[] m_Levels = new Level[G.MAX_LEVELS];
        private readonly Dictionary<string, int> m_Colors;

        public Game()
        {
            m_Colors = SetupColors();
            InitializeLevels();
        }

        private void InitializeLevels()
        {
            for (int i = 0; i < G.MAX_LEVELS; i++)
            {
                m_Levels[i] = new Level(i + 1);
            }
        }

        public void Play()
        {
            ShowIntro();
            int input, numberOfMoves = 0, totalNumberOfMoves = 0, level = 0;
            bool match = false;
            string inputStr = string.Empty;
            while (true)
            {
                m_Levels[level].Paint(inputStr, match);
                input = GetUserInput(out inputStr);
                if (input == G.USER_GAVE_UP)
                {
                    GC.Write($"\nYou gave up after {totalNumberOfMoves + numberOfMoves} moves - game ended!");
                    return;
                }
                if (input == G.USER_WANTS_HELP || input == G.INVALID_INPUT)
                {
                    ShowIntro(input, inputStr);
                    continue;
                }
                numberOfMoves += 1;
                match = m_Levels[level].SelectColor(input);
                if (GameFinished())
                {
                    m_Levels[level].Paint(inputStr, match);
                    GC.Write($"\n\nCongrats!\nYou finished the game in {totalNumberOfMoves + numberOfMoves} moves!", false, ConsoleColor.Green);
                    return;
                }
                if (m_Levels[level].Finished())
                {
                    m_Levels[level].Paint(inputStr, match);
                    GC.Write($"\n\nHurray!\nYou finished the level {m_Levels[level].Number} in {numberOfMoves} moves!\n", false, ConsoleColor.Green);
                    level += 1;
                    totalNumberOfMoves += numberOfMoves;
                    numberOfMoves = 0;
                    inputStr = string.Empty;
                    match = false;
                    GC.Write("\nPress any key to advance to the next level!");
                    GC.ReadKey();
                }
            }
        }

        private void ShowIntro(int invalidInput = 0, string inputStr = "")
        {
            GC.Clear();
            GC.Write(" Color tiles game ", true);
            GC.Write("\n\n");
            if (invalidInput == G.INVALID_INPUT)
            {
                GC.Write(@$"Sorry I did not understand '{inputStr}'!
Please type a valid color name or number.
", false, ConsoleColor.Red);
            }
            else
            {
                GC.Write("Remember the name or number for each color!\n");
            }
            foreach (KeyValuePair<string, int> color in m_Colors)
            {
                GC.Write($"\n{color.Value,2}. {color.Key,-12}");
                GC.Write($"{color.Key,-12}", false, ConsoleColor.Black, (ConsoleColor)color.Value);
            }
            GC.Write("\n\nWhen ready press any key when ready to play!\n");
            GC.ReadKey();
            GC.Clear();
        }

        private bool GameFinished()
        {
            foreach (Level level in m_Levels)
            {
                if (!level.Finished())
                {
                    return false;
                }
            }

            return true;
        }

        private int GetUserInput(out string inputStr)
        {
            GC.Write(@$"

Please enter color 1-{G.MAX_COLORS} or color name displayed and [ENTER]
Press H and [ENTER] for help with colors.
Press Q and [ENTER] to quit.
> ");
            inputStr = GC.ReadLine().ToUpper().Replace(" ","");
            string input = inputStr;
            if (m_Colors.ContainsKey(input))
            {
                input = m_Colors[input].ToString();
            }
            if (input == "H")
            {
                return G.USER_WANTS_HELP;
            }
            if (input == "Q")
            {
                return G.USER_GAVE_UP;
            }
            if (int.TryParse(input, out int num) && num > 0 && num < G.MAX_COLORS + 1)
            {
                return num;
            }

            return G.INVALID_INPUT;
        }

        private static Dictionary<string, int> SetupColors()
        {
            Dictionary<string, int> colors = new Dictionary<string, int>(G.MAX_COLORS);
            for (int c = 1; c <= G.MAX_COLORS; c++)
            {
                colors.Add(Enum.GetName(typeof(ConsoleColor), c).ToUpper(), c);
            }
            return colors;
        }
    }
}