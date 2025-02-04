using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Handles updates to game states
    /// </summary>
    public interface IGameStateUpdater
    {
        void UpdateGameState(int gameId, int iteration);
        void SetGameState(int gameId, GameEngine engine, int iteration);
    }
} 