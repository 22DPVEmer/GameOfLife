using System.Collections.Generic;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Interface for managing game state persistence
    /// </summary>
    public interface IGameStateService
    {
        void SaveGame(GameState state, bool isParallel = false);
        void SaveParallelGame(ParallelGameState state);
        GameState LoadGame(string filename);
        List<string> GetSaveFiles(bool parallelGames = false);
        bool SaveFileExists(bool parallelGames = false);
        GameState LoadMostRecentGame(bool parallelGames = false);
        void SaveParallelGames(IEnumerable<GameState> states, int rows, int columns);
        ParallelGameState LoadParallelGame(string fileName);
    }
} 