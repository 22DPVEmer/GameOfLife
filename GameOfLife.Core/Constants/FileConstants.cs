using System.IO;

namespace GameOfLife.Core.Constants
{
    public static class FileConstants
    {
        public const string SAVE_FILE_EXTENSION = ".json";
        private const string SAVE_DIRECTORY_NAME = "saves";
        
        /// <summary>
        /// Gets the default path for saving game states, which combines the current directory
        /// with the saves folder name. This path is used when no custom save location is specified.
        /// </summary>
        public static string DEFAULT_SAVE_PATH => Path.Combine(Directory.GetCurrentDirectory(), SAVE_DIRECTORY_NAME);
    }
} 