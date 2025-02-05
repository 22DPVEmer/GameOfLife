using System.Threading.Tasks;
using GameOfLife.Core.Models;

namespace GameOfLife.Console.Menu.Interfaces
{
    /// <summary>
    /// Interface for creating game manager instances.
    /// </summary>
    public interface IGameManagerFactory
    {
        /// <summary>
        /// Creates a new single game instance.
        /// </summary>
        Task<IGameManager> CreateNewGame(int rows, int columns);

        /// <summary>
        /// Creates a new parallel game instance.
        /// </summary>
        Task<IGameManager> CreateNewParallelGame(int rows, int columns);

        /// <summary>
        /// Creates a game manager from a saved state.
        /// </summary>
        Task<IGameManager> CreateFromState(GameState state, bool isParallel);
    }
} 