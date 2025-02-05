using System;
using System.Threading.Tasks;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Constants;
using GameOfLife.Console.GameLoop;
using GameOfLife.Console.Input;

namespace GameOfLife.Console.Base
{
    /// <summary>
    /// Base class for game handlers providing core game lifecycle management.
    /// Implements basic game control and resource cleanup.
    /// </summary>
    public abstract class GameHandlerBase : IDisposable
    {
        protected readonly IRenderer _renderer;
        protected readonly IGameStateService _gameStateService;
        protected bool _isRunning;
        protected bool _disposed;

        /// <summary>
        /// Initializes a new instance of the game handler.
        /// </summary>
        /// <param name="renderer">The renderer implementation to use for display</param>
        /// <param name="gameStateService">The service for managing game state persistence</param>
        /// <exception cref="ArgumentNullException">Thrown when renderer or gameStateService is null</exception>
        protected GameHandlerBase(IRenderer renderer, IGameStateService gameStateService)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
        }

        /// <summary>
        /// Starts the game loop, initializing the game and running input and game loop tasks.
        /// </summary>
        /// <returns>Task representing the running game</returns>
        public virtual async Task StartGame()
        {
            _isRunning = true;
            InitializeGame();

            var inputTask = Task.Run(HandleInput);
            var gameLoopTask = Task.Run(GameLoop);

            await Task.WhenAll(inputTask, gameLoopTask);
        }

        /// <summary>
        /// Initializes the game state. Must be implemented by derived classes.
        /// </summary>
        protected abstract void InitializeGame();

        /// <summary>
        /// Handles the main game loop. Must be implemented by derived classes.
        /// </summary>
        protected abstract Task GameLoop();

        /// <summary>
        /// Handles user input. Must be implemented by derived classes.
        /// </summary>
        protected abstract Task HandleInput();

        /// <summary>
        /// Stops the game loop.
        /// </summary>
        protected virtual void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Disposes of resources and stops the game loop.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                _disposed = true;
            }
        }
    }
} 