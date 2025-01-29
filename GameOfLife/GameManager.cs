using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife
{
    /// <summary>
    /// Orchestrates the game flow and manages interaction between components.
    /// Acts as the main controller for the Game of Life application.
    /// </summary>
    public class GameManager
    {
        private readonly GameEngine _gameEngine;
        private readonly ConsoleRenderer _renderer;
        private bool _isRunning;
        private readonly int _updateIntervalMs = 1000;  // milliseconds

        /// <summary>
        /// Creates a new game manager with specified grid dimensions.
        /// </summary>
        public GameManager(int rows, int columns)
        {
            _gameEngine = new GameEngine(rows, columns);
            _renderer = new ConsoleRenderer();
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
                    Thread.Sleep(100);  // reduce CPU usage
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
        public static async Task<GameManager> CreateGame()
        {
            var renderer = new ConsoleRenderer();
            var (rows, columns) = renderer.GetGridSize();
            return new GameManager(rows, columns);
        }
    }
} 