﻿using System;
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
        
        public int MinesNearby { get; set; }

        public void Open()
        {
            if (minefield.State == FieldState.GameOver)
                return;

            if (State == CellState.NotOpened)
            {
                if (mined)
                    State = CellState.Explosion;
                else
                {
                    MinesNearby = minefield.GetCellMineNearbyCount(this);
                    State = CellState.Opened;
                }
                
            }
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

        public override bool Equals(object obj)
        {
            var cell = obj as Cell;
            return cell != null &&
                   Mined == cell.Mined &&
                   CoordX == cell.CoordX &&
                   CoordY == cell.CoordY;
        }

        public override int GetHashCode()
        {
            var hashCode = 280058429;
            hashCode = hashCode * -1521134295 + Mined.GetHashCode();
            hashCode = hashCode * -1521134295 + CoordX.GetHashCode();
            hashCode = hashCode * -1521134295 + CoordY.GetHashCode();
            return hashCode;
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
