using System;
using System.Threading.Tasks;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Models;
using GameOfLife.Core.Services;
using GameOfLife.Console.GameLoop;
using GameOfLife.Console.Input;

namespace GameOfLife.Console.Base
{
    /// <summary>
    /// Base class for game handlers that use a game loop and input handler.
    /// Provides initialization and management of game loop and input handling components.
    /// </summary>
    public abstract class GameHandlerWithLoopBase
    {
        protected readonly IRenderer _renderer;
        protected readonly IGameStateService _gameStateService;
        protected GameLoopHandler _gameLoopHandler;
        protected GameInputHandler _inputHandler;
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the game handler with loop support.
        /// </summary>
        /// <param name="renderer">The renderer implementation to use for display</param>
        /// <param name="gameStateService">The service for managing game state persistence</param>
        /// <exception cref="ArgumentNullException">Thrown when renderer or gameStateService is null</exception>
        protected GameHandlerWithLoopBase(IRenderer renderer, IGameStateService gameStateService)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
            _isInitialized = false;
        }

        /// <summary>
        /// Starts the game, initializing handlers if needed and running the game loop.
        /// </summary>
        /// <returns>Task representing the running game</returns>
        /// <exception cref="InvalidOperationException">Thrown when game loop or input handlers are not properly initialized</exception>
        public async Task StartGame()
        {
            if (!_isInitialized)
            {
                InitializeHandlers();
                InitializeGame();
                _isInitialized = true;
            }

            if (_gameLoopHandler == null)
                throw new InvalidOperationException(DisplayConstants.GAME_LOOP_NOT_INITIALIZED);

            if (_inputHandler == null)
                throw new InvalidOperationException(DisplayConstants.INPUT_HANDLER_NOT_INITIALIZED);

            _gameLoopHandler.Start();
            var inputTask = Task.Run(() => _inputHandler.Start());
            await inputTask;
        }

        /// <summary>
        /// Stops both the game loop and input handlers.
        /// </summary>
        public void Stop()
        {
            _gameLoopHandler?.Stop();
            _inputHandler?.Stop();
        }

        /// <summary>
        /// Initializes game loop and input handlers. Must be implemented by derived classes.
        /// </summary>
        protected abstract void InitializeHandlers();

        /// <summary>
        /// Initializes the game state. Must be implemented by derived classes.
        /// </summary>
        protected abstract void InitializeGame();

        /// <summary>
        /// Displays game controls. Must be implemented by derived classes.
        /// </summary>
        protected abstract void DisplayControls();

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="ex">The exception containing the error details</param>
        protected virtual void DisplayError(Exception ex)
        {
            System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.ERROR_PREFIX + ex.Message);
        }
    }
} 