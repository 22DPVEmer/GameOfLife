using System;
using System.Threading.Tasks;
using GameOfLife.Constants;

namespace GameOfLife
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
                Console.WriteLine($"{DisplayConstants.ERROR_PREFIX}{ex.Message}");
                Console.WriteLine(DisplayConstants.EXIT_PROMPT);
                Console.ReadKey();
            }
        }
    }
}
