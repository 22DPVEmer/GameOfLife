using System;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Console.Menu.Interfaces;

namespace GameOfLife.Console.Menu
{
    public class GameMenu
    {
        private readonly IRenderer _renderer;
        private readonly IGameStateService _gameStateService;
        private readonly MenuRenderer _menuRenderer;
        private readonly InputHandler _inputHandler;
        private readonly IGameLoader _gameLoader;
        private readonly IGameManagerFactory _gameFactory;
        private readonly IConsoleUI _consoleUI;

        public GameMenu(
            IRenderer renderer, 
            IGameStateService gameStateService,
            MenuRenderer menuRenderer,
            InputHandler inputHandler,
            IGameLoader gameLoader,
            IGameManagerFactory gameFactory,
            IConsoleUI consoleUI)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
            _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
            _gameFactory = gameFactory ?? throw new ArgumentNullException(nameof(gameFactory));
            _consoleUI = consoleUI ?? throw new ArgumentNullException(nameof(consoleUI));
        }

        public async Task ShowMenu()
        {
            bool exitProgram = false;
            while (!exitProgram)
            {
                _menuRenderer.DisplayMainMenu();
                exitProgram = await ProcessMenuChoice();
            }
        }

        private async Task<bool> ProcessMenuChoice()
        {
            if (!_inputHandler.TryGetMenuChoice(out char choice))
            {
                _consoleUI.WaitForKeyPress();
                return false;
            }

            return await HandleChoice((MenuOption)choice);
        }

        private async Task<bool> HandleChoice(MenuOption choice)
        {
            try
            {
                switch (choice)
                {
                    case MenuOption.NewGame:
                        var dimensions = _inputHandler.GetGridDimensions();
                        var game = await _gameFactory.CreateNewGame(dimensions.rows, dimensions.columns);
                        await game.StartGame();
                        return false;

                    case MenuOption.LoadGame:
                        await _gameLoader.LoadExistingGame();
                        return false;

                    case MenuOption.NewParallelGame:
                        dimensions = _inputHandler.GetGridDimensions();
                        var parallelGame = await _gameFactory.CreateNewParallelGame(dimensions.rows, dimensions.columns);
                        await parallelGame.StartGame();
                        return false;

                    case MenuOption.LoadParallelGame:
                        await _gameLoader.LoadExistingParallelGame();
                        return false;

                    case MenuOption.Exit:
                        return true;

                    default:
                        _consoleUI.DisplayMessage(DisplayConstants.INVALID_OPTION);
                        _consoleUI.WaitForKeyPress();
                        return false;
                }
            }
            catch (Exception ex)
            {
                _consoleUI.DisplayMessage($"{DisplayConstants.ERROR_PREFIX}{ex.Message}");
                _consoleUI.WaitForKeyPress();
                return false;
            }
        }
    }
} 