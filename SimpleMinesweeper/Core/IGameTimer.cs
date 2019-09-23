using System;

namespace SimpleMinesweeper.Core
{
    public interface IGameTimer
    {
        int Seconds { get; }

        void Start();

        void Stop();

        void Reset();

        event EventHandler OnTimerTick;
    }
}
