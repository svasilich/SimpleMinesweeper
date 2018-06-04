using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleMinesweeper.Core
{
    public class GameTimer : IGameTimer
    {
        private Timer timer;

        public int Seconds
        {
            get; private set;
        }

        public void Start()
        {
            Seconds = 0;
            timer = new Timer(TimerTick, null, 0, 1000);
        }

        public void Stop()
        {
            timer.Dispose();
        }

        private void TimerTick(object stateInfo)
        {
            ++Seconds;
            OnTimerTick?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnTimerTick;
    }
}
