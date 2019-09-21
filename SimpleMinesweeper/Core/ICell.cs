using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public enum CellState
    {
        NotOpened,
        Opened,
        Explosion,
        Flagged,
        WrongFlag,
        NoFindedMine
    }

    public interface ICell
    {
        CellState State { get; set; }
        bool Mined { get; set; }
        int CoordX { get; }
        int CoordY { get; }
        int MinesNearby { get; set; }

        IMinefield Owner { get; }

        void Open();
        void SetFlag();
        
        event EventHandler<CellChangeStateEventArgs> OnStateChanged;
        event EventHandler OnMinedChanged;
    }

    public class CellChangeStateEventArgs : EventArgs
    {
        public CellState NewState { get; private set; }
        public CellState OldState { get; private set; }

        public CellChangeStateEventArgs(CellState oldState, CellState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }
}
