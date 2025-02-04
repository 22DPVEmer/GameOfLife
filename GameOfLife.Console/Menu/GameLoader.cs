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
    /// Handles loading saved game states for both single and parallel games.
    /// </summary>
    public class GameLoader : IGameLoader
    {
        private readonly IGameStateService _gameStateService;
        private readonly IGameManagerFactory _gameFactory;
        private readonly IConsoleUI _consoleUI;

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
        /// Loads a saved single game state selected by the user.
        /// </summary>
        public async Task LoadExistingGame()
        {
            await LoadGame(isParallel: false);
        }

        /// <summary>
        /// Loads a saved parallel game state selected by the user.
        /// </summary>
        public async Task LoadExistingParallelGame()
        {
            await LoadGame(isParallel: true);
        }

        private async Task LoadGame(bool isParallel)
        {
            var saves = _gameStateService.GetSaveFiles(parallelGames: isParallel);
            if (!saves.Any())
            {
                var message = isParallel ? DisplayConstants.NO_PARALLEL_SAVES : DisplayConstants.NO_SINGLE_SAVES;
                _consoleUI.DisplayMessage(message);
                _consoleUI.WaitForKeyPress();
                return;
            }

            _consoleUI.DisplaySaveFiles(saves);

            if (_consoleUI.TryGetSaveChoice(saves, out int choice))
            {
                try
                {
                    GameState state;
                    if (isParallel)
                    {
                        state = _gameStateService.LoadParallelGame(saves[choice]);
                    }
                    else
                    {
                        state = _gameStateService.LoadGame(saves[choice]);
                    }
                    
                    if (state != null)
                    {
                        var manager = await _gameFactory.CreateFromState(state, isParallel);
                        await manager.StartGame();
                    }
                }
                catch (Exception ex)
                {
                    _consoleUI.DisplayMessage($"{DisplayConstants.ERROR_PREFIX}{ex.Message}");
                    _consoleUI.WaitForKeyPress();
                }
            }
        }
    }
} 