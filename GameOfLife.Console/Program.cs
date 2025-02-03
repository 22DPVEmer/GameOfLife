using System;
using System.Threading.Tasks;
using GameOfLife.Core.Constants;

namespace GameOfLife.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var gameManager = await GameManager.CreateGame();
                await gameManager.StartGame();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{DisplayConstants.ERROR_PREFIX}{ex.Message}");
                System.Console.WriteLine(DisplayConstants.EXIT_PROMPT);
                System.Console.ReadKey();
            }
        }
    }
}
