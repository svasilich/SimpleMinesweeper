using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleMinesweeper.Core
{
    public enum FieldState
    {
        NotStarted,
        InGame,
        GameOver,
        Win
    }

    public interface IMinefield
    {
        FieldState State { get; }

        int Height { get; }
        int Length { get; }
        int MinesCount { get; }
        int FlagsCount { get; }

        event EventHandler OnStateChanged;
        event EventHandler OnFilled;
        event EventHandler OnFlagsCountChanged;

        List<List<ICell>> Cells { get; }

        ICell GetCellByCoords(int x, int y);
        int GetCellMineNearbyCount(ICell cell);

        void Fill(int height, int length, int mineCount);

        bool CellsStateCanBeChanged { get; }
    }
}
