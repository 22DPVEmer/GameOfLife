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
        public const int GAMES_PER_PAGE = 8;

        /// <summary>
        /// Game settings
        /// </summary>
        public const int INITIAL_ALIVE_CELLS_PERCENTAGE = 20;

        /// <summary>
        /// Parallel game settings
        /// </summary>
        public const int TOTAL_PARALLEL_GAMES = 1000;
        public const int VISIBLE_PARALLEL_GAMES = 8;

        // Format Strings
        public const string EVENT_MESSAGE_FORMAT = "[Event] {0}";
        public const string SAVE_FORMAT = "{0}. {1}";
        public const string DIMENSION_INPUT_FORMAT = "{0} ({1}-{2}): ";
        public const string INVALID_DIMENSION_FORMAT = "Please enter a number between {0} and {1} for {2}.";
        public const string GAME_STATUS_FORMAT = "Generation: {0} | Total Living Cells: {1} | Active Games: {2}";
        public const string SHOWING_GAMES_FORMAT = "Showing games {0} to {1}";
        public const string GAME_CONTROLS = "Generation: {0} | Living Cells: {1}";
        public const string PARALLEL_GAME_STATUS = "Generation: {0} | Total Living Cells: {1} | Active Games: {2}";
        public const string PARALLEL_GAMES_RANDOMIZED = "Games randomized!";
        public const string PARALLEL_GAMES_PAUSED = "Games paused!";
        public const string PARALLEL_GAMES_RESUMED = "Games resumed!";
        public const string PARALLEL_GAME_STATE_ERROR = "Invalid state type for parallel game";

        // Common Messages
        public const string ERROR_PREFIX = "Error: ";
        public const string INVALID_OPTION = "Invalid option. Please try again.";
        public const string CONTINUE_PROMPT = "Press any key to continue...";
        public const string EXIT_PROMPT = "Press any key to exit...";
        public const string SEPARATOR_LINE = "--------------------------------";
        public const string PRESS_ANY_KEY_MESSAGE = "Press any key to try again...";

        // Menu Messages
        public const string WELCOME_TEXT = "Welcome to Conway's Game of Life!";
        public const string MENU_HEADER = "\nGame of Life Menu:";
        public const string MENU_OPTION_1 = "1. New Game";
        public const string MENU_OPTION_2 = "2. Load Game";
        public const string MENU_OPTION_3 = "3. New Parallel Game";
        public const string MENU_OPTION_4 = "4. Load Parallel Game";
        public const string MENU_OPTION_5 = "5. Exit";
        public const string MENU_PROMPT = "\nSelect an option (1-5): ";

        // Grid Input Messages
        public const string GRID_DIMENSIONS_PROMPT = "\nEnter grid dimensions (10-30):";
        public const string GRID_SIZE_PROMPT = "Please enter grid dimensions:";
        public const string DIMENSION_ROWS = "Rows";
        public const string DIMENSION_COLUMNS = "Columns";
        public const string ROWS_PROMPT = "Enter number of rows (10-30): ";
        public const string COLUMNS_PROMPT = "Enter number of columns (10-30): ";
        public const string INVALID_INPUT_MESSAGE = "Invalid input! Please enter a number between 10 and 30.";

        // Save/Load Messages
        public const string AVAILABLE_SAVES_HEADER = "\nAvailable save files:";
        public const string SAVE_SELECTION_PROMPT = "\nEnter the number of the save file to load (or press Enter to cancel): ";
        public const string NO_SINGLE_SAVES = "No saved single games found.";
        public const string NO_PARALLEL_SAVES = "No saved parallel games found.";
        public const string GAME_SAVED_MESSAGE = "Game state saved successfully.";
        public const string GAME_LOADED_MESSAGE = "Game loaded successfully!";

        // Game Control Messages
        public const string QUIT_MESSAGE = "Exiting game...";
        public const string RETURN_TO_MENU_MESSAGE = "Returning to main menu...";
        public const string SINGLE_GAME_CONTROLS = "\nControls:\nSpace - Start/Stop\nS - Save\nQ - Quit\nM - Return to Menu";
        public const string PARALLEL_GAME_CONTROLS = "\nControls:\nSpace - Start/Stop\nS - Save\nQ - Quit\nM - Return to Menu\nArrow Keys - Navigate Games";
        public const string SWITCH_GAMES_PROMPT = "Use Left/Right arrows to shift view, Up/Down arrows to jump by 8 games";

        // Menu Options
        public const char NEW_GAME = '1';
        public const char LOAD_GAME = '2';
        public const char NEW_PARALLEL_GAME = '3';
        public const char LOAD_PARALLEL_GAME = '4';
        public const char EXIT = '5';

        // Error Messages
        public const string GAME_LOOP_NOT_INITIALIZED = "Game loop handler not initialized.";
        public const string INPUT_HANDLER_NOT_INITIALIZED = "Input handler not initialized.";
        public const string HANDLERS_NOT_INITIALIZED = "Handlers not properly initialized.";

        /// <summary>
        /// Console Renderer specific constants
        /// </summary>
        public const string GAME_HORIZONTAL_SEPARATOR = "          ";
        public const string RENDERER_GENERATION = "Generation: {0}";
        public const string RENDERER_LIVING_CELLS = "Living Cells: {0}";
        public const string RENDERER_CONTROLS_HEADER = "Controls:";
        public const string RENDERER_CONTROL_SPACE = "Space - Start/Stop";
        public const string RENDERER_CONTROL_SAVE = "S - Save";
        public const string RENDERER_CONTROL_QUIT = "Q - Quit";
        public const string RENDERER_CONTROL_MENU = "M - Return to Menu";

        /// <summary>
        /// Renderer display formats and messages
        /// </summary>
        public const string RENDERER_GENERATION_STATUS = "Generation: {0} | Total Living Cells: {1} | Active Games: {2}";
        public const string RENDERER_SHOWING_GAMES = "Showing games {0} to {1}";
        public const string RENDERER_CONTROLS = "Controls: [Q] Quit | [S] Save | [M] Menu | Arrow keys to change view | Space - Start/Stop";
        public const string RENDERER_NAVIGATION_HELP = "Left/Right arrows to shift view, Up/Down arrows to jump by 8 games";
    }
} 