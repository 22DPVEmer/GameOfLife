using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GameOfLife.Core;
using GameOfLife.Core.Models;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Services;
using GameOfLife.Core.Constants;

namespace GameOfLife.Console
{
    /// <summary>
    /// Orchestrates the game flow and manages interaction between components.
    /// Acts as the main controller for the Game of Life application.
    /// </summary>
    public class GameManager : BaseGameManager
    {
        private readonly GameEngine _gameEngine;
        private int _currentIteration;
        private TaskCompletionSource<bool> _quitTaskSource;

        /// <summary>
        /// Creates a new game manager with specified grid dimensions and renderer.
        /// </summary>
        public GameManager(int rows, int columns, IRenderer renderer)
            : base(renderer)
        {
            _gameEngine = new GameEngine(rows, columns) ?? throw new ArgumentNullException(nameof(_gameEngine));
            _currentIteration = 1;
            _quitTaskSource = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// Creates a new game manager with specified dependencies (for testing).
        /// </summary>
        public GameManager(int rows, int columns, IRenderer renderer, GameStateService gameStateService)
            : base(renderer, gameStateService)
        {
            _gameEngine = new GameEngine(rows, columns) ?? throw new ArgumentNullException(nameof(_gameEngine));
            _currentIteration = 1;
            _quitTaskSource = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// Gets the current game state (for testing).
        /// </summary>
        public GameState GetCurrentState()
        {
            return new GameState(_gameEngine.GetCurrentGrid(), _currentIteration);
        }

        /// <summary>
        /// Saves the current state and quits the game (for testing).
        /// </summary>
        public async Task SaveAndQuit()
        {
            SaveGame();
            await _quitTaskSource.Task;
        }

        /// <summary>
        /// Main game loop that updates the grid every second and handles user input
        /// </summary>
        public override async Task StartGame()
        {
            if (_currentIteration == 1) // Only randomize if it's a new game
            {
                _gameEngine.RandomizeInitialState();
            }
            
            System.Console.WriteLine(DisplayConstants.SINGLE_GAME_CONTROLS);

            // Start a background task to handle user input
            var quitTask = Task.Run(async () =>
            {
                while (true)
                {
                    // Check for key presses
                    if (System.Console.KeyAvailable)
                    {
                        var key = System.Console.ReadKey(true);
                        if (key.Key == ConsoleKey.S)
                        {
                            SaveGame();
                        }
                        else
                        {
                            await HandleBasicInput(key);
                        }
                    }
                    await Task.Delay(DisplayConstants.INPUT_CHECK_INTERVAL_MS);  // reduce CPU usage
                }
                _quitTaskSource.SetResult(true);
            });

            // Main game loop - updates and renders the game state
            while (true)
            {
                var currentGrid = _gameEngine.GetCurrentGrid();
                var livingCells = CountLivingCells(currentGrid);
                _renderer.Render(currentGrid, _currentIteration, livingCells);
                System.Console.WriteLine(DisplayConstants.SINGLE_GAME_CONTROLS);
                await Task.Delay(_updateIntervalMs);
                _gameEngine.NextGeneration();
                _currentIteration++;
            }

            await quitTask; // Wait for quit task to complete
        }

        /// <summary>
        /// Saves the current game state to a file and handles any errors that occur during the process.
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
        /// Displays a menu of available save files and handles the loading of a selected save file.
        /// </summary>
        private void LoadGameMenu()
        {
            var saves = _gameStateService.GetSaveFiles();
            if (!saves.Any())
            {
                System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.NO_SINGLE_SAVES);
                return;
            }

            System.Console.WriteLine(DisplayConstants.AVAILABLE_SAVES_HEADER);
            for (int i = 0; i < saves.Count; i++)
            {
                System.Console.WriteLine(DisplayConstants.SAVE_FORMAT, i + 1, saves[i]);
            }
            System.Console.WriteLine(DisplayConstants.SAVE_SELECTION_PROMPT);

            if (int.TryParse(System.Console.ReadLine(), out int choice) && choice > 0 && choice <= saves.Count)
            {
                LoadGame(saves[choice - 1]);
            }
        }

        /// <summary>
        /// Loads a specific save file and updates the game state accordingly.
        /// Handles any errors that occur during the loading process.
        /// </summary>
        /// <param name="saveFile">The name of the save file to load.</param>
        private void LoadGame(string saveFile)
        {
            try
            {
                var state = _gameStateService.LoadGame(saveFile);
                if (state != null)
                {
                    _currentIteration = state.Iteration;
                    _gameEngine.SetCurrentGrid(state.ToGrid());
                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.GAME_LOADED_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.ERROR_PREFIX + ex.Message);
            }
        }

        /// <summary>
        /// Counts the number of living cells in the current grid.
        /// Used for displaying game statistics.
        /// </summary>
        /// <param name="grid">The grid to count living cells in.</param>
        /// <returns>The total number of living cells in the grid.</returns>
        private int CountLivingCells(Grid grid)
        {
            int count = 0;
            for (int row = 0; row < grid.Rows; row++)
            {
                for (int col = 0; col < grid.Columns; col++)
                {
                    if (grid.GetCell(row, col).IsAlive)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Method to create a new game instance.
        /// Handles user input for grid dimensions and validates the input.
        /// </summary>
        public static async Task<GameManager> CreateGame(IRenderer renderer = null, GameStateService gameStateService = null)
        {
            renderer ??= new ConsoleRenderer();
            gameStateService ??= new GameStateService();
            
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine(DisplayConstants.WELCOME_TEXT);
                System.Console.WriteLine(DisplayConstants.SEPARATOR_LINE);
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_1);
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_2);
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_5);
                System.Console.WriteLine(DisplayConstants.MENU_PROMPT);

                var choice = System.Console.ReadKey(true).KeyChar;
                GameManager manager = null;

                switch ((MenuOption)choice)
                {
                    case MenuOption.NewGame:
                        var (rows, columns) = renderer.GetGridSize();
                        manager = new GameManager(rows, columns, renderer, gameStateService);
                        break;
                    case MenuOption.LoadGame:
                        if (gameStateService.SaveFileExists())
                        {
                            var saves = gameStateService.GetSaveFiles();
                            System.Console.WriteLine(DisplayConstants.AVAILABLE_SAVES_HEADER);
                            for (int i = 0; i < saves.Count; i++)
                            {
                                System.Console.WriteLine(DisplayConstants.SAVE_FORMAT, i + 1, saves[i]);
                            }
                            System.Console.WriteLine(DisplayConstants.SAVE_SELECTION_PROMPT);

                            if (int.TryParse(System.Console.ReadLine(), out int saveChoice) && 
                                saveChoice > 0 && saveChoice <= saves.Count)
                            {
                                var state = gameStateService.LoadGame(saves[saveChoice - 1]);
                                if (state != null)
                                {
                                    manager = new GameManager(state.Rows, state.Columns, renderer, gameStateService);
                                    manager._currentIteration = state.Iteration;
                                    manager._gameEngine.SetCurrentGrid(state.ToGrid());
                                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.GAME_LOADED_MESSAGE);
                                }
                            }
                        }
                        else
                        {
                            System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.NO_SINGLE_SAVES);
                            System.Console.WriteLine(DisplayConstants.CONTINUE_PROMPT);
                            System.Console.ReadKey(true);
                        }
                        break;
                    case MenuOption.Exit:
                        Environment.Exit(0);
                        break;
                }

                if (manager != null)
                {
                    await manager.StartGame();
                    if (!manager._isRunning)
                    {
                        System.Console.WriteLine(DisplayConstants.CONTINUE_PROMPT);
                        System.Console.ReadKey(true);
                    }
                }
            }
        }

        /// <summary>
        /// Loads a game state into the current game manager.
        /// </summary>
        public void LoadState(GameState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _currentIteration = state.Iteration;
            _gameEngine.SetCurrentGrid(state.ToGrid());
        }
    }
} 