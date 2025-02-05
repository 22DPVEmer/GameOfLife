using System.Collections.Generic;
using GameOfLife.Core.Interfaces;

namespace GameOfLife.Core.Services
{
    public class VisibleGamesManager : IVisibleGamesManager
    {
        private readonly List<int> _visibleGameIds;
        private int _totalGames;
        private int _maxVisibleGames;

        public VisibleGamesManager()
        {
            _visibleGameIds = new List<int>();
        }

        public void Initialize(int totalGames, int maxVisibleGames)
        {
            _totalGames = totalGames;
            _maxVisibleGames = maxVisibleGames;
            _visibleGameIds.Clear();

            for (int i = 0; i < maxVisibleGames && i < totalGames; i++)
            {
                _visibleGameIds.Add(i);
            }
        }

        public IReadOnlyList<int> GetVisibleGameIds() => _visibleGameIds;

        public void CycleVisibleGames(int direction)
        {
            int firstVisibleId = _visibleGameIds[0];
            int newFirstId = (firstVisibleId + direction + _totalGames) % _totalGames;
            if (newFirstId < 0) newFirstId += _totalGames;

            _visibleGameIds.Clear();
            for (int i = 0; i < _maxVisibleGames; i++)
            {
                int nextId = (newFirstId + i) % _totalGames;
                _visibleGameIds.Add(nextId);
            }
        }
    }
} 