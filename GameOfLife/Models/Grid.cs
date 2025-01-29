using System;

namespace GameOfLife.Models
{
    /// <summary>
    /// Represents the game grid that contains all cells.
    /// Manages the 2D array of cells and provides methods to interact with them.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// The 2D array that holds all cells in the grid.
        /// Private to encapsulate the internal representation.
        /// </summary>
        private readonly Cell[,] _cells;

        /// <summary>
        /// Number of rows in the grid (immutable).
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// Number of columns in the grid (immutable).
        /// </summary>
        public int Columns { get; }

        /// <summary>
        /// Creates a new grid with specified dimensions.
        /// </summary>
        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _cells = new Cell[rows, columns];
            InitializeGrid();
        }

        /// <summary>
        /// Initializes all cells in the grid to dead state.
        /// Called once during grid creation.
        /// </summary>
        private void InitializeGrid()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    _cells[i, j] = new Cell(i, j);
                }
            }
        }

        /// <summary>
        /// Returns the cell at the specified position.
        /// </summary>
        public Cell GetCell(int row, int column)
        {
            return _cells[row, column];
        }

        /// <summary>
        /// Sets the state of a cell at the specified position.
        /// </summary>
        public void SetCell(int row, int column, bool isAlive)
        {
            _cells[row, column].IsAlive = isAlive;
        }

        /// <summary>
        /// Counts the number of live neighbors for a cell at the specified position.
        /// Checks all 8 neighboring cells (horizontal, vertical, and diagonal).
        /// </summary>
        public int GetLiveNeighborsCount(int row, int column)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // Skip the cell itself
                    if (i == 0 && j == 0) continue;

                    int newRow = row + i;
                    int newCol = column + j;

                    if (newRow >= 0 && newRow < Rows && newCol >= 0 && newCol < Columns)
                    {
                        if (_cells[newRow, newCol].IsAlive)
                            count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Used to create the next generation without modifying the current one.
        /// </summary>
        public Grid Clone()
        {
            Grid newGrid = new Grid(Rows, Columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    newGrid.SetCell(i, j, _cells[i, j].IsAlive);
                }
            }
            return newGrid;
        }

        /// <summary>
        /// Used to create the initial state of the game with 20% of the cells alive.
        /// </summary>
        public void RandomizeGrid(int seedPercentage = 20)
        {
            Random random = new Random();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    _cells[i, j].IsAlive = random.Next(100) < seedPercentage;
                }
            }
        }
    }
} 