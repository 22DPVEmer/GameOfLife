using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Interface for rendering grid-specific components
    /// </summary>
    public interface IGridRenderer
    {
        /// <summary>
        /// Renders a single grid to the output
        /// </summary>
        void RenderGrid(Grid grid);

        /// <summary>
        /// Renders the game status (iteration and cell count)
        /// </summary>
        void RenderStatus(int iteration, int livingCells);

        /// <summary>
        /// Clears the output
        /// </summary>
        void Clear();
    }
} 