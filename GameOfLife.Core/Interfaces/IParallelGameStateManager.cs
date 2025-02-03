using GameOfLife.Core.Models;
using System.Collections.Generic;

namespace GameOfLife.Core.Interfaces
{
    public interface IParallelGameStateManager
    {
        void InitializeGames(int numberOfGames, int rows, int columns, GameState initialState = null);
        void InitializeFromSavedState(ParallelGameState savedState);
        IReadOnlyDictionary<int, GameEngine> GetGames();
        IReadOnlyDictionary<int, int> GetIterations();
        void UpdateGameState(int gameId, int iteration);
        int GetTotalLivingCells();
        void SetGameState(int gameId, GameEngine engine, int iteration);
    }
} 