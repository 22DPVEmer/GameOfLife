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
        public const int GAME_UPDATE_INTERVAL_MS = 100;
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
        public const string AUTO_UPDATE_MESSAGE = "Game will auto-update. Press [Q] to quit, [S] to save, [L] to load.";
        public const string ERROR_PREFIX = "Error: ";
        public const string EXIT_PROMPT = "Press any key to exit...";
        
        // Game status and control messages
        public const string QuitInstruction = "Press Q to quit";
        public const string SaveInstruction = "Press S to save";
        public const string LoadInstruction = "Press L to load";
        public const string MainMenuInstruction = "Press M for menu";
        public const string GAME_CONTROLS = "Generation: {0} | Total Living Cells: {1} | Active Games: {2}";
        public const string EVENT_MESSAGE_FORMAT = "\n{0}";
        public const string GameSavedMessage = "Game saved successfully!";
        public const string GameLoadedMessage = "Game loaded successfully!";
        public const string NoSaveFileMessage = "No save files found.";
        public const string QuitMessage = "Quitting game...";
        public const string ReturnToMenuMessage = "Returning to menu...";

        /// <summary>
        /// Main menu options
        /// </summary>
        public const string MENU_OPTION_NEW_GAME = "1. Start New Game";
        public const string MENU_OPTION_LOAD_GAME = "2. Load Game";
        public const string MENU_OPTION_EXIT = "3. Exit";
        public const string MENU_SELECT_PROMPT = "Select an option: ";

        /// <summary>
        /// Save/Load menu messages
        /// </summary>
        public const string AVAILABLE_SAVES_HEADER = "Available save files:";
        public const string SAVE_SELECTION_PROMPT = "Enter the number of the save to load: ";
        public const string CONTINUE_PROMPT = "Press any key to continue...";
        public const string SAVE_FORMAT = "{0}. {1}";

        // Add new constants
        public const string SWITCH_GAMES_PROMPT = "Use Left/Right arrows to shift view, Up/Down arrows to jump by 8 games";
        public const string SAVE_SUCCESS = "Saved {0} games!";

        // Parallel game options
        public const string START_SINGLE_GAME = "1. Start Single Game";
        public const string LOAD_SINGLE_GAME = "2. Load Single Game";
        public const string START_PARALLEL_GAMES = "3. Start 1000 Parallel Games";
        public const string LOAD_PARALLEL_GAMES = "4. Load Parallel Games";
        public const string EXIT_OPTION = "5. Exit";
        public const string SELECT_OPTION_PROMPT = "\nSelect an option (1-5): ";
        public const string INVALID_OPTION_MESSAGE = "\nInvalid option. Press any key to continue...";

        // Parallel game display
        public const string SHOWING_GAMES_FORMAT = "Showing games: {0}";
    }
} 