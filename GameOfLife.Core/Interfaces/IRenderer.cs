using System.Collections.Generic;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Interface for game renderers, allowing different rendering implementations
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Renders the current state of a single game grid with additional statistics
        /// </summary>
        void Render(Grid grid, int iteration, int livingCells);

        /// <summary>
        /// Renders multiple game grids with combined statistics
        /// </summary>
        void Render(IEnumerable<Grid> visibleGrids, int iteration, int totalLivingCells, int activeGames);

        /// <summary>
        /// Gets the grid size from user input
        /// </summary>
        (int rows, int columns) GetGridSize();

        /// <summary>
        /// Displays an error message for invalid input
        /// </summary>
        void DisplayInvalidInputMessage();

        /// <summary>
        /// Displays the game controls and instructions
        /// </summary>
        void DisplayGameControls();
    }
} 