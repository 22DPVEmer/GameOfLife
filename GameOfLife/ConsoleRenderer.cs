using System;
using GameOfLife.Models;
using GameOfLife.Constants;

namespace GameOfLife
{
    /// <summary>
    /// Handles all console-based user interface rendering.
    /// Responsible for displaying the game grid, menu, and messages to the user.
    /// </summary>
    public class ConsoleRenderer
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
            Console.WriteLine("\n");
            
            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Console.Write(grid.GetCell(i, j).IsAlive ? DisplayConstants.ALIVE_CELL : DisplayConstants.DEAD_CELL);
                    Console.Write(' '); // Add space for better visibility
                }
                Console.WriteLine();
            }
            
            // Add some space at the bottom
            Console.WriteLine("\n");
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
                Console.WriteLine("Welcome to Conway's Game of Life!");
                Console.WriteLine("--------------------------------");
                Console.WriteLine("Please enter the grid size:");
                Console.Write("Rows (5-30): ");

                if (int.TryParse(Console.ReadLine(), out rows) && rows >= 5 && rows <= 30)
                {
                    Console.Write("Columns (5-30): ");
                    if (int.TryParse(Console.ReadLine(), out columns) && columns >= 5 && columns <= 30)
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
            Console.WriteLine("Invalid input. Please enter a number between 5 and 30.");
            Console.WriteLine("Press any key to try again...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the game controls and instructions.
        /// </summary>
        public void DisplayGameControls()
        {
            Console.WriteLine("\nPress 'Q' to quit the game");
            Console.WriteLine("Game will update automatically every second...");
        }
    }
} 