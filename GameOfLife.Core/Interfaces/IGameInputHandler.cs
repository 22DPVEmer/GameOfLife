using System;
using System.Threading.Tasks;

namespace GameOfLife.Core.Interfaces
{
    public interface IGameInputHandler
    {
        Task HandleInput(ConsoleKeyInfo keyInfo);
        Task StartListening();
        void StopListening();
        bool IsListening { get; }
    }
} 