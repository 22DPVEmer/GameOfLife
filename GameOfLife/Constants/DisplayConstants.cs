namespace GameOfLife.Constants
{
    public static class DisplayConstants
    {
        /// <summary>
        /// Characters used to represent cells in the console
        /// </summary>
        public const char ALIVE_CELL = '■';
        public const char DEAD_CELL = '□';

        /// <summary>
        /// Grid size constraints
        /// </summary>
        public const int MIN_GRID_SIZE = 5;
        public const int MAX_GRID_SIZE = 30;

        /// <summary>
        /// Menu and message texts
        /// </summary>
        public const string WELCOME_TEXT = "Welcome to Conway's Game of Life!";
        public const string SEPARATOR_LINE = "--------------------------------";
        public const string GRID_SIZE_PROMPT = "Please enter the grid size:";
        public const string ROWS_PROMPT = "Rows (5-30): ";
        public const string COLUMNS_PROMPT = "Columns (5-30): ";
        public const string INVALID_INPUT_MESSAGE = "Invalid input. Please enter a number between 5 and 30.";
        public const string PRESS_ANY_KEY_MESSAGE = "Press any key to try again...";
        public const string QUIT_INSTRUCTION = "Press 'Q' to quit the game";
        public const string AUTO_UPDATE_MESSAGE = "Game will update automatically every second...";
    }
} 