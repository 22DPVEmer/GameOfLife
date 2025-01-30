using System;
using GameOfLife.Core.Models;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.Console
{
    /// <summary>
    /// Handles all console-based user interface rendering.
    /// Responsible for displaying the game grid, menu, and messages to the user.
    /// </summary>
    public class ConsoleRenderer : IRenderer
    {
        /// <summary>
        /// Initializes the console renderer and sets up console properties.
        /// </summary>
        public ConsoleRenderer()
        {
            try
            {
                // Set UTF-8 encoding to properly display special characters
                System.Console.OutputEncoding = System.Text.Encoding.UTF8;
                System.Console.CursorVisible = false; // Hide cursor for cleaner display
            }
            catch
            {
                // Ignore any console mode errors
            }
        }

        /// <summary>
        /// Renders the current state of the game grid to the console.
        /// Uses special characters for better visualization, with a fallback to ASCII characters.
        /// </summary>
        public void Render(Grid grid, int iteration, int livingCells)
        {
            System.Console.Clear();
            
            // Render grid
            for (int rowIndex = 0; rowIndex < grid.Rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < grid.Columns; colIndex++)
                {
                    var cell = grid.GetCell(rowIndex, colIndex);
                    System.Console.Write($"{(cell.IsAlive ? DisplayConstants.ALIVE_CELL : DisplayConstants.DEAD_CELL)}");
                    System.Console.Write(DisplayConstants.CELL_SEPARATOR);
                }
                System.Console.WriteLine();
            }
            
            // Display status at the bottom of the grid
            var statusLine = string.Format(DisplayConstants.GAME_CONTROLS, iteration, livingCells);
            System.Console.WriteLine();
            System.Console.WriteLine(statusLine);
        }

        /// <summary>
        /// Displays the menu and handles grid size input.
        /// Returns a tuple containing rows and columns if valid input is provided.
        /// </summary>
        public (int rows, int columns) GetGridSize()
        {
            int rows = 0, columns = 0;
            bool validInput = false;

            while (!validInput)
            {
                System.Console.Clear();
                System.Console.WriteLine(DisplayConstants.WELCOME_TEXT);
                System.Console.WriteLine(DisplayConstants.SEPARATOR_LINE);
                System.Console.WriteLine(DisplayConstants.GRID_SIZE_PROMPT);
                System.Console.Write(DisplayConstants.ROWS_PROMPT);

                if (int.TryParse(System.Console.ReadLine(), out rows) && 
                    rows >= DisplayConstants.MIN_GRID_SIZE && 
                    rows <= DisplayConstants.MAX_GRID_SIZE)
                {
                    System.Console.Write(DisplayConstants.COLUMNS_PROMPT);
                    if (int.TryParse(System.Console.ReadLine(), out columns) && 
                        columns >= DisplayConstants.MIN_GRID_SIZE && 
                        columns <= DisplayConstants.MAX_GRID_SIZE)
                    {
                        validInput = true;
                    }
                }

                if (!validInput)
                {
                    DisplayInvalidInputMessage();
                }
            }

            return (rows, columns);
        }

        /// <summary>
        /// Displays an error message for invalid grid size input.
        /// </summary>
        public void DisplayInvalidInputMessage()
        {
            System.Console.WriteLine(DisplayConstants.INVALID_INPUT_MESSAGE);
            System.Console.WriteLine(DisplayConstants.PRESS_ANY_KEY_MESSAGE);
            System.Console.ReadKey();
        }

        /// <summary>
        /// Displays the game controls and instructions.
        /// </summary>
        public void DisplayGameControls()
        {
            System.Console.WriteLine(DisplayConstants.AUTO_UPDATE_MESSAGE);
        }
    }
} 