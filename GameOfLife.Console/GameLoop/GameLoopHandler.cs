using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Console.GameLoop
{
    /// <summary>
    /// Handles the game loop using functional programming concepts:
    /// - Uses function delegates for pure state transformations
    /// - Implements dependency injection for better composition
    /// - Separates pure functions from side effects (rendering/UI)
    /// - Maintains immutable state through function calls
    /// - Minimizes and encapsulates mutable state
    /// </summary>
    public class GameLoopHandler
    {
        // Function delegates for pure operations and side effects:
        private readonly IRenderer _renderer;                        // Side effect: Handles all rendering operations
        private readonly Action _updateAction;                       // Pure function: Transforms game state without side effects
        private readonly Func<Grid> _getGridAction;                 // Pure function: Returns current grid state immutably
        private readonly Func<IEnumerable<Grid>> _getMultipleGridsAction;  // Pure function: Returns multiple grid states
        private readonly Func<int> _getLivingCellsAction;           // Pure function: Calculates current living cells
        private readonly Func<int> _getIterationAction;             // Pure function: Returns current iteration count
        private readonly Action _displayControlsAction;             // Side effect: Handles UI control display

        // Minimized mutable state - kept private and encapsulated
        private readonly bool _isParallel;
        private bool _isRunning;
        private bool _isPaused;
        private CancellationTokenSource? _cancellationTokenSource;
        private int _currentStartIndex;
        private bool _shouldGenerateNext = true;

        /// <summary>
        /// Encapsulated state access following functional principles
        /// Minimizes direct state mutation from outside
        /// </summary>
        public bool IsPaused
        {
            get => _isPaused;
            private set => _isPaused = value;
        }

        /// <summary>
        /// Constructor implementing functional dependency injection:
        /// - Takes pure functions as parameters
        /// - Separates concerns between state and effects
        /// - Allows for better testing and composition
        /// </summary>
        public GameLoopHandler(
            IRenderer renderer,
            Action updateAction,
            Func<Grid> getGridAction,
            Func<int> getLivingCellsAction,
            Func<int> getIterationAction,
            Action displayControlsAction,
            bool isParallel = false,
            Func<IEnumerable<Grid>>? getMultipleGridsAction = null,
            int currentStartIndex = 0)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _updateAction = updateAction ?? throw new ArgumentNullException(nameof(updateAction));
            _getGridAction = getGridAction ?? throw new ArgumentNullException(nameof(getGridAction));
            _getLivingCellsAction = getLivingCellsAction ?? throw new ArgumentNullException(nameof(getLivingCellsAction));
            _getIterationAction = getIterationAction ?? throw new ArgumentNullException(nameof(getIterationAction));
            _displayControlsAction = displayControlsAction ?? throw new ArgumentNullException(nameof(displayControlsAction));
            _isParallel = isParallel;
            _getMultipleGridsAction = getMultipleGridsAction;
            _currentStartIndex = currentStartIndex;
            _isRunning = false;
            _isPaused = false;
        }

        /// <summary>
        /// Starts the game loop using functional async patterns:
        /// - Maintains immutable state until explicit updates
        /// - Uses Task for pure async operations
        /// </summary>
        public void Start()
        {
            if (_isRunning) return;
            
            _isRunning = true;
            _isPaused = false;
            _cancellationTokenSource = new CancellationTokenSource();
            
            Task.Run(GameLoop, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// Pure function to stop the game loop
        /// Follows functional principle of minimal state mutation
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _isPaused = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        /// <summary>
        /// Toggles pause state following functional principles:
        /// - Minimal state mutation
        /// - Side effects isolated to rendering
        /// </summary>
        public void TogglePause()
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                RenderCurrentState();
            }
        }

        /// <summary>
        /// Updates start index and triggers pure rendering function
        /// </summary>
        public void UpdateStartIndex(int newStartIndex)
        {
            _currentStartIndex = newStartIndex;
            RenderCurrentState();
        }

        /// <summary>
        /// Pure function to render current state:
        /// - Gets fresh state through pure functions
        /// - Isolates all rendering side effects
        /// - Handles both single and parallel game states
        /// </summary>
        public void RenderCurrentState()
        {
            _renderer.Clear();
            if (_isParallel && _getMultipleGridsAction != null)
            {
                // Get fresh immutable state
                var grids = _getMultipleGridsAction();
                _renderer.Render(grids, _getIterationAction(), _getLivingCellsAction(), _currentStartIndex);
            }
            else
            {
                // Get fresh immutable state
                var grid = _getGridAction();
                _renderer.RenderGrid(grid);
                _renderer.RenderStatus(_getIterationAction(), _getLivingCellsAction());
            }
        }

        /// <summary>
        /// Pure function to reset generation flag
        /// </summary>
        public void ResetNextGenerationFlag()
        {
            _shouldGenerateNext = false;
        }

        /// <summary>
        /// Main game loop implementing functional patterns:
        /// - Uses async/await for pure asynchronous operations
        /// - Composes multiple pure functions for state updates
        /// - Isolates side effects (rendering) from pure calculations
        /// - Maintains immutability by getting fresh state each iteration
        /// - Handles cancellation in a functional way
        /// </summary>
        private async Task GameLoop()
        {
            _displayControlsAction();
            RenderCurrentState(); // Initial render

            while (!_cancellationTokenSource?.Token.IsCancellationRequested ?? true)
            {
                try
                {
                    // Pure async delay
                    await Task.Delay(DisplayConstants.GAME_UPDATE_INTERVAL_MS, _cancellationTokenSource?.Token ?? CancellationToken.None);
                    
                    // Pure functional update: only perform if not paused
                    if (!IsPaused)
                    {
                        _updateAction();  // Pure state transformation
                        RenderCurrentState();  // Isolated side effect
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
    }
} 