using GameOfLife.Models;

namespace GameOfLife.Interfaces
{
    /// <summary>
    /// Interface for game renderers, allowing structured rendering implementations
    /// </summary>
    public interface IRenderer
    {
        void Render(Grid grid);
        (int rows, int columns) GetGridSize();
        void DisplayInvalidInputMessage();
        void DisplayGameControls();
    }
} 