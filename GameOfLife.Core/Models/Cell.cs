namespace GameOfLife.Core.Models
{
    /// <summary>
    /// Represents a single cell in Conway's Game of Life.
    /// Each cell has a position (row, column) and a state (alive or dead).
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Indicates whether the cell is alive (true) or dead (false).
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// The row position of the cell in the grid. This value cannot be changed after creation.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// The column position of the cell in the grid. This value cannot be changed after creation.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Creates a new cell with a specific position and initial state.
        /// </summary>
        /// <param name="row">The row where the cell is located.</param>
        /// <param name="column">The column where the cell is located.</param>
        /// <param name="isAlive">Whether the cell is alive (default is dead).</param>
        public Cell(int row, int column, bool isAlive = false)
        {
            Row = row;
            Column = column;
            IsAlive = isAlive;
        }

        /// <summary>
        /// Creates a copy of this cell with the same position and state.
        /// Used when creating the next generation to avoid modifying the current one.
        /// </summary>
        public Cell Clone()
        {
            return new Cell(Row, Column, IsAlive);
        }
    }
} 