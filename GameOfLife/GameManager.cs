using System;
using System.Threading;
using System.Threading.Tasks;
using GameOfLife.Constants;

namespace GameOfLife
{
    /// <summary>
    /// Orchestrates the game flow and manages interaction between components.
    /// Acts as the main controller for the Game of Life application.
    /// </summary>
    public class GameManager
    {
        private readonly GameEngine _gameEngine;
        private readonly IRenderer _renderer;
        private bool _isRunning;
        private readonly int _updateIntervalMs = DisplayConstants.GAME_UPDATE_INTERVAL_MS;

        /// <summary>
        /// Creates a new game manager with specified grid dimensions and renderer.
        /// </summary>
        public GameManager(int rows, int columns, IRenderer renderer)
        {
            _gameEngine = new GameEngine(rows, columns);
            _renderer = renderer;
        }

        /// <summary>
        /// Main game loop that updates the grid every second and handles user input
        /// </summary>
        public async Task StartGame()
        {
            _isRunning = true;
            _gameEngine.RandomizeInitialState();
            _renderer.DisplayGameControls();

            // Start a background task to handle user input
            var quitTask = Task.Run(() =>
            {
                while (_isRunning)
                {
                    // Check for 'Q' key press to quit the game
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Q)
                        {
                            _isRunning = false;
                        }
                    }
                    Thread.Sleep(DisplayConstants.INPUT_CHECK_INTERVAL_MS);  // reduce CPU usage
                }
            });

            // Main game loop - updates and renders the game state
            while (_isRunning)
            {
                _renderer.Render(_gameEngine.GetCurrentGrid());
                await Task.Delay(_updateIntervalMs);
                _gameEngine.NextGeneration();
            }

            await quitTask; // Wait for quit task to complete
        }

        /// <summary>
        /// Factory method to create a new game instance.
        /// Handles user input for grid dimensions and validates the input.
        /// </summary>
        /// <returns>A configured GameManager instance</returns>
        public static async Task<GameManager> CreateGame(IRenderer renderer = null)
        {
            renderer ??= new ConsoleRenderer();
            var (rows, columns) = renderer.GetGridSize();
            return new GameManager(rows, columns, renderer);
        }
    }
} 