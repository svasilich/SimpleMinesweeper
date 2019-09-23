using System;

namespace SimpleMinesweeper.Core
{
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
}
