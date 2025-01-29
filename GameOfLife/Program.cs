using System;
using System.Threading.Tasks;

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
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
