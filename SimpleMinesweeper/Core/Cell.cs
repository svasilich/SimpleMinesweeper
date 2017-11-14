using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    class Cell : ICell
    {
        private const int MaxNearBy = 8;

        public CellState State { get; set; }

        public bool Mined { get; set; }

        public int CoordX { get; private set; }

        public int CoordY { get; private set; }

        private int minesNearby;
        public int MinesNearby
        {
            get => minesNearby;
            set
            {
                if (value >= 0 && value <= MaxNearBy)
                    minesNearby = value;
            }
        }

        public void Open()
        {
            OnOpen?.Invoke(this, EventArgs.Empty);
        }

        public void SetFlag()
        {
            OnSetFlag?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnOpen;
        public event EventHandler OnSetFlag;

        public Cell(int x, int y)
        {
            CoordX = x;
            CoordY = y;
            State = CellState.NoOpened;
        }
    }
}
