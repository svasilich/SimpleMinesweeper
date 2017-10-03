using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    class Cell : ICell
    {
        public CellState State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Mined => throw new NotImplementedException();

        public int CoordX => throw new NotImplementedException();

        public int CoordY => throw new NotImplementedException();

        public ICellStrategy LeftClickStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICellStrategy RightClickStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler OnLeftClick;
        public event EventHandler OnRihtClick;
    }
}
