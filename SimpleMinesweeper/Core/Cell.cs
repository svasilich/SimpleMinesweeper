using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    class Cell : ICell
    {
        public CellState State { get; set; }

        public bool Mined { get; set; }

        public int CoordX { get; private set; }

        public int CoordY { get; private set; }

        public ICellStrategy LeftClickStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICellStrategy RightClickStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler OnLeftClick;
        public event EventHandler OnRihtClick;

        public Cell(int x, int y)
        {
            CoordX = x;
            CoordY = y;
        }
    }
}
