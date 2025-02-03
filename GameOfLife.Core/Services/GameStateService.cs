using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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
        /// Saves a single game state with a timestamp-based filename
        /// </summary>
        public void SaveGame(GameState state, bool isParallel = false)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }

            var prefix = isParallel ? FileConstants.PARALLEL_GAME_PREFIX : FileConstants.SINGLE_GAME_PREFIX;
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filePath = Path.Combine(_savePath, $"{prefix}save_{timestamp}{FileConstants.SAVE_FILE_EXTENSION}");
            var json = JsonSerializer.Serialize(state, _jsonOptions);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Gets a list of all available save files of a specific type
        /// </summary>
        public List<string> GetSaveFiles(bool parallelGames = false)
        {
            if (!Directory.Exists(_savePath))
            {
                return new List<string>();
            }

            var prefix = parallelGames ? FileConstants.PARALLEL_GAME_PREFIX : FileConstants.SINGLE_GAME_PREFIX;
            return Directory.GetFiles(_savePath, $"{prefix}*{FileConstants.SAVE_FILE_EXTENSION}")
                .Select(Path.GetFileName)
                .ToList();
        }

        /// <summary>
        /// Loads a game state from a specific save file
        /// </summary>
        public GameState LoadGame(string fileName)
        {
            var filePath = Path.Combine(_savePath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Save file not found: {fileName}");
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameState>(json, _jsonOptions);
        }

        /// <summary>
        /// Loads the most recent save file of a specific type
        /// </summary>
        public GameState? LoadMostRecentGame(bool parallelGames = false)
        {
            var saves = GetSaveFiles(parallelGames);
            if (!saves.Any())
                return null;

            var mostRecent = saves.OrderByDescending(f => f).First();
            return LoadGame(mostRecent);
        }

        public bool SaveFileExists(bool parallelGames = false)
        {
            var prefix = parallelGames ? FileConstants.PARALLEL_GAME_PREFIX : FileConstants.SINGLE_GAME_PREFIX;
            return Directory.Exists(_savePath) && 
                   Directory.GetFiles(_savePath, $"{prefix}*{FileConstants.SAVE_FILE_EXTENSION}").Any();
        }

        /// <summary>
        /// Saves multiple game states in a single file for parallel games
        /// </summary>
        public void SaveParallelGames(IEnumerable<GameState> states, int rows, int columns)
        {
            if (states == null)
                throw new ArgumentNullException(nameof(states));

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }

            var parallelState = new ParallelGameState
            {
                Games = states.ToList(),
                TotalGames = states.Count(),
                Rows = rows,
                Columns = columns
            };

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filePath = Path.Combine(_savePath, $"{FileConstants.PARALLEL_GAME_PREFIX}save_{timestamp}{FileConstants.SAVE_FILE_EXTENSION}");
            var json = JsonSerializer.Serialize(parallelState, _jsonOptions);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Loads a parallel game state from a specific save file
        /// </summary>
        public ParallelGameState LoadParallelGame(string fileName)
        {
            var filePath = Path.Combine(_savePath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Save file not found: {fileName}");
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<ParallelGameState>(json, _jsonOptions);
        }
    }
} 