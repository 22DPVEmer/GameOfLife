using System.Threading.Tasks;
using GameOfLife.Core.Interfaces;
using GameOfLife.Core.Services;
using GameOfLife.Console.Menu;
using GameOfLife.Console.Menu.Interfaces;
using GameOfLife.Console.Renderers;

namespace GameOfLife.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize UI components
            var consoleUI = new ConsoleUI();
            var menuRenderer = new MenuRenderer();
            var inputHandler = new InputHandler(consoleUI);

            // Initialize core services
            var renderer = new ConsoleRenderer(consoleUI, inputHandler);
            var gameStateService = new GameStateService();

            // Initialize game factories and loaders
            IGameManagerFactory gameFactory = new GameManagerFactory(gameStateService, renderer);
            IGameLoader gameLoader = new GameLoader(gameStateService, gameFactory, consoleUI);
            
            // Initialize and start the game menu
            var gameMenu = new GameMenu(
                renderer, 
                gameStateService, 
                menuRenderer, 
                inputHandler,
                gameLoader,
                gameFactory,
                consoleUI);
            
            await gameMenu.ShowMenu();
        }
    }
}
