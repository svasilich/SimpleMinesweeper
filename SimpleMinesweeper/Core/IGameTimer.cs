using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public interface IGameTimer
    {
        int Seconds { get; }

        void Start();

        void Stop();

        event EventHandler OnTimerTick;
    }
}
