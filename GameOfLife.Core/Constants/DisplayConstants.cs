namespace GameOfLife.Core.Constants
{
    public static class DisplayConstants
    {
        /// <summary>
        /// Characters used to represent cells in the console
        /// </summary>
        public const char ALIVE_CELL = '■';
        public const char DEAD_CELL = '□';
        public const char CELL_SEPARATOR = ' ';
        public const string NEW_LINE = "\n";

        /// <summary>
        /// Grid size constraints
        /// </summary>
        public const int MIN_GRID_SIZE = 5;
        public const int MAX_GRID_SIZE = 30;

        /// <summary>
        /// Timing constants (in milliseconds)
        /// </summary>
        public const int GAME_UPDATE_INTERVAL_MS = 2000;
        public const int INPUT_CHECK_INTERVAL_MS = 100;

        /// <summary>
        /// Game settings
        /// </summary>
        public const int INITIAL_ALIVE_CELLS_PERCENTAGE = 20;

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
        public const string AUTO_UPDATE_MESSAGE = "Game will update automatically every 2 seconds...";
        public const string ERROR_PREFIX = "Error: ";
        public const string EXIT_PROMPT = "Press any key to exit...";
        
        // Game status and control messages
        public const string QuitInstruction = "Press Q to quit";
        public const string SaveInstruction = "Press S to save";
        public const string LoadInstruction = "Press L to load";
        public const string MainMenuInstruction = "Press M for menu";
        public const string GAME_CONTROLS = "Generation: {0} | Living Cells: {1} | Press Q to quit | Press S to save | Press M for menu";
        public const string EVENT_MESSAGE_FORMAT = "\n{0}";
        public const string GameSavedMessage = "Game saved successfully!";
        public const string GameLoadedMessage = "Game loaded successfully!";
        public const string NoSaveFileMessage = "No save file found.";
        public const string QuitMessage = "Game stopped. Thanks for playing!";
        public const string ReturnToMenuMessage = "Returning to main menu...";

        /// <summary>
        /// Main menu options
        /// </summary>
        public const string MENU_OPTION_NEW_GAME = "1. Start New Game";
        public const string MENU_OPTION_LOAD_GAME = "2. Load Game";
        public const string MENU_OPTION_EXIT = "3. Exit";
        public const string MENU_SELECT_PROMPT = "\nSelect an option (1-3):";

        /// <summary>
        /// Save/Load menu messages
        /// </summary>
        public const string AVAILABLE_SAVES_HEADER = "\nAvailable saves:";
        public const string SAVE_SELECTION_PROMPT = "\nEnter save number to load (or any other key to return):";
        public const string CONTINUE_PROMPT = "\nPress any key to continue...";
        public const string SAVE_FORMAT = "{0}. {1}";
    }
} 