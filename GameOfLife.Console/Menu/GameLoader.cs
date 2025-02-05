using System.Threading.Tasks;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;
using System.Linq;
using GameOfLife.Core.Constants;
using GameOfLife.Console.Menu.Interfaces;
using System;

namespace GameOfLife.Console.Menu
{
    /// <summary>
    /// Handles loading saved game states using functional programming patterns:
    /// - Immutable state transformations
    /// - Pure functions for loading and transforming game states
    /// - Separation of concerns between state management and UI
    /// </summary>
    public class GameLoader : IGameLoader
    {
        private readonly IGameStateService _gameStateService;
        private readonly IGameManagerFactory _gameFactory;
        private readonly IConsoleUI _consoleUI;

        /// <summary>
        /// Constructor following dependency injection pattern for better functional composition
        /// </summary>
        public GameLoader(
            IGameStateService gameStateService,
            IGameManagerFactory gameFactory,
            IConsoleUI consoleUI)
        {
            _gameStateService = gameStateService;
            _gameFactory = gameFactory;
            _consoleUI = consoleUI;
        }

        /// <summary>
        /// Loads a single game state using functional composition.
        /// Delegates to LoadGame with specific parameter for single game loading.
        /// </summary>
        public async Task LoadExistingGame()
        {
            await LoadGame(isParallel: false);
        }

        /// <summary>
        /// Loads a parallel game state using functional composition.
        /// Delegates to LoadGame with specific parameter for parallel game loading.
        /// </summary>
        public async Task LoadExistingParallelGame()
        {
            await LoadGame(isParallel: true);
        }

        /// <summary>
        /// Core loading function implementing functional patterns:
        /// - Uses LINQ for functional list operations
        /// - Implements early return pattern to reduce side effects
        /// - Maintains immutability in state transformations
        /// - Separates pure operations from side effects (UI)
        /// </summary>
        private async Task LoadGame(bool isParallel)
        {
            // Pure function: Get immutable list of save files
            var saves = _gameStateService.GetSaveFiles(parallelGames: isParallel);
            
            // Functional pattern: Using LINQ's Any() for existence check
            // Early return pattern reduces nesting and side effects
            if (!saves.Any())
            {
                var message = isParallel ? DisplayConstants.NO_PARALLEL_SAVES : DisplayConstants.NO_SINGLE_SAVES;
                _consoleUI.DisplayMessage(message);
                _consoleUI.WaitForKeyPress();
                return;
            }

            // Side effect: Display available saves
            _consoleUI.DisplaySaveFiles(saves);

            // Pattern matching and immutable state handling
            if (_consoleUI.TryGetSaveChoice(saves, out int choice))
            {
                try
                {
                    // Pure functional transformation: Load and create game state
                    // Each step produces new immutable state
                    GameState state = isParallel 
                        ? _gameStateService.LoadParallelGame(saves[choice])
                        : _gameStateService.LoadGame(saves[choice]);
                    
                    if (state != null)
                    {
                        // Functional composition: Create and start game from state
                        var manager = await _gameFactory.CreateFromState(state, isParallel);
                        await manager.StartGame();
                    }
                }
                catch (Exception ex)
                {
                    // Side effect: Error handling through UI
                    _consoleUI.DisplayMessage($"{DisplayConstants.ERROR_PREFIX}{ex.Message}");
                    _consoleUI.WaitForKeyPress();
                }
            }
        }
    }
} 