using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleMinesweeper.Core
{
    public enum FieldState
    {
        NotStarted,
        InGame,
        GameOver
    }

    public interface IMinefield
    {
        FieldState State { get; }

        event EventHandler OnStateChanged;

        List<List<ICell>> Cells { get; }

        ICell GetCellByCoords(int x, int y);

        void Fill(int hight, int length, int mineCount);
    }
}
