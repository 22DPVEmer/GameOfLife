using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Manages the core game state and operations
    /// </summary>
    public interface IGameStateManager
    {
        int CurrentIteration { get; }
        void Initialize(int rows, int columns);
        void LoadState(GameState state);
        GameState GetCurrentState();
        void NextGeneration();
        Grid GetCurrentGrid();
        int CountLivingCells();
    }
} 