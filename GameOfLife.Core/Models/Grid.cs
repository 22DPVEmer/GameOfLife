using System;

namespace GameOfLife.Core.Models
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
            for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < Columns; colIndex++)
                {
                    _cells[rowIndex, colIndex] = new Cell(rowIndex, colIndex);
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

            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int colOffset = -1; colOffset <= 1; colOffset++)
                {
                    // Skip the cell itself
                    if (rowOffset == 0 && colOffset == 0) continue;

                    int neighborRow = row + rowOffset;
                    int neighborCol = column + colOffset;

                    if (neighborRow >= 0 && neighborRow < Rows && neighborCol >= 0 && neighborCol < Columns)
                    {
                        if (_cells[neighborRow, neighborCol].IsAlive)
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
            for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < Columns; colIndex++)
                {
                    newGrid.SetCell(rowIndex, colIndex, _cells[rowIndex, colIndex].IsAlive);
                }
            }
            return newGrid;
        }

        /// <summary>
        /// Randomly sets cells to alive or dead.
        /// Used to create the initial state of the game.
        /// </summary>
        public void RandomizeGrid(int seedPercentage = 20)
        {
            Random random = new Random();
            for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < Columns; colIndex++)
                {
                    _cells[rowIndex, colIndex].IsAlive = random.Next(100) < seedPercentage;
                }
            }
        }
    }
} 