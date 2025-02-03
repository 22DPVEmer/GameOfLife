using System.Collections.Generic;

namespace GameOfLife.Core.Models
{
    public class ParallelGameState
    {
        public List<GameState> Games { get; set; }
        public int TotalGames { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public ParallelGameState()
        {
            Games = new List<GameState>();
        }
    }
} 