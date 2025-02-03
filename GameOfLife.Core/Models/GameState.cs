using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// Represents the complete state of a game that can be saved to or loaded from a file
    /// </summary>
    [Serializable]
    public class GameState
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Iteration { get; set; }

        // Store cell states as a list of lists for better serialization support
        [JsonPropertyName("cells")]
        public List<List<bool>> CellStates { get; set; }

        public GameState() 
        {
            CellStates = new List<List<bool>>();
        }

        public GameState(Grid grid, int iteration)
        {
            Rows = grid.Rows;
            Columns = grid.Columns;
            Iteration = iteration;
            CellStates = new List<List<bool>>();

            for (int row = 0; row < Rows; row++)
            {
                var rowList = new List<bool>();
                for (int col = 0; col < Columns; col++)
                {
                    rowList.Add(grid.GetCell(row, col).IsAlive);
                }
                CellStates.Add(rowList);
            }
        }

        public Grid ToGrid()
        {
            var grid = new Grid(Rows, Columns);
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (CellStates[row][col])
                    {
                        grid.GetCell(row, col).IsAlive = true;
                    }
                }
            }
            return grid;
        }
    }
} 