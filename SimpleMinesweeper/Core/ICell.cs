using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public enum CellState
    {
        NoOpened,
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

        event EventHandler OnOpen;
        event EventHandler OnSetFlag;
    }
}
