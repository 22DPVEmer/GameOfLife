using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Console.GameLoop
{
    /// <summary>
    /// Handles the game loop for parallel games using functional programming concepts:
    /// - Uses function delegates to manage game state transformations
    /// - Separates pure functions (state calculations) from side effects (rendering)
    /// - Maintains immutability by getting fresh state through function calls
    /// - Minimizes shared mutable state
    /// </summary>
    public class ParallelGameLoopHandler
    {
        // Function delegates representing pure operations:
        private readonly IRenderer _renderer;
        private readonly Func<Task> _updateAllGamesAction;           // Pure function: Updates all game states without side effects
        private readonly Func<IEnumerable<Grid>> _getVisibleGridsFunc;    // Pure function: Returns current visible grids without modifying state
        private readonly Func<int> _getTotalLivingCellsFunc;        // Pure function: Calculates total living cells across all grids
        private readonly Func<int> _getMaxIterationFunc;            // Pure function: Gets current iteration count without side effects
        private readonly Func<int> _getTotalGamesFunc;              // Pure function: Returns total number of parallel games
        private readonly Action _displayControlsAction;             // Side effect: Handles UI display of controls
        private bool _isRunning;

        /// <summary>
        /// Constructor using dependency injection through function delegates.
        /// Each delegate represents a pure operation or isolated side effect.
        /// This functional approach allows for better testing and separation of concerns.
        /// </summary>
        public ParallelGameLoopHandler(
            IRenderer renderer,
            Func<Task> updateAllGamesAction,
            Func<IEnumerable<Grid>> getVisibleGridsFunc,
            Func<int> getTotalLivingCellsFunc,
            Func<int> getMaxIterationFunc,
            Func<int> getTotalGamesFunc,
            Action displayControlsAction)
        {
            _renderer = renderer;
            _updateAllGamesAction = updateAllGamesAction;
            _getVisibleGridsFunc = getVisibleGridsFunc;
            _getTotalLivingCellsFunc = getTotalLivingCellsFunc;
            _getMaxIterationFunc = getMaxIterationFunc;
            _getTotalGamesFunc = getTotalGamesFunc;
            _displayControlsAction = displayControlsAction;
            _isRunning = true;
        }

        /// <summary>
        /// Pure function to stop the game loop.
        /// Follows functional principle of minimal state mutation.
        /// </summary>
        public void Stop() => _isRunning = false;

        /// <summary>
        /// Main game loop implementing functional patterns:
        /// - Uses async/await for pure asynchronous operations
        /// - Composes multiple pure functions to transform game state
        /// - Isolates side effects (rendering) from pure calculations
        /// - Maintains immutability by getting fresh state each iteration
        /// </summary>
        public async Task Start()
        {
            while (_isRunning)
            {
                // Pure state transformation
                await _updateAllGamesAction();

                // Get fresh immutable state through pure functions
                var visibleGrids = _getVisibleGridsFunc();
                var totalLivingCells = _getTotalLivingCellsFunc();
                var maxIteration = _getMaxIterationFunc();
                var totalGames = _getTotalGamesFunc();

                // Isolated side effects for rendering
                _renderer.Render(visibleGrids, maxIteration, totalLivingCells, totalGames);
                _displayControlsAction();
                System.Console.WriteLine(DisplayConstants.SWITCH_GAMES_PROMPT);

                await Task.Delay(DisplayConstants.GAME_UPDATE_INTERVAL_MS);
            }
        }
    }
} 