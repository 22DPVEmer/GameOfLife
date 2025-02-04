using System.Threading.Tasks;

namespace GameOfLife.Console.Menu.Interfaces
{
    /// <summary>
    /// Interface for loading saved game states.
    /// </summary>
    public interface IGameLoader
    {
        /// <summary>
        /// Loads a saved single game state selected by the user.
        /// </summary>
        Task LoadExistingGame();

        /// <summary>
        /// Loads a saved parallel game state selected by the user.
        /// </summary>
        Task LoadExistingParallelGame();
    }
} 