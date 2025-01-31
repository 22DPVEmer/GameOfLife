using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Services
{
    /// <summary>
    /// Service for saving and loading game states to/from files
    /// </summary>
    public class GameStateService
    {
        private readonly string _saveDirectory;

        public GameStateService()
        {
            _saveDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GameOfLife",
                "Saves"
            );
            Directory.CreateDirectory(_saveDirectory);
        }

        /// <summary>
        /// Saves the current game state with a timestamp-based filename
        /// </summary>
        public void SaveGame(GameState state)
        {
            var fileName = $"game_save_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(_saveDirectory, fileName);
            
            var json = JsonSerializer.Serialize(state);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Gets a list of all available save files
        /// </summary>
        public List<string> GetSaveFiles()
        {
            return new List<string>(Directory.GetFiles(_saveDirectory, "game_save_*.json"));
        }

        /// <summary>
        /// Loads a game state from a specific save file
        /// </summary>
        public GameState LoadGame(string fileName)
        {
            var filePath = Path.Combine(_saveDirectory, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Save file not found: {fileName}");
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameState>(json);
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
            return Directory.GetFiles(_saveDirectory, "game_save_*.json").Length > 0;
        }
    }
} 