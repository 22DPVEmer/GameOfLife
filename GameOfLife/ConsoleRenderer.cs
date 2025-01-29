using System;
using GameOfLife.Models;
using GameOfLife.Constants;
using GameOfLife.Interfaces;

namespace GameOfLife
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
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.CursorVisible = false; // Hide cursor for cleaner display
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
        public void Render(Grid grid)
        {
            Console.Clear();
            // Add some space at the top
            Console.WriteLine(DisplayConstants.NEW_LINE);
            
            for (int rowIndex = 0; rowIndex < grid.Rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < grid.Columns; colIndex++)
                {
                    Console.Write(grid.GetCell(rowIndex, colIndex).IsAlive ? DisplayConstants.ALIVE_CELL : DisplayConstants.DEAD_CELL);
                    Console.Write(DisplayConstants.CELL_SEPARATOR); // Add space for better visibility
                }
                Console.WriteLine();
            }
            
            // Add some space at the bottom and show quit instruction
            Console.WriteLine(DisplayConstants.NEW_LINE + DisplayConstants.QUIT_INSTRUCTION);
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
                Console.Clear();
                Console.WriteLine(DisplayConstants.WELCOME_TEXT);
                Console.WriteLine(DisplayConstants.SEPARATOR_LINE);
                Console.WriteLine(DisplayConstants.GRID_SIZE_PROMPT);
                Console.Write(DisplayConstants.ROWS_PROMPT);

                if (int.TryParse(Console.ReadLine(), out rows) && 
                    rows >= DisplayConstants.MIN_GRID_SIZE && 
                    rows <= DisplayConstants.MAX_GRID_SIZE)
                {
                    Console.Write(DisplayConstants.COLUMNS_PROMPT);
                    if (int.TryParse(Console.ReadLine(), out columns) && 
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
            Console.WriteLine(DisplayConstants.INVALID_INPUT_MESSAGE);
            Console.WriteLine(DisplayConstants.PRESS_ANY_KEY_MESSAGE);
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the game controls and instructions.
        /// </summary>
        public void DisplayGameControls()
        {
            Console.WriteLine(DisplayConstants.NEW_LINE + DisplayConstants.AUTO_UPDATE_MESSAGE);
        }
    }
} 