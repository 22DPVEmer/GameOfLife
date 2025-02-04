using System;
using System.Threading.Tasks;
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
    /// Orchestrates the game flow and manages interaction between components.
    /// Acts as the main controller for the Game of Life application.
    /// </summary>
    public class GameManager : GameHandlerWithLoopBase, IGameManager
    {
        private readonly GameEngine _gameEngine;
        private int _currentIteration;

        /// <summary>
        /// Creates a new game manager with specified grid dimensions and renderer.
        /// </summary>
        public GameManager(int rows, int columns, IRenderer renderer)
            : this(rows, columns, renderer, new GameStateService())
        {
        }

        /// <summary>
        /// Creates a new game manager with specified dependencies for testing.
        /// </summary>
        /// <param name="rows">Number of rows in the game grid</param>
        /// <param name="columns">Number of columns in the game grid</param>
        /// <param name="renderer">The renderer implementation to use</param>
        /// <param name="gameStateService">The game state service for saving/loading games</param>
        public GameManager(int rows, int columns, IRenderer renderer, IGameStateService gameStateService)
            : base(renderer, gameStateService)
        {
            _gameEngine = new GameEngine(rows, columns);
            _currentIteration = 1;
        }

        /// <summary>
        /// Initializes game handlers including game loop and input handlers.
        /// </summary>
        protected override void InitializeHandlers()
        {
            _gameLoopHandler = new GameLoopHandler(
                _renderer,
                () => {
                    _gameEngine.NextGeneration();
                    _currentIteration++;
                },
                () => _gameEngine.GetCurrentGrid(),
                () => _gameEngine.GetCurrentGrid().CountLivingCells(),
                () => _currentIteration,
                DisplayControls
            );

            _inputHandler = new GameInputHandler(
                _gameStateService,
                SaveGame,
                _ => {
                    _gameEngine.RandomizeInitialState();
                    _currentIteration = 1;
                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, "Game randomized!");
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
                null,
                () => {
                    if (_gameLoopHandler != null)
                    {
                        _gameLoopHandler.TogglePause();
                        var status = _gameLoopHandler.IsPaused ? "paused" : "resumed";
                        System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, $"Game {status}!");
                    }
                }
            );
        }

        /// <summary>
        /// Initializes the game state, randomizing initial state if this is the first iteration.
        /// </summary>
        protected override void InitializeGame()
        {
            if (_currentIteration == 1)
            {
                _gameEngine.RandomizeInitialState();
            }
            DisplayControls();
        }

        /// <summary>
        /// Displays the game controls in the console.
        /// </summary>
        protected override void DisplayControls()
        {
            System.Console.WriteLine(DisplayConstants.SINGLE_GAME_CONTROLS);
        }

        /// <summary>
        /// Saves the current game state to persistent storage.
        /// </summary>
        private void SaveGame()
        {
            try
            {
                var state = new GameState(_gameEngine.GetCurrentGrid(), _currentIteration);
                _gameStateService.SaveGame(state);
                System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.GAME_SAVED_MESSAGE);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        /// <summary>
        /// Loads a game state, updating the current grid and iteration count.
        /// </summary>
        /// <param name="state">The game state to load</param>
        /// <exception cref="ArgumentNullException">Thrown when state is null</exception>
        public void LoadState(GameState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            
            _currentIteration = state.Iteration;
            _gameEngine.SetCurrentGrid(state.ToGrid());
        }

        protected override void DisplayError(Exception ex)
        {
            base.DisplayError(ex);
        }
    }
} 