using GameOfLife.Core.Models;

namespace GameOfLife.Console.Interfaces
{
    /// <summary>
    /// Interface for game renderers, allowing different rendering implementations
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Renders the current state of the game grid with additional statistics
        /// </summary>
        void Render(Grid grid, int iteration, int livingCells);

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