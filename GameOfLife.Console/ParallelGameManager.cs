using System;
using System.Linq;
using System.Threading.Tasks;
using GameOfLife.Core;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;
using GameOfLife.Core.Services;

namespace GameOfLife.Console
{
    public class ParallelGameManager : BaseGameManager
    {
        private readonly IParallelGameStateManager _gameStateManager;
        private readonly IVisibleGamesManager _visibleGamesManager;

        public ParallelGameManager(int numberOfGames, int rows, int columns, IRenderer renderer)
            : this(numberOfGames, rows, columns, renderer, null)
        {
        }

        public ParallelGameManager(int numberOfGames, int rows, int columns, IRenderer renderer, GameState initialState)
            : base(renderer)
        {
            if (numberOfGames > DisplayConstants.MAX_PARALLEL_GAMES)
                throw new ArgumentException($"Number of games cannot exceed {DisplayConstants.MAX_PARALLEL_GAMES}");

            _gameStateManager = new ParallelGameStateManager();
            _visibleGamesManager = new VisibleGamesManager();

            _gameStateManager.InitializeGames(numberOfGames, rows, columns, initialState);
            _visibleGamesManager.Initialize(numberOfGames, DisplayConstants.MAX_VISIBLE_GAMES);
        }

        public ParallelGameManager(ParallelGameState savedState, IRenderer renderer)
            : base(renderer)
        {
            if (savedState == null)
                throw new ArgumentNullException(nameof(savedState));

            _gameStateManager = new ParallelGameStateManager();
            _visibleGamesManager = new VisibleGamesManager();

            _gameStateManager.InitializeFromSavedState(savedState);
            _visibleGamesManager.Initialize(savedState.Games.Count, DisplayConstants.MAX_VISIBLE_GAMES);
        }

        public override async Task StartGame()
        {
            _isRunning = true;
            System.Console.WriteLine(DisplayConstants.PARALLEL_GAME_CONTROLS);

            var inputTask = Task.Run(HandleUserInput);
            var updateTask = Task.Run(async () =>
            {
                while (_isRunning)
                {
                    await UpdateAllGames();
                    await Task.Delay(_updateIntervalMs);
                }
            });

            var renderTask = Task.Run(async () =>
            {
                while (_isRunning)
                {
                    RenderVisibleGames();
                    await Task.Delay(_updateIntervalMs);
                }
            });

            await Task.WhenAll(inputTask, updateTask, renderTask);
        }

        private async Task UpdateAllGames()
        {
            var games = _gameStateManager.GetGames();
            var updateTasks = games.Select(async kvp =>
            {
                await Task.Run(() =>
                {
                    kvp.Value.NextGeneration();
                    var iterations = _gameStateManager.GetIterations();
                    _gameStateManager.UpdateGameState(kvp.Key, iterations[kvp.Key] + 1);
                });
            });

            await Task.WhenAll(updateTasks);
        }

        private void RenderVisibleGames()
        {
            lock (_renderLock)
            {
                var games = _gameStateManager.GetGames();
                var visibleIds = _visibleGamesManager.GetVisibleGameIds();
                var visibleGrids = visibleIds.Select(id => games[id].GetCurrentGrid()).ToList();

                var iterations = _gameStateManager.GetIterations();
                int maxIteration = iterations.Values.Max();
                int totalLivingCells = _gameStateManager.GetTotalLivingCells();

                _renderer.Render(visibleGrids, maxIteration, totalLivingCells, games.Count);
                
                System.Console.WriteLine(string.Format(DisplayConstants.SHOWING_GAMES_FORMAT, 
                    string.Join(", ", visibleIds.Select(id => id + 1))));
                System.Console.WriteLine(DisplayConstants.PARALLEL_GAME_CONTROLS);
                System.Console.WriteLine(DisplayConstants.SWITCH_GAMES_PROMPT);
            }
        }

        private async Task HandleUserInput()
        {
            while (_isRunning)
            {
                if (System.Console.KeyAvailable)
                {
                    var key = System.Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            _visibleGamesManager.CycleVisibleGames(-1);
                            break;
                        case ConsoleKey.RightArrow:
                            _visibleGamesManager.CycleVisibleGames(1);
                            break;
                        case ConsoleKey.UpArrow:
                            _visibleGamesManager.CycleVisibleGames(-DisplayConstants.GAMES_PER_PAGE);
                            break;
                        case ConsoleKey.DownArrow:
                            _visibleGamesManager.CycleVisibleGames(DisplayConstants.GAMES_PER_PAGE);
                            break;
                        case ConsoleKey.S:
                            await SaveCurrentGame();
                            break;
                        default:
                            await HandleBasicInput(key);
                            break;
                    }
                }
                await Task.Delay(DisplayConstants.INPUT_CHECK_INTERVAL_MS);
            }
        }

        private async Task SaveCurrentGame()
        {
            try
            {
                var games = _gameStateManager.GetGames();
                var iterations = _gameStateManager.GetIterations();
                var states = games.Select(kvp => new GameState(kvp.Value.GetCurrentGrid(), iterations[kvp.Key])).ToList();
                
                var firstGrid = games[0].GetCurrentGrid();
                _gameStateService.SaveParallelGames(states, firstGrid.Rows, firstGrid.Columns);
                
                await ShowTemporaryMessage(string.Format(DisplayConstants.SAVE_SUCCESS, states.Count));
            }
            catch (Exception ex)
            {
                DisplayError(ex);
                await Task.Delay(1000);
            }
        }
    }
} 