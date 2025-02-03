using System;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Models;
using GameOfLife.Core.Services;

namespace GameOfLife.Console
{
    public abstract class BaseGameManager
    {
        protected readonly IRenderer _renderer;
        protected readonly GameStateService _gameStateService;
        protected bool _isRunning;
        protected readonly int _updateIntervalMs;
        protected readonly object _renderLock;

        protected BaseGameManager(IRenderer renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _gameStateService = new GameStateService();
            _updateIntervalMs = DisplayConstants.GAME_UPDATE_INTERVAL_MS;
            _renderLock = new object();
            _isRunning = false;
        }

        protected BaseGameManager(IRenderer renderer, GameStateService gameStateService)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
            _updateIntervalMs = DisplayConstants.GAME_UPDATE_INTERVAL_MS;
            _renderLock = new object();
            _isRunning = false;
        }

        protected async Task HandleBasicInput(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Q:
                    _isRunning = false;
                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.QUIT_MESSAGE);
                    await Task.Delay(1000);
                    Environment.Exit(0);
                    break;
                case ConsoleKey.M:
                    _isRunning = false;
                    System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.RETURN_TO_MENU_MESSAGE);
                    await Task.Delay(1000);
                    break;
            }
        }

        protected void DisplayError(Exception ex)
        {
            System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, DisplayConstants.ERROR_PREFIX + ex.Message);
        }

        protected async Task ShowTemporaryMessage(string message, int delayMs = 1000)
        {
            System.Console.WriteLine(DisplayConstants.EVENT_MESSAGE_FORMAT, message);
            await Task.Delay(delayMs);
        }

        public abstract Task StartGame();
    }
} 