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

        private int seconds;
        public int Seconds
        {
            get { return seconds; }
            private set
            {
                seconds = value;
                OnTimerTick?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            Seconds = 0;
            timer = new Timer(TimerTick, null, 0, 1000);
        }

        public void Stop()
        {
            timer?.Dispose();
        }

        private void TimerTick(object stateInfo)
        {
            ++Seconds;    
        }

        public void Reset()
        {
            Stop();
            Seconds = 0;
        }

        public event EventHandler OnTimerTick;
    }
}
