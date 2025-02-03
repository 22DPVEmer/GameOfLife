using System.Collections.Generic;

namespace GameOfLife.Core.Interfaces
{
    public interface IVisibleGamesManager
    {
        IReadOnlyList<int> GetVisibleGameIds();
        void CycleVisibleGames(int direction);
        void Initialize(int totalGames, int maxVisibleGames);
    }
} 