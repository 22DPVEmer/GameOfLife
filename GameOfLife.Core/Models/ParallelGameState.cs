using System.Collections.Generic;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// Represents the state of multiple Game of Life instances running in parallel.
    /// Extends GameState to support saving and loading multiple game states simultaneously.
    /// </summary>
    public class ParallelGameState : GameState
    {
        /// <summary>
        /// Gets the list of individual game states.
        /// </summary>
        public List<GameState> Games { get; }

        /// <summary>
        /// Gets the number of rows in each game grid.
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// Gets the number of columns in each game grid.
        /// </summary>
        public int Columns { get; }

        /// <summary>
        /// Initializes a new instance of the parallel game state.
        /// </summary>
        /// <param name="games">List of individual game states</param>
        /// <param name="rows">Number of rows in each game grid</param>
        /// <param name="columns">Number of columns in each game grid</param>
        public ParallelGameState(List<GameState> games, int rows, int columns) : base(games[0].ToGrid(), games[0].Iteration)
        {
            Games = games;
            Rows = rows;
            Columns = columns;
        }
    }
} 