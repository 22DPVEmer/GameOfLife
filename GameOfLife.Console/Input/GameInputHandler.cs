using System;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.Console.Input
{
    public class GameInputHandler
    {
        private readonly IGameStateService _gameStateService;
        private readonly Action _saveAction;
        private readonly Action<ConsoleKeyInfo> _randomizeAction;
        private readonly Action _quitAction;
        private readonly Action _menuAction;
        private readonly Action<ConsoleKey>? _arrowKeyAction;
        private readonly Action? _togglePauseAction;
        private bool _isRunning;

        public GameInputHandler(
            IGameStateService gameStateService,
            Action saveAction,
            Action<ConsoleKeyInfo> randomizeAction,
            Action quitAction,
            Action menuAction,
            Action<ConsoleKey>? arrowKeyAction = null,
            Action? togglePauseAction = null)
        {
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
            _saveAction = saveAction ?? throw new ArgumentNullException(nameof(saveAction));
            _randomizeAction = randomizeAction ?? throw new ArgumentNullException(nameof(randomizeAction));
            _quitAction = quitAction ?? throw new ArgumentNullException(nameof(quitAction));
            _menuAction = menuAction ?? throw new ArgumentNullException(nameof(menuAction));
            _arrowKeyAction = arrowKeyAction;
            _togglePauseAction = togglePauseAction;
            _isRunning = true;
        }

        public void Start()
        {
            _isRunning = true;
            while (_isRunning)
            {
                var keyInfo = System.Console.ReadKey(true);
                HandleInput(keyInfo);
            }
        }

        public void Stop() => _isRunning = false;

        private void HandleInput(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.S:
                    _saveAction();
                    break;
                case ConsoleKey.R:
                    _randomizeAction(keyInfo);
                    break;
                case ConsoleKey.Q:
                    _quitAction();
                    break;
                case ConsoleKey.M:
                    _menuAction();
                    break;
                case ConsoleKey.Spacebar:
                    _togglePauseAction?.Invoke();
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                    _arrowKeyAction?.Invoke(keyInfo.Key);
                    break;
            }
        }
    }
} 