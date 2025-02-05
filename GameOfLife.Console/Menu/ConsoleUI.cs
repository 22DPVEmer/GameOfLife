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

            // Handle empty input (cancel)
            if (string.IsNullOrEmpty(input))
            {
                choice = -1;
                return false;
            }

            // Parse and validate the choice
            if (!int.TryParse(input, out choice))
            {
                choice = -1;
                DisplayMessage(DisplayConstants.INVALID_OPTION);
                return false;
            }

            // Validate range (1-based indexing for display)
            if (choice < 1 || choice > saves.Count)
            {
                choice = -1;
                DisplayMessage(DisplayConstants.INVALID_OPTION);
                return false;
            }

            // Convert to 0-based index
            choice--;
            
            // Debug output to verify the conversion
            DisplayMessage($"Debug: Selected save file '{saves[choice]}'");
            
            return true;
        }

        public void WaitForKeyPress()
        {
            System.Console.WriteLine(DisplayConstants.CONTINUE_PROMPT);
            System.Console.ReadKey(true);
        }
    }
} 