using System.Collections.Generic;

namespace GameOfLife.Console.Menu.Interfaces
{
    /// <summary>
    /// Interface for console UI operations.
    /// </summary>
    public interface IConsoleUI
    {
        /// <summary>
        /// Displays a message to the user.
        /// </summary>
        void DisplayMessage(string message);

        /// <summary>
        /// Displays a list of save files.
        /// </summary>
        void DisplaySaveFiles(IList<string> saves);

        /// <summary>
        /// Gets user input for save file selection.
        /// </summary>
        bool TryGetSaveChoice(IList<string> saves, out int choice);

        /// <summary>
        /// Waits for user to press any key.
        /// </summary>
        void WaitForKeyPress();
    }
} 