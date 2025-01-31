using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameOfLife.Core;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;
using GameOfLife.Core.Services;

namespace GameOfLife.Console
{
    public class ParallelGameManager
    {
        private readonly IRenderer _renderer;
        private readonly GameStateService _gameStateService;
        private readonly ConcurrentDictionary<int, GameEngine> _games;
        private readonly ConcurrentDictionary<int, int> _iterations;
        private bool _isRunning;
        private readonly int _updateIntervalMs = DisplayConstants.GAME_UPDATE_INTERVAL_MS;
        private readonly List<int> _visibleGameIds;
        private readonly int _maxVisibleGames = DisplayConstants.MAX_VISIBLE_GAMES;
        private readonly object _renderLock = new object();

        public ParallelGameManager(int numberOfGames, int rows, int columns, IRenderer renderer)
            : this(numberOfGames, rows, columns, renderer, null)
        {
        }

        public ParallelGameManager(int numberOfGames, int rows, int columns, IRenderer renderer, GameState initialState)
        {
            if (numberOfGames > DisplayConstants.MAX_PARALLEL_GAMES)
                throw new ArgumentException($"Number of games cannot exceed {DisplayConstants.MAX_PARALLEL_GAMES}");

            _renderer = renderer;
            _gameStateService = new GameStateService();
            _games = new ConcurrentDictionary<int, GameEngine>();
            _iterations = new ConcurrentDictionary<int, int>();
            _visibleGameIds = new List<int>();

            // Initialize games
            for (int i = 0; i < numberOfGames; i++)
            {
                var gameEngine = new GameEngine(rows, columns);
                if (i == 0 && initialState != null)
                {
                    // Use the saved state for the first game
                    gameEngine.SetCurrentGrid(initialState.ToGrid());
                    _iterations[i] = initialState.Iteration;
                }
                else
                {
                    gameEngine.RandomizeInitialState();
                    _iterations[i] = 1;
                }
                _games[i] = gameEngine;

                if (i < _maxVisibleGames)
                {
                    _visibleGameIds.Add(i);
                }
            }
        }

        public async Task StartGames()
        {
            _isRunning = true;
            _renderer.DisplayGameControls();

            // Start input handling task
            var inputTask = Task.Run(HandleUserInput);

            // Start update task
            var updateTask = Task.Run(async () =>
            {
                while (_isRunning)
                {
                    await UpdateAllGames();
                    await Task.Delay(_updateIntervalMs);
                }
            });

            // Start render task
            var renderTask = Task.Run(async () =>
            {
                while (_isRunning)
                {
                    RenderVisibleGames();
                    await Task.Delay(_updateIntervalMs);
                }
            });

            await Task.WhenAll(inputTask, updateTask, renderTask);
        }

        private async Task UpdateAllGames()
        {
            var updateTasks = _games.Select(async kvp =>
            {
                await Task.Run(() =>
                {
                    kvp.Value.NextGeneration();
                    _iterations.AddOrUpdate(kvp.Key, 1, (_, current) => current + 1);
                });
            });

            await Task.WhenAll(updateTasks);
        }

        private void RenderVisibleGames()
        {
            lock (_renderLock)
            {
                var visibleGrids = _visibleGameIds
                    .Select(id => _games[id].GetCurrentGrid())
                    .ToList();

                int totalLivingCells = CountTotalLivingCells();
                int maxIteration = _iterations.Values.Max();

                _renderer.Render(visibleGrids, maxIteration, totalLivingCells, _games.Count);
                
                // Display current game indices
                System.Console.WriteLine(string.Format(DisplayConstants.SHOWING_GAMES_FORMAT, 
                    string.Join(", ", _visibleGameIds.Select(id => id + 1))));
                System.Console.WriteLine(DisplayConstants.SWITCH_GAMES_PROMPT);
            }
        }

        private async Task HandleUserInput()
        {
            while (_isRunning)
            {
                if (System.Console.KeyAvailable)
                {
                    var key = System.Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Q:
                            _isRunning = false;
                            break;
                        case ConsoleKey.LeftArrow:
                            CycleVisibleGames(-1);
                            break;
                        case ConsoleKey.RightArrow:
                            CycleVisibleGames(1);
                            break;
                        case ConsoleKey.UpArrow:
                            CycleVisibleGames(-DisplayConstants.GAMES_PER_PAGE);
                            break;
                        case ConsoleKey.DownArrow:
                            CycleVisibleGames(DisplayConstants.GAMES_PER_PAGE);
                            break;
                        case ConsoleKey.S:
                            SaveCurrentGame();
                            break;
                    }
                }
                await Task.Delay(DisplayConstants.INPUT_CHECK_INTERVAL_MS);
            }
        }

        private void SaveCurrentGame()
        {
            try
            {
                // Save the first visible game's state
                var gameId = _visibleGameIds.First();
                var grid = _games[gameId].GetCurrentGrid();
                var iteration = _iterations[gameId];
                var state = new GameState(grid, iteration);
                _gameStateService.SaveGame(state);
                System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.GameSavedMessage);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.ERROR_PREFIX + ex.Message);
            }
        }

        private void CycleVisibleGames(int direction)
        {
            lock (_renderLock)
            {
                // Calculate the starting point for the new visible set
                int firstVisibleId = _visibleGameIds[0];
                int newFirstId = (firstVisibleId + direction + _games.Count) % _games.Count;
                if (newFirstId < 0) newFirstId += _games.Count;

                // Clear and repopulate visible games
                _visibleGameIds.Clear();
                for (int i = 0; i < _maxVisibleGames; i++)
                {
                    int nextId = (newFirstId + i) % _games.Count;
                    _visibleGameIds.Add(nextId);
                }
            }
        }

        private int CountTotalLivingCells()
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