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
        BlownUpped,
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

        void Open();
        void SetFlag();
        
        event EventHandler<CellChangeStateEventArgs> OnStateChanged;
        event EventHandler OnMinedChanged;
    }

    public class CellChangeStateEventArgs : EventArgs
    {
        private CellState newState;

        public CellChangeStateEventArgs(CellState newState)
        {
            this.newState = newState;
        }

        public CellState NewState => newState;
    }
}
