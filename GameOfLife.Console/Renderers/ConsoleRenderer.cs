using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameOfLife.Core;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;
using GameOfLife.Console.Menu;
using GameOfLife.Console.Menu.Interfaces;

namespace GameOfLife.Console.Renderers
{
    public class ConsoleRenderer : IRenderer
    {
        private readonly IConsoleUI _consoleUI;
        private readonly InputHandler _inputHandler;

        public ConsoleRenderer(IConsoleUI consoleUI, InputHandler inputHandler)
        {
            _consoleUI = consoleUI ?? throw new ArgumentNullException(nameof(consoleUI));
            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
            System.Console.OutputEncoding = Encoding.UTF8;
        }

        public void Clear()
        {
            System.Console.Clear();
        }

        public void RenderGrid(Grid grid)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    char cellChar = grid.GetCell(i, j).IsAlive ? DisplayConstants.ALIVE_CELL : DisplayConstants.DEAD_CELL;
                    System.Console.Write($"{cellChar}{DisplayConstants.CELL_SEPARATOR}");
                }
                System.Console.WriteLine();
            }
        }

        private void RenderGridsRow(IList<Grid> grids)
        {
            if (grids.Count == 0) return;

            int rows = grids[0].Rows;
            // For each row in the grids
            for (int i = 0; i < rows; i++)
            {
                // For each grid
                for (int gridIndex = 0; gridIndex < grids.Count; gridIndex++)
                {
                    var grid = grids[gridIndex];
                    // Render one row of the current grid
                    for (int j = 0; j < grid.Columns; j++)
                    {
                        char cellChar = grid.GetCell(i, j).IsAlive ? DisplayConstants.ALIVE_CELL : DisplayConstants.DEAD_CELL;
                        System.Console.Write($"{cellChar}{DisplayConstants.CELL_SEPARATOR}");
                    }
                    // Add separator between grids
                    if (gridIndex < grids.Count - 1)
                    {
                        System.Console.Write(DisplayConstants.GAME_HORIZONTAL_SEPARATOR);
                    }
                }
                System.Console.WriteLine();
            }
        }

        public void RenderStatus(int iteration, int livingCells)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(string.Format(DisplayConstants.RENDERER_GENERATION, iteration) + " | " + 
                string.Format(DisplayConstants.RENDERER_LIVING_CELLS, livingCells));
            System.Console.WriteLine();
            System.Console.WriteLine(DisplayConstants.RENDERER_CONTROLS_HEADER);
            System.Console.WriteLine(DisplayConstants.RENDERER_CONTROL_SPACE);
            System.Console.WriteLine(DisplayConstants.RENDERER_CONTROL_SAVE);
            System.Console.WriteLine(DisplayConstants.RENDERER_CONTROL_QUIT);
            System.Console.WriteLine(DisplayConstants.RENDERER_CONTROL_MENU);
        }

        public void Render(Grid grid, int iteration, int livingCells)
        {
            Clear();
            RenderGrid(grid);
            RenderStatus(iteration, livingCells);
        }

        /// <summary>
        /// Renders the current state of multiple game grids along with game statistics and controls.
        /// </summary>
        /// <param name="grids">Collection of grids to render</param>
        /// <param name="iteration">Current generation number</param>
        /// <param name="livingCells">Total number of living cells across all games</param>
        /// <param name="currentGridIndex">Starting index of the currently visible grids</param>
        public void Render(IEnumerable<Grid> grids, int iteration, int livingCells, int currentGridIndex)
        {
            Clear();
            var gridsList = grids.ToList();
            RenderGridsRow(gridsList);
            
            System.Console.WriteLine();
            System.Console.WriteLine(string.Format(DisplayConstants.RENDERER_GENERATION_STATUS, 
                iteration, livingCells, DisplayConstants.TOTAL_PARALLEL_GAMES));
            System.Console.WriteLine(string.Format(DisplayConstants.RENDERER_SHOWING_GAMES, 
                currentGridIndex + 1, currentGridIndex + gridsList.Count));
            System.Console.WriteLine();
            System.Console.WriteLine(DisplayConstants.RENDERER_CONTROLS);
            System.Console.WriteLine(DisplayConstants.RENDERER_NAVIGATION_HELP);
        }

        /// <summary>
        /// Prompts the user for grid dimensions and returns the specified size.
        /// </summary>
        /// <returns>A tuple containing the number of rows and columns for the grid</returns>
        public (int rows, int columns) GetGridSize()
        {
            _consoleUI.DisplayMessage(DisplayConstants.GRID_DIMENSIONS_PROMPT);
            return _inputHandler.GetGridDimensions();
        }

        /// <summary>
        /// Displays an error message when invalid input is received.
        /// </summary>
        public void DisplayInvalidInputMessage()
        {
            _consoleUI.DisplayMessage(DisplayConstants.INVALID_INPUT_MESSAGE);
        }
    }
} 