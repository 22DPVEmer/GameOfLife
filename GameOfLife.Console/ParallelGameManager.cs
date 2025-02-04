using System;
using System.Collections.Generic;
using System.Linq;
using GameOfLife.Core;
using GameOfLife.Core.Models;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Services;
using GameOfLife.Core.Constants;
using GameOfLife.Console.GameLoop;
using GameOfLife.Console.Input;
using GameOfLife.Console.Base;
using GameOfLife.Console.Menu.Interfaces;

namespace GameOfLife.Console
{
    /// <summary>
    /// Manages multiple Game of Life instances running in parallel.
    /// Supports running up to 1000 games simultaneously with the ability to view 8 at a time.
    /// </summary>
    public class ParallelGameManager : GameHandlerWithLoopBase, IGameManager
    {
        private readonly List<GameEngine> _allGames;
        private readonly int _rows;
        private readonly int _columns;
        private int _currentIteration;
        private int _currentStartIndex;

        /// <summary>
        /// Creates a new parallel game manager with specified grid dimensions and renderer.
        /// </summary>
        /// <param name="rows">Number of rows in each game grid</param>
        /// <param name="columns">Number of columns in each game grid</param>
        /// <param name="renderer">The renderer implementation to use</param>
        public ParallelGameManager(int rows, int columns, IRenderer renderer)
            : this(rows, columns, renderer, new GameStateService())
        {
        }

        /// <summary>
        /// Creates a new parallel game manager with specified dependencies for testing.
        /// </summary>
        /// <param name="rows">Number of rows in each game grid</param>
        /// <param name="columns">Number of columns in each game grid</param>
        /// <param name="renderer">The renderer implementation to use</param>
        /// <param name="gameStateService">The game state service for saving/loading games</param>
        public ParallelGameManager(int rows, int columns, IRenderer renderer, IGameStateService gameStateService)
            : base(renderer, gameStateService)
        {
            _rows = rows;
            _columns = columns;
            _allGames = new List<GameEngine>();
            _currentIteration = 1;
            _currentStartIndex = 0;
        }

        /// <summary>
        /// Initializes all parallel game instances with random states.
        /// </summary>
        private void InitializeAllGames()
        {
            for (int i = 0; i < DisplayConstants.TOTAL_PARALLEL_GAMES; i++)
            {
                var game = new GameEngine(_rows, _columns);
                game.RandomizeInitialState();
                _allGames.Add(game);
            }
        }

        protected override void InitializeHandlers()
        {
            _gameLoopHandler = new GameLoopHandler(
                _renderer,
                () => {
                    foreach (var game in _allGames)
                    {
                        game.NextGeneration();
                    }
                    _currentIteration++;
                },
                () => _allGames[_currentStartIndex].GetCurrentGrid(),
                () => GetTotalLivingCells(),
                () => _currentIteration,
                DisplayControls,
                true,
                () => GetVisibleGrids(),
                _currentStartIndex
            );

            _inputHandler = new GameInputHandler(
                _gameStateService,
                SaveGame,
                _ => {
                    foreach (var game in _allGames)
                    {
                        game.RandomizeInitialState();
                    }
                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.PARALLEL_GAMES_RANDOMIZED);
                    if (_gameLoopHandler != null)
                    {
                        _gameLoopHandler.RenderCurrentState();
                    }
                },
                () => {
                    Stop();
                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.QUIT_MESSAGE);
                    Environment.Exit(0);
                },
                () => {
                    Stop();
                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.RETURN_TO_MENU_MESSAGE);
                },
                HandleArrowKeys,
                () => {
                    if (_gameLoopHandler != null)
                    {
                        _gameLoopHandler.TogglePause();
                        var message = _gameLoopHandler.IsPaused ? DisplayConstants.PARALLEL_GAMES_PAUSED : DisplayConstants.PARALLEL_GAMES_RESUMED;
                        System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, message);
                    }
                }
            );
        }

        protected override void InitializeGame()
        {
            InitializeAllGames();
            DisplayControls();
        }

        private void HandleArrowKeys(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    _currentStartIndex = (_currentStartIndex - 1 + DisplayConstants.TOTAL_PARALLEL_GAMES) % DisplayConstants.TOTAL_PARALLEL_GAMES;
                    break;
                case ConsoleKey.RightArrow:
                    _currentStartIndex = (_currentStartIndex + 1) % DisplayConstants.TOTAL_PARALLEL_GAMES;
                    break;
                case ConsoleKey.UpArrow:
                    _currentStartIndex = (_currentStartIndex - DisplayConstants.VISIBLE_PARALLEL_GAMES + DisplayConstants.TOTAL_PARALLEL_GAMES) % DisplayConstants.TOTAL_PARALLEL_GAMES;
                    break;
                case ConsoleKey.DownArrow:
                    _currentStartIndex = (_currentStartIndex + DisplayConstants.VISIBLE_PARALLEL_GAMES) % DisplayConstants.TOTAL_PARALLEL_GAMES;
                    break;
            }
            
            if (_gameLoopHandler != null)
            {
                _gameLoopHandler.UpdateStartIndex(_currentStartIndex);
            }
        }

        /// <summary>
        /// Gets the currently visible subset of games for display.
        /// </summary>
        /// <returns>List of grids representing the visible games</returns>
        private IEnumerable<Grid> GetVisibleGrids()
        {
            var visibleGrids = new List<Grid>();
            for (int i = 0; i < DisplayConstants.VISIBLE_PARALLEL_GAMES; i++)
            {
                int index = (_currentStartIndex + i) % DisplayConstants.TOTAL_PARALLEL_GAMES;
                visibleGrids.Add(_allGames[index].GetCurrentGrid());
            }
            return visibleGrids;
        }

        /// <summary>
        /// Calculates the total number of living cells across all parallel games.
        /// </summary>
        /// <returns>Total count of living cells</returns>
        private int GetTotalLivingCells()
        {
            int total = 0;
            foreach (var game in _allGames)
            {
                total += game.GetCurrentGrid().CountLivingCells();
            }
            return total;
        }

        /// <summary>
        /// Saves the state of all parallel games to persistent storage.
        /// </summary>
        private void SaveGame()
        {
            try
            {
                var states = new List<GameState>();
                foreach (var game in _allGames)
                {
                    states.Add(new GameState(game.GetCurrentGrid(), _currentIteration));
                }
                _gameStateService.SaveParallelGame(new ParallelGameState(states, _rows, _columns));
                System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.GAME_SAVED_MESSAGE);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        /// <summary>
        /// Loads a parallel game state, initializing all games from the saved state.
        /// If there are fewer saved games than total slots, fills remaining slots with random games.
        /// </summary>
        /// <param name="state">The parallel game state to load</param>
        /// <exception cref="ArgumentNullException">Thrown when state is null</exception>
        /// <exception cref="ArgumentException">Thrown when state is not a parallel game state</exception>
        public void LoadState(GameState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            
            var parallelState = state as ParallelGameState;
            if (parallelState == null)
            {
                throw new ArgumentException(DisplayConstants.PARALLEL_GAME_STATE_ERROR, nameof(state));
            }
            
            _currentIteration = parallelState.Games[0].Iteration;
            _allGames.Clear();
            
            foreach (var gameState in parallelState.Games)
            {
                var game = new GameEngine(_rows, _columns);
                game.SetCurrentGrid(gameState.ToGrid());
                _allGames.Add(game);
            }
            
            // Fill remaining slots with random games if needed
            while (_allGames.Count < DisplayConstants.TOTAL_PARALLEL_GAMES)
            {
                var game = new GameEngine(_rows, _columns);
                game.RandomizeInitialState();
                _allGames.Add(game);
            }
        }

        protected override void DisplayControls()
        {
            System.Console.WriteLine(string.Format(DisplayConstants.PARALLEL_GAME_STATUS, 
                _currentIteration, GetTotalLivingCells(), DisplayConstants.TOTAL_PARALLEL_GAMES));
            System.Console.WriteLine(string.Format(DisplayConstants.SHOWING_GAMES_FORMAT, 
                _currentStartIndex + 1, _currentStartIndex + DisplayConstants.VISIBLE_PARALLEL_GAMES));
            System.Console.WriteLine();
            System.Console.WriteLine(DisplayConstants.PARALLEL_GAME_CONTROLS);
            System.Console.WriteLine(DisplayConstants.SWITCH_GAMES_PROMPT);
        }

        protected override void DisplayError(Exception ex)
        {
            base.DisplayError(ex);
        }
    }
} 