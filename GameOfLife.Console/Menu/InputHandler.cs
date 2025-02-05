using System;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Services;
using GameOfLife.Console.Menu.Interfaces;

namespace GameOfLife.Console.Menu
{
    public class InputHandler
    {
        private readonly IConsoleUI _consoleUI;

        public InputHandler(IConsoleUI consoleUI)
        {
            _consoleUI = consoleUI ?? throw new ArgumentNullException(nameof(consoleUI));
        }

        public bool TryGetMenuChoice(out char choice)
        {
            var key = System.Console.ReadKey(true);
            choice = key.KeyChar;
            
            switch (choice)
            {
                case DisplayConstants.NEW_GAME:
                case DisplayConstants.LOAD_GAME:
                case DisplayConstants.NEW_PARALLEL_GAME:
                case DisplayConstants.LOAD_PARALLEL_GAME:
                case DisplayConstants.EXIT:
                    return true;
                default:
                    _consoleUI.DisplayMessage(DisplayConstants.INVALID_OPTION);
                    return false;
            }
        }

        public bool TryGetSaveChoice(IList<string> saves, out int choice)
        {
            choice = 0;
            return int.TryParse(System.Console.ReadLine(), out choice) && choice > 0 && choice <= saves.Count;
        }

        public (int rows, int columns) GetGridDimensions()
        {
            _consoleUI.DisplayMessage(DisplayConstants.GRID_DIMENSIONS_PROMPT);
            
            int rows = GetDimension(DisplayConstants.DIMENSION_ROWS);
            int columns = GetDimension(DisplayConstants.DIMENSION_COLUMNS);
            
            return (rows, columns);
        }

        private int GetDimension(string dimensionName)
        {
            while (true)
            {
                _consoleUI.DisplayMessage(string.Format(DisplayConstants.DIMENSION_INPUT_FORMAT, 
                    dimensionName, 
                    DisplayConstants.MIN_GRID_SIZE, 
                    DisplayConstants.MAX_GRID_SIZE));

                string input = System.Console.ReadLine();
                
                if (int.TryParse(input, out int value) && 
                    value >= DisplayConstants.MIN_GRID_SIZE && 
                    value <= DisplayConstants.MAX_GRID_SIZE)
                {
                    return value;
                }
                
                _consoleUI.DisplayMessage(string.Format(DisplayConstants.INVALID_DIMENSION_FORMAT, 
                    DisplayConstants.MIN_GRID_SIZE, 
                    DisplayConstants.MAX_GRID_SIZE,
                    dimensionName.ToLower()));
            }
        }
    }
} 