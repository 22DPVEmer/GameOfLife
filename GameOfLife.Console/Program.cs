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
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_1);
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_2);
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_3);
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_4);
                System.Console.WriteLine(DisplayConstants.MENU_OPTION_5);
                System.Console.Write(DisplayConstants.MENU_PROMPT);

                var choice = System.Console.ReadKey(true).KeyChar;

                switch (choice)
                {
                    case DisplayConstants.NEW_GAME:
                        var (rows, columns) = renderer.GetGridSize();
                        var singleGame = new GameManager(rows, columns, renderer);
                        await singleGame.StartGame();
                        break;

                    case DisplayConstants.LOAD_GAME:
                        if (gameStateService.SaveFileExists(parallelGames: false))
                        {
                            var saves = gameStateService.GetSaveFiles(parallelGames: false);
                            System.Console.WriteLine(DisplayConstants.AVAILABLE_SAVES_HEADER);
                            for (int i = 0; i < saves.Count; i++)
                            {
                                System.Console.WriteLine(DisplayConstants.SAVE_FORMAT, i + 1, saves[i]);
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
                            System.Console.WriteLine(DisplayConstants.NO_SINGLE_SAVES);
                            System.Console.ReadKey(true);
                        }
                        break;

                    case DisplayConstants.NEW_PARALLEL_GAME:
                        var (parallelRows, parallelColumns) = renderer.GetGridSize();
                        var newParallelGames = new ParallelGameManager(1000, parallelRows, parallelColumns, renderer);
                        await newParallelGames.StartGame();
                        break;

                    case DisplayConstants.LOAD_PARALLEL_GAME:
                        if (gameStateService.SaveFileExists(parallelGames: true))
                        {
                            var saves = gameStateService.GetSaveFiles(parallelGames: true);
                            System.Console.WriteLine(DisplayConstants.AVAILABLE_SAVES_HEADER);
                            for (int i = 0; i < saves.Count; i++)
                            {
                                System.Console.WriteLine(DisplayConstants.SAVE_FORMAT, i + 1, saves[i]);
                            }
                            System.Console.WriteLine(DisplayConstants.SAVE_SELECTION_PROMPT);

                            if (int.TryParse(System.Console.ReadLine(), out int saveChoice) && 
                                saveChoice > 0 && saveChoice <= saves.Count)
                            {
                                var parallelState = gameStateService.LoadParallelGame(saves[saveChoice - 1]);
                                var loadedParallelGames = new ParallelGameManager(parallelState, renderer);
                                await loadedParallelGames.StartGame();
                            }
                        }
                        else
                        {
                            System.Console.WriteLine(DisplayConstants.NO_PARALLEL_SAVES);
                            System.Console.ReadKey(true);
                        }
                        break;

                    case DisplayConstants.EXIT:
                        exitProgram = true;
                        break;

                    default:
                        System.Console.WriteLine(DisplayConstants.INVALID_OPTION);
                        System.Console.ReadKey(true);
                        break;
                }
            }
        }
    }
}
