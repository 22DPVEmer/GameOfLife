using System.Collections.Generic;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Interface for rendering the game state.
    /// Implementations can render to different outputs (console, GUI, etc.).
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Renders a single game grid with its current state.
        /// </summary>
        void Render(Grid grid, int iteration, int livingCells);

        /// <summary>
        /// Renders multiple game grids with their current states.
        /// </summary>
        void Render(IEnumerable<Grid> visibleGrids, int iteration, int totalLivingCells, int activeGames);

        /// <summary>
        /// Gets grid dimensions from user input.
        /// </summary>
        (int rows, int columns) GetGridSize();

        /// <summary>
        /// Displays an error message for invalid input.
        /// </summary>
        void DisplayInvalidInputMessage();
    }
} 