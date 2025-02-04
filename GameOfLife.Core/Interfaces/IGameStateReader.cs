using System.Collections.Generic;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Provides read-only access to game states
    /// </summary>
    public interface IGameStateReader
    {
        IReadOnlyDictionary<int, GameEngine> GetGames();
        IReadOnlyDictionary<int, int> GetIterations();
        int GetTotalLivingCells();
    }
} 