using System;

namespace GameOfLife.Core.Constants
{
    public static class DisplayConstants
    {
        /// <summary>
        /// Characters used to represent cells in the console
        /// </summary>
        public const char ALIVE_CELL = '■';
        public const char DEAD_CELL = '□';
        public const string CELL_SEPARATOR = " ";
        public const string NEW_LINE = "\n";

        /// <summary>
        /// Grid size constraints
        /// </summary>
        public const int MIN_GRID_SIZE = 10;
        public const int MAX_GRID_SIZE = 30;

        /// <summary>
        /// Timing constants (in milliseconds)
        /// </summary>
        public const int GAME_UPDATE_INTERVAL_MS = 1000;
        public const int INPUT_CHECK_INTERVAL_MS = 50;
        public const int MAX_PARALLEL_GAMES = 1000;
        public const int MAX_VISIBLE_GAMES = 8;
        public const int GAMES_PER_PAGE = 8;

        /// <summary>
        /// Game settings
        /// </summary>
        public const int INITIAL_ALIVE_CELLS_PERCENTAGE = 20;

        /// <summary>
        /// Menu and message texts
        /// </summary>
        public const string WELCOME_TEXT = "Welcome to Conway's Game of Life!";
        public const string SEPARATOR_LINE = "--------------------------------";
        public const string GRID_SIZE_PROMPT = "Please enter grid dimensions:";
        public const string ROWS_PROMPT = "Enter number of rows (10-30): ";
        public const string COLUMNS_PROMPT = "Enter number of columns (10-30): ";
        public const string INVALID_INPUT_MESSAGE = "Invalid input! Please enter a number between 10 and 30.";
        public const string PRESS_ANY_KEY_MESSAGE = "Press any key to try again...";
        public const string SINGLE_GAME_CONTROLS = "\nControls: [Q] Quit | [S] Save | [M] Menu";
        public const string PARALLEL_GAME_CONTROLS = "\nControls: [Q] Quit | [S] Save | [M] Menu | Arrow keys to change view";
        public const string ERROR_PREFIX = "Error: ";
        public const string EXIT_PROMPT = "Press any key to exit...";
        
        // Main menu options
        public const string MENU_OPTION_1 = "1. Start Single Game";
        public const string MENU_OPTION_2 = "2. Load Single Game";
        public const string MENU_OPTION_3 = "3. Start 1000 Parallel Games";
        public const string MENU_OPTION_4 = "4. Load Parallel Games";
        public const string MENU_OPTION_5 = "5. Exit";
        public const string MENU_PROMPT = "\nSelect an option (1-5): ";
        public const string INVALID_OPTION = "\nInvalid option. Press any key to continue...";

        // Save/Load messages
        public const string NO_SINGLE_SAVES = "\nNo single game save files found. Press any key to continue...";
        public const string NO_PARALLEL_SAVES = "\nNo parallel game save files found. Press any key to continue...";
        public const string AVAILABLE_SAVES_HEADER = "\nAvailable save files:";
        public const string SAVE_SELECTION_PROMPT = "Enter the number of the save to load: ";
        public const string SAVE_FORMAT = "{0}. {1}";
        public const string SAVE_SUCCESS = "Saved {0} games!";
        public const string CONTINUE_PROMPT = "Press any key to continue...";

        // Game status messages
        public const string GAME_CONTROLS = "Generation: {0} | Total Living Cells: {1} | Active Games: {2}";
        public const string EVENT_MESSAGE_FORMAT = "\n{0}";
        public const string GAME_SAVED_MESSAGE = "Game saved successfully!";
        public const string GAME_LOADED_MESSAGE = "Game loaded successfully!";
        public const string QUIT_MESSAGE = "Quitting game...";
        public const string RETURN_TO_MENU_MESSAGE = "Returning to menu...";

        // Parallel game display
        public const string SHOWING_GAMES_FORMAT = "Showing games: {0}";
        public const string SWITCH_GAMES_PROMPT = "Use Left/Right arrows to shift view, Up/Down arrows to jump by 8 games";

        // Menu Options
        public const char NEW_GAME = '1';
        public const char LOAD_GAME = '2';
        public const char NEW_PARALLEL_GAME = '3';
        public const char LOAD_PARALLEL_GAME = '4';
        public const char EXIT = '5';
    }
} 