using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Services
{
    public class ParallelGameStateManager : IParallelGameStateManager
    {
        private readonly ConcurrentDictionary<int, GameEngine> _games;
        private readonly ConcurrentDictionary<int, int> _iterations;

        public ParallelGameStateManager()
        {
            _games = new ConcurrentDictionary<int, GameEngine>();
            _iterations = new ConcurrentDictionary<int, int>();
        }

        public void InitializeGames(int numberOfGames, int rows, int columns, GameState initialState = null)
        {
            for (int i = 0; i < numberOfGames; i++)
            {
                var gameEngine = new GameEngine(rows, columns);
                if (i == 0 && initialState != null)
                {
                    gameEngine.SetCurrentGrid(initialState.ToGrid());
                    _iterations[i] = initialState.Iteration;
                }
                else
                {
                    gameEngine.RandomizeInitialState();
                    _iterations[i] = 1;
                }
                _games[i] = gameEngine;
            }
        }

        public void InitializeFromSavedState(ParallelGameState savedState)
        {
            if (savedState == null)
                throw new ArgumentNullException(nameof(savedState));

            for (int i = 0; i < savedState.Games.Count; i++)
            {
                var state = savedState.Games[i];
                var gameEngine = new GameEngine(savedState.Rows, savedState.Columns);
                gameEngine.SetCurrentGrid(state.ToGrid());
                _iterations[i] = state.Iteration;
                _games[i] = gameEngine;
            }
        }

        public IReadOnlyDictionary<int, GameEngine> GetGames() => _games;
        public IReadOnlyDictionary<int, int> GetIterations() => _iterations;

        public void UpdateGameState(int gameId, int iteration)
        {
            _iterations.AddOrUpdate(gameId, iteration, (_, _) => iteration);
        }

        public void SetGameState(int gameId, GameEngine engine, int iteration)
        {
            _games[gameId] = engine;
            _iterations[gameId] = iteration;
        }

        public int GetTotalLivingCells()
        {
            return _games.Values.Sum(game =>
            {
                var grid = game.GetCurrentGrid();
                int count = 0;
                for (int row = 0; row < grid.Rows; row++)
                {
                    for (int col = 0; col < grid.Columns; col++)
                    {
                        if (grid.GetCell(row, col).IsAlive)
                        {
                            count++;
                        }
                    }
                }
                return count;
            });
        }
    }
} 