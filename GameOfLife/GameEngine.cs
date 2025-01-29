using GameOfLife.Models;

namespace GameOfLife
{
    /// <summary>
    /// Core game engine that implements Conway's Game of Life rules.
    /// Responsible for calculating each new generation based on the current state.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        /// The current state of the game grid.
        /// </summary>
        private Grid _currentGrid;

        /// <summary>
        /// Creates a new game engine with a grid of specified dimensions.
        /// </summary>
        public GameEngine(int rows, int columns)
        {
            _currentGrid = new Grid(rows, columns);
        }

        /// <summary>
        /// Initializes the grid with random live cells.
        /// </summary>
        public void RandomizeInitialState()
        {
            _currentGrid.RandomizeGrid();
        }

        /// <summary>
        /// Returns the current state of the grid.
        /// </summary>
        public Grid GetCurrentGrid()
        {
            return _currentGrid;
        }

        /// <summary>
        /// Calculates the next generation based on Conway's Game of Life rules:
        /// 1. Any live cell with fewer than two live neighbours dies (underpopulation)
        /// 2. Any live cell with two or three live neighbours lives on to the next generation
        /// 3. Any live cell with more than three live neighbours dies (overpopulation)
        /// 4. Any dead cell with exactly three live neighbours becomes a live cell (reproduction)
        /// </summary>
        public void NextGeneration()
        {
            // Create a new grid to store the next generation
            Grid newGrid = _currentGrid.Clone();

            // Apply rules to each cell in the grid
            for (int row = 0; row < _currentGrid.Rows; row++)
            {
                for (int col = 0; col < _currentGrid.Columns; col++)
                {
                    // Count live neighbors for current cell
                    int liveNeighbors = _currentGrid.GetLiveNeighborsCount(row, col);
                    bool isCurrentlyAlive = _currentGrid.GetCell(row, col).IsAlive;

                    if (isCurrentlyAlive)
                    {
                        // Rules 1, 2, and 3: Cell survives only with 2 or 3 neighbors
                        newGrid.SetCell(row, col, liveNeighbors == 2 || liveNeighbors == 3);
                    }
                    else
                    {
                        // Rule 4: Dead cell comes alive with exactly 3 neighbors
                        newGrid.SetCell(row, col, liveNeighbors == 3);
                    }
                }
            }

            // Update the current grid to the next generation
            _currentGrid = newGrid;
        }
    }
} 