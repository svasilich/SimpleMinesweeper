using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public class Cell : ICell
    {
        private const int MaxNearBy = 8;
        private IMinefield minefield;

        private CellState state;
        public CellState State {
            get => state;
            set
            {
                state = value;
                OnStateChanged?.Invoke(this, new CellChangeStateEventArgs(state));
            }
        }

        private bool mined;
        public bool Mined
        {
            get => mined;
            set
            {
                mined = value;
                OnMinedChanged?.Invoke(this, ResolveEventArgs.Empty);
            }
        }

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
            if (minefield.State == FieldState.GameOver)
                return;

            if (State == CellState.NotOpened)
                State = (mined ? 
                    CellState.BlownUpped : 
                    CellState.Opened);
        }

        public void SetFlag()
        {
            if (minefield.State == FieldState.GameOver)
                return;

            if (State == CellState.NotOpened)
                State = CellState.Flagged;
            else if (State== CellState.Flagged)
                State = CellState.NotOpened;
        }
        
        public event EventHandler<CellChangeStateEventArgs> OnStateChanged;
        public event EventHandler OnMinedChanged;

        public Cell(IMinefield field, int x, int y)
        {
            this.minefield = field;
            CoordX = x;
            CoordY = y;
            State = CellState.NotOpened;
        }
    }
}
