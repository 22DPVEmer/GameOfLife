using System;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Services;

namespace GameOfLife.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var renderer = new ConsoleRenderer();
            var gameStateService = new GameStateService();
            bool exitProgram = false;

            while (!exitProgram)
            {
                System.Console.Clear();
                System.Console.WriteLine(DisplayConstants.WELCOME_TEXT);
                System.Console.WriteLine(DisplayConstants.SEPARATOR_LINE);
                System.Console.WriteLine("1. Start Single Game");
                System.Console.WriteLine("2. Load Single Game");
                System.Console.WriteLine("3. Start 1000 Parallel Games");
                System.Console.WriteLine("4. Load Parallel Games");
                System.Console.WriteLine("5. Exit");
                System.Console.Write("\nSelect an option (1-5): ");

                var choice = System.Console.ReadKey(true).KeyChar;

                switch (choice)
                {
                    case '1':
                        var (rows, columns) = renderer.GetGridSize();
                        var singleGame = new GameManager(rows, columns, renderer);
                        await singleGame.StartGame();
                        break;

                    case '2':
                        if (gameStateService.SaveFileExists())
                        {
                            var saves = gameStateService.GetSaveFiles();
                            System.Console.WriteLine("\n" + DisplayConstants.AVAILABLE_SAVES_HEADER);
                            for (int i = 0; i < saves.Count; i++)
                            {
                                System.Console.WriteLine($"{i + 1}. {saves[i]}");
                            }
                            System.Console.WriteLine(DisplayConstants.SAVE_SELECTION_PROMPT);

                            if (int.TryParse(System.Console.ReadLine(), out int saveChoice) && 
                                saveChoice > 0 && saveChoice <= saves.Count)
                            {
                                var state = gameStateService.LoadGame(saves[saveChoice - 1]);
                                var loadedGame = new GameManager(state.Rows, state.Columns, renderer);
                                loadedGame.LoadState(state);
                                await loadedGame.StartGame();
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("\nNo save files found. Press any key to continue...");
                            System.Console.ReadKey(true);
                        }
                        break;

                    case '3':
                        var (parallelRows, parallelColumns) = renderer.GetGridSize();
                        var newParallelGames = new ParallelGameManager(1000, parallelRows, parallelColumns, renderer);
                        await newParallelGames.StartGames();
                        break;

                    case '4':
                        if (gameStateService.SaveFileExists())
                        {
                            var saves = gameStateService.GetSaveFiles();
                            System.Console.WriteLine("\n" + DisplayConstants.AVAILABLE_SAVES_HEADER);
                            for (int i = 0; i < saves.Count; i++)
                            {
                                System.Console.WriteLine($"{i + 1}. {saves[i]}");
                            }
                            System.Console.WriteLine(DisplayConstants.SAVE_SELECTION_PROMPT);

                            if (int.TryParse(System.Console.ReadLine(), out int saveChoice) && 
                                saveChoice > 0 && saveChoice <= saves.Count)
                            {
                                var state = gameStateService.LoadGame(saves[saveChoice - 1]);
                                var loadedParallelGames = new ParallelGameManager(1000, state.Rows, state.Columns, renderer, state);
                                await loadedParallelGames.StartGames();
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("\nNo save files found. Press any key to continue...");
                            System.Console.ReadKey(true);
                        }
                        break;

                    case '5':
                        exitProgram = true;
                        break;

                    default:
                        System.Console.WriteLine("\nInvalid option. Press any key to continue...");
                        System.Console.ReadKey(true);
                        break;
                }
            }
        }
    }
}
