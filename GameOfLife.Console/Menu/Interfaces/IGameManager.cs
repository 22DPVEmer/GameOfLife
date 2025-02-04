using System.Threading.Tasks;
using GameOfLife.Core.Models;

namespace GameOfLife.Console.Menu.Interfaces
{
    /// <summary>
    /// Interface for game manager instances.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Starts the game.
        /// </summary>
        Task StartGame();

        /// <summary>
        /// Loads a game state.
        /// </summary>
        void LoadState(GameState state);
    }
} 