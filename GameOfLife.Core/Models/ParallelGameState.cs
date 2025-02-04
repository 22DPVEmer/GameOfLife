using System.Collections.Generic;

namespace GameOfLife.Core.Models
{
    public class ParallelGameState : GameState
    {
        public List<GameState> Games { get; }
        public int Rows { get; }
        public int Columns { get; }

        public ParallelGameState(List<GameState> games, int rows, int columns) : base(games[0].ToGrid(), games[0].Iteration)
        {
            Games = games;
            Rows = rows;
            Columns = columns;
        }
    }
} 