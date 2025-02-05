using System.IO;

namespace GameOfLife.Core.Constants
{
    public static class FileConstants
    {
        public const string SAVE_DIRECTORY = "saves";
        public const string SAVE_FILE_EXTENSION = ".json";
        public const string SINGLE_GAME_PREFIX = "single_";
        public const string PARALLEL_GAME_PREFIX = "parallel_";
        
        /// <summary>
        /// Format string for timestamps in save file names
        /// </summary>
        public const string SAVE_TIMESTAMP_FORMAT = "yyyyMMdd_HHmmss";
        
        /// <summary>
        /// Gets the default path for saving game states, which combines the current directory
        /// with the saves folder name. This path is used when no custom save location is specified.
        /// </summary>
        public static string DEFAULT_SAVE_PATH => Path.Combine(Directory.GetCurrentDirectory(), SAVE_DIRECTORY);
    }
} 