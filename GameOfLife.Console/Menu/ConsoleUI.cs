using System;
using System.Collections.Generic;
using GameOfLife.Console.Menu.Interfaces;
using GameOfLife.Core.Constants;

namespace GameOfLife.Console.Menu
{
    /// <summary>
    /// Implementation of console UI operations.
    /// </summary>
    public class ConsoleUI : IConsoleUI
    {
        public void DisplayMessage(string message)
        {
            System.Console.WriteLine(message);
        }

        public void DisplaySaveFiles(IList<string> saves)
        {
            System.Console.WriteLine(DisplayConstants.AVAILABLE_SAVES_HEADER);
            for (int i = 0; i < saves.Count; i++)
            {
                System.Console.WriteLine(string.Format(DisplayConstants.SAVE_FORMAT, i + 1, saves[i]));
            }
        }

        public bool TryGetSaveChoice(IList<string> saves, out int choice)
        {
            System.Console.Write(DisplayConstants.SAVE_SELECTION_PROMPT);
            string? input = System.Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                choice = -1;
                return false;
            }

            if (int.TryParse(input, out choice) && choice >= 1 && choice <= saves.Count)
            {
                choice--; // Convert to 0-based index
                return true;
            }

            choice = -1;
            return false;
        }

        public void WaitForKeyPress()
        {
            System.Console.WriteLine(DisplayConstants.CONTINUE_PROMPT);
            System.Console.ReadKey(true);
        }
    }
} 