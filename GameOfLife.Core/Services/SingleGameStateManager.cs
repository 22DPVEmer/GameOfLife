using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;
using System;

namespace GameOfLife.Core.Services
{
    public class SingleGameStateManager : IGameStateManager
    {
        private GameEngine _gameEngine;
        private int _currentIteration;

        public SingleGameStateManager()
        {
            _currentIteration = 1;
            _gameEngine = new GameEngine(0, 0); // Default initialization
        }

        public void Initialize(int rows, int columns)
        {
            _gameEngine = new GameEngine(rows, columns);
            _gameEngine.RandomizeInitialState();
            _currentIteration = 1;
        }

        public void LoadState(GameState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            
            _gameEngine = new GameEngine(state.Rows, state.Columns);
            _gameEngine.SetCurrentGrid(state.ToGrid());
            _currentIteration = state.Iteration;
        }

        public GameState GetCurrentState()
        {
            return new GameState(_gameEngine.GetCurrentGrid(), _currentIteration);
        }

        public void NextGeneration()
        {
            _gameEngine.NextGeneration();
            _currentIteration++;
        }

        public Grid GetCurrentGrid() => _gameEngine.GetCurrentGrid();

        public int CountLivingCells() => _gameEngine.GetCurrentGrid().CountLivingCells();

        public int CurrentIteration => _currentIteration;
    }
} 