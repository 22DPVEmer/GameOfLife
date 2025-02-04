using System.Collections.Generic;
using GameOfLife.Core.Models;
using GameOfLife.Core;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Interface for rendering the game state.
    /// Implementations can render to different outputs (console, GUI, etc.).
    /// </summary>
    public interface IRenderer : IGridRenderer
    {
        /// <summary>
        /// Renders a single game grid with its current state.
        /// </summary>
        void Render(Grid grid, int iteration, int livingCells);

        /// <summary>
        /// Renders multiple game grids with their current states.
        /// </summary>
        void Render(IEnumerable<Grid> grids, int iteration, int livingCells, int currentGridIndex);

        /// <summary>
        /// Gets grid dimensions from user input.
        /// </summary>
        (int rows, int columns) GetGridSize();

        /// <summary>
        /// Displays an error message for invalid input.
        /// </summary>
        void DisplayInvalidInputMessage();

        void Clear();
        new void RenderGrid(Grid grid);
        new void RenderStatus(int iteration, int livingCells);
    }
} 