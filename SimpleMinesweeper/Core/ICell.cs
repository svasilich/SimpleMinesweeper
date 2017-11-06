﻿using System;
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
        Flagged
    }

    public interface ICell
    {
        CellState State { get; set; }
        bool Mined { get; set; }
        int CoordX { get; }
        int CoordY { get; }

        ICellStrategy LeftClickStrategy { get; set; }
        ICellStrategy RightClickStrategy { get; set; }

        event EventHandler OnLeftClick;
        event EventHandler OnRihtClick;
    }
}
