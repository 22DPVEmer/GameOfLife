using System;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;

namespace GameOfLife.Console.Input
{
    public class ConsoleGameInputHandler : IGameInputHandler
    {
        private readonly IGameStateManager _gameStateManager;
        private readonly IGameStateService _gameStateService;
        private bool _isListening;

        public bool IsListening => _isListening;

        public ConsoleGameInputHandler(IGameStateManager gameStateManager, IGameStateService gameStateService)
        {
            _gameStateManager = gameStateManager ?? throw new ArgumentNullException(nameof(gameStateManager));
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
            _isListening = false;
        }

        public async Task HandleInput(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.S:
                    await SaveGame();
                    break;
                case ConsoleKey.Q:
                    await QuitGame();
                    break;
                case ConsoleKey.M:
                    await ReturnToMenu();
                    break;
            }
        }

        public async Task StartListening()
        {
            _isListening = true;
            while (_isListening)
            {
                if (System.Console.KeyAvailable)
                {
                    var key = System.Console.ReadKey(true);
                    await HandleInput(key);
                }
                await Task.Delay(DisplayConstants.INPUT_CHECK_INTERVAL_MS);
            }
        }

        public void StopListening()
        {
            _isListening = false;
        }

        private async Task SaveGame()
        {
            try
            {
                var state = _gameStateManager.GetCurrentState();
                _gameStateService.SaveGame(state);
                await ShowTemporaryMessage(DisplayConstants.GAME_SAVED_MESSAGE);
            }
            catch (Exception ex)
            {
                await ShowTemporaryMessage(DisplayConstants.ERROR_PREFIX + ex.Message);
            }
        }

        private async Task QuitGame()
        {
            _isListening = false;
            await ShowTemporaryMessage(DisplayConstants.QUIT_MESSAGE);
            Environment.Exit(0);
        }

        private async Task ReturnToMenu()
        {
            _isListening = false;
            await ShowTemporaryMessage(DisplayConstants.RETURN_TO_MENU_MESSAGE);
        }

        private async Task ShowTemporaryMessage(string message, int delayMs = 1000)
        {
            System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, message);
            await Task.Delay(delayMs);
        }
    }
} 