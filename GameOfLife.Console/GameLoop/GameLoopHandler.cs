using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Console.GameLoop
{
    public class GameLoopHandler
    {
        private readonly IRenderer _renderer;
        private readonly Action _updateAction;
        private readonly Func<Grid> _getGridAction;
        private readonly Func<IEnumerable<Grid>> _getMultipleGridsAction;
        private readonly Func<int> _getLivingCellsAction;
        private readonly Func<int> _getIterationAction;
        private readonly Action _displayControlsAction;
        private readonly bool _isParallel;
        private bool _isRunning;
        private bool _isPaused;
        private CancellationTokenSource? _cancellationTokenSource;
        private int _currentStartIndex;

        public bool IsPaused => _isPaused;

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

        public void Start()
        {
            if (_isRunning) return;
            
            _isRunning = true;
            _isPaused = false;
            _cancellationTokenSource = new CancellationTokenSource();
            
            Task.Run(GameLoop, _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _isRunning = false;
            _isPaused = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                // When pausing, ensure we render one final time to show the current state
                RenderCurrentState();
            }
        }

        public void UpdateStartIndex(int newStartIndex)
        {
            _currentStartIndex = newStartIndex;
            RenderCurrentState();
        }

        public void RenderCurrentState()
        {
            _renderer.Clear();
            if (_isParallel && _getMultipleGridsAction != null)
            {
                var grids = _getMultipleGridsAction();
                _renderer.Render(grids, _getIterationAction(), _getLivingCellsAction(), _currentStartIndex);
            }
            else
            {
                var grid = _getGridAction();
                _renderer.RenderGrid(grid);
                _renderer.RenderStatus(_getIterationAction(), _getLivingCellsAction());
            }
        }

        private async Task GameLoop()
        {
            _displayControlsAction();
            RenderCurrentState(); // Initial render

            while (_isRunning)
            {
                if (!_isPaused)
                {
                    _updateAction();
                    RenderCurrentState();
                }

                try
                {
                    await Task.Delay(DisplayConstants.GAME_UPDATE_INTERVAL_MS, _cancellationTokenSource?.Token ?? CancellationToken.None);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
    }
} 