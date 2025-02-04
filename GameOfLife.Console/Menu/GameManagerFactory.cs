using System;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;
using GameOfLife.Core.Services;
using GameOfLife.Console.Menu.Interfaces;

namespace GameOfLife.Console.Menu
{
    /// <summary>
    /// Factory class for creating both single and parallel Game of Life instances.
    /// Handles creation of game managers with proper dependencies and initialization.
    /// </summary>
    public class GameManagerFactory : IGameManagerFactory
    {
        private readonly IGameStateService _gameStateService;
        private readonly IRenderer _renderer;

        /// <summary>
        /// Initializes a new instance of the GameManagerFactory.
        /// </summary>
        /// <param name="gameStateService">Service for managing game state persistence</param>
        /// <param name="renderer">Renderer implementation for game display</param>
        /// <exception cref="ArgumentNullException">Thrown when gameStateService or renderer is null</exception>
        public GameManagerFactory(IGameStateService gameStateService, IRenderer renderer)
        {
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        /// <summary>
        /// Creates a new single game instance with specified dimensions.
        /// </summary>
        /// <param name="rows">Number of rows in the game grid</param>
        /// <param name="columns">Number of columns in the game grid</param>
        /// <returns>Task containing the new game manager instance</returns>
        public Task<IGameManager> CreateNewGame(int rows, int columns)
        {
            var manager = new GameManager(rows, columns, _renderer, _gameStateService);
            return Task.FromResult<IGameManager>(manager);
        }

        /// <summary>
        /// Creates a new parallel game instance with specified dimensions.
        /// </summary>
        /// <param name="rows">Number of rows in each game grid</param>
        /// <param name="columns">Number of columns in each game grid</param>
        /// <returns>Task containing the new parallel game manager instance</returns>
        public Task<IGameManager> CreateNewParallelGame(int rows, int columns)
        {
            var manager = new ParallelGameManager(rows, columns, _renderer, _gameStateService);
            return Task.FromResult<IGameManager>(manager);
        }

        /// <summary>
        /// Creates a game manager from a saved state.
        /// </summary>
        /// <param name="state">The game state to load</param>
        /// <param name="isParallel">Whether to create a parallel game manager</param>
        /// <returns>Task containing the new game manager instance initialized with the saved state</returns>
        /// <exception cref="ArgumentNullException">Thrown when state is null</exception>
        public Task<IGameManager> CreateFromState(GameState state, bool isParallel)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            IGameManager manager;
            if (isParallel)
            {
                manager = new ParallelGameManager(state.Rows, state.Columns, _renderer, _gameStateService);
            }
            else
            {
                manager = new GameManager(state.Rows, state.Columns, _renderer, _gameStateService);
            }
            
            manager.LoadState(state);
            return Task.FromResult(manager);
        }
    }
} 