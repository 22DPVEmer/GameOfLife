using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Services
{
    /// <summary>
    /// Service for saving and loading game states to/from files
    /// </summary>
    public class GameStateService
    {
        private readonly string _savePath;
        private const string SaveFileExtension = ".json";
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public GameStateService()
        {
            _savePath = Path.Combine(Directory.GetCurrentDirectory(), "saves");
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

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var fileName = $"game_save_{timestamp}{SaveFileExtension}";
            var filePath = Path.Combine(_savePath, fileName);
            var json = JsonSerializer.Serialize(state, _jsonOptions);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Gets a list of all available save files
        /// </summary>
        public List<string> GetSaveFiles()
        {
            return Directory.GetFiles(_savePath, $"*{SaveFileExtension}")
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
            return GetSaveFiles().Any();
        }
    }
} 