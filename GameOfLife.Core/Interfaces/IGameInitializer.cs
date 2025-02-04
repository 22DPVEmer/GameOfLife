using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Handles initialization of game instances
    /// </summary>
    public interface IGameInitializer
    {
        void InitializeGames(int numberOfGames, int rows, int columns, GameState initialState = null);
        void InitializeFromSavedState(ParallelGameState savedState);
    }
} 