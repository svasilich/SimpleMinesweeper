using System;
using System.Threading;

namespace SimpleMinesweeper.Core
{
    public class GameTimer : IGameTimer
    {
        #region Fields

        private Timer timer;

        private int seconds;

        #endregion

        #region Properties

        public int Seconds
        {
            get { return seconds; }
            private set
            {
                seconds = value;
                OnTimerTick?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Events

        public event EventHandler OnTimerTick;

        #endregion

        #region Event handlers

        private void TimerTick(object stateInfo)
        {
            ++Seconds;
        }

        #endregion

        #region Timer logic
        public void Start()
        {
            Seconds = 0;
            timer = new Timer(TimerTick, null, 0, 1000);
        }

        public void Stop()
        {
            timer?.Dispose();
        }

        public void Reset()
        {
            Stop();
            Seconds = 0;
        }

        #endregion
    }
}
