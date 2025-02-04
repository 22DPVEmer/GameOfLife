using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Console.GameLoop
{
    public class ParallelGameLoopHandler
    {
        private readonly IRenderer _renderer;
        private readonly Func<Task> _updateAllGamesAction;
        private readonly Func<IEnumerable<Grid>> _getVisibleGridsFunc;
        private readonly Func<int> _getTotalLivingCellsFunc;
        private readonly Func<int> _getMaxIterationFunc;
        private readonly Func<int> _getTotalGamesFunc;
        private readonly Action _displayControlsAction;
        private bool _isRunning;

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

        public void Stop() => _isRunning = false;

        public async Task Start()
        {
            while (_isRunning)
            {
                await _updateAllGamesAction();

                var visibleGrids = _getVisibleGridsFunc();
                var totalLivingCells = _getTotalLivingCellsFunc();
                var maxIteration = _getMaxIterationFunc();
                var totalGames = _getTotalGamesFunc();

                _renderer.Render(visibleGrids, maxIteration, totalLivingCells, totalGames);
                _displayControlsAction();
                System.Console.WriteLine(DisplayConstants.SWITCH_GAMES_PROMPT);

                await Task.Delay(DisplayConstants.GAME_UPDATE_INTERVAL_MS);
            }
        }
    }
} 