using GameOfLife.Core.Models;
using System.Collections.Generic;

namespace GameOfLife.Core.Interfaces
{
    /// <summary>
    /// Composes all game state management capabilities
    /// </summary>
    public interface IParallelGameStateManager : IGameInitializer, IGameStateReader, IGameStateUpdater
    {
        // This interface is now just a composition of more focused interfaces
    }
} 