using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Services
{
    /// <summary>
    /// Service for saving and loading game states to/from files
    /// </summary>
    public class GameStateService : IGameStateService
    {
        private readonly string _savePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public GameStateService()
        {
            _savePath = Path.Combine(Environment.CurrentDirectory, FileConstants.SAVE_DIRECTORY);
            _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
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
            var timestamp = DateTime.Now.ToString(FileConstants.SAVE_TIMESTAMP_FORMAT);
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
                return new List<string>();

            var prefix = parallelGames ? FileConstants.PARALLEL_GAME_PREFIX : FileConstants.SINGLE_GAME_PREFIX;
            
            // Functional pipeline to transform file paths:
            // 1. Get all matching files using the prefix pattern
            // 2. Extract just the filename from the full path
            // 3. Filter out any null values for safety
            // 4. Convert nullable strings to non-nullable (with null check)
            // 5. Convert to a materialized list
            var files = Directory.GetFiles(_savePath, $"{prefix}*{FileConstants.SAVE_FILE_EXTENSION}")
                               .Select(Path.GetFileName)
                               .Where(f => f != null)
                               .Select(f => f!)
                               .ToList();
            return files;
        }

        /// <summary>
        /// Loads a game state from a specific save file
        /// </summary>
        public GameState LoadGame(string filename)
        {
            var filePath = Path.Combine(_savePath, filename);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Save file not found: {filename}");
            }

            var json = File.ReadAllText(filePath);
            var state = JsonSerializer.Deserialize<GameState>(json, _jsonOptions);
            if (state == null)
            {
                throw new InvalidOperationException("Failed to deserialize game state");
            }
            return state;
        }

        /// <summary>
        /// Loads the most recent save file of a specific type
        /// </summary>
        public GameState LoadMostRecentGame(bool parallelGames = false)
        {
            var saves = GetSaveFiles(parallelGames);
            if (!saves.Any())
                throw new FileNotFoundException("No save files found");

            var mostRecent = saves.OrderByDescending(f => f).First();
            var state = LoadGame(mostRecent);
            if (state == null)
            {
                throw new InvalidOperationException("Failed to load most recent game state");
            }
            return state;
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

            var statesList = states.ToList();
            var parallelState = new ParallelGameState(statesList, rows, columns);

            var timestamp = DateTime.Now.ToString(FileConstants.SAVE_TIMESTAMP_FORMAT);
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
            var state = JsonSerializer.Deserialize<ParallelGameState>(json, _jsonOptions);
            if (state == null)
            {
                throw new InvalidOperationException("Failed to deserialize parallel game state");
            }
            return state;
        }

        public void SaveParallelGame(ParallelGameState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            
            var timestamp = DateTime.Now.ToString(FileConstants.SAVE_TIMESTAMP_FORMAT);
            var filePath = Path.Combine(_savePath, $"{FileConstants.PARALLEL_GAME_PREFIX}save_{timestamp}{FileConstants.SAVE_FILE_EXTENSION}");
            var json = JsonSerializer.Serialize(state, _jsonOptions);
            File.WriteAllText(filePath, json);
        }
    }
} 