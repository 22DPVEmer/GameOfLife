using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using GameOfLife.Core.Models;
using GameOfLife.Core.Constants;

namespace GameOfLife.Core.Services
{
    /// <summary>
    /// Service for saving and loading game states to/from files
    /// </summary>
    public class GameStateService
    {
        private readonly string _savePath;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public GameStateService()
        {
            _savePath = FileConstants.DEFAULT_SAVE_PATH;
            Directory.CreateDirectory(_savePath);
        }

        public GameStateService(string customSavePath)
        {
            _savePath = customSavePath;
            Directory.CreateDirectory(_savePath);
        }

        /// <summary>
        /// Saves the current game state with a timestamp-based filename
        /// </summary>
        public void SaveGame(GameState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filePath = Path.Combine(_savePath, $"save_{timestamp}{FileConstants.SAVE_FILE_EXTENSION}");
            var json = JsonSerializer.Serialize(state, _jsonOptions);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Gets a list of all available save files
        /// </summary>
        public List<string> GetSaveFiles()
        {
            if (!Directory.Exists(_savePath))
            {
                return new List<string>();
            }

            return Directory.GetFiles(_savePath, $"*{FileConstants.SAVE_FILE_EXTENSION}")
                .Select(Path.GetFileName)
                .ToList();
        }

        /// <summary>
        /// Loads a game state from a specific save file
        /// </summary>
        public GameState? LoadGame(string fileName)
        {
            var filePath = Path.Combine(_savePath, fileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            var json = File.ReadAllText(filePath);
            try 
            {
                return JsonSerializer.Deserialize<GameState>(json, _jsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        /// <summary>
        /// Loads the most recent save file
        /// </summary>
        public GameState? LoadMostRecentGame()
        {
            var saves = GetSaveFiles();
            if (!saves.Any())
                return null;

            var mostRecent = saves.OrderByDescending(f => f).First();
            return LoadGame(mostRecent);
        }

        public bool SaveFileExists()
        {
            return Directory.Exists(_savePath) && 
                   Directory.GetFiles(_savePath, $"*{FileConstants.SAVE_FILE_EXTENSION}").Any();
        }
    }
} 