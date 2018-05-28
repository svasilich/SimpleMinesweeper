﻿using System;
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

        event EventHandler OnStateChanged;
        event EventHandler OnFilled;

        List<List<ICell>> Cells { get; }

        ICell GetCellByCoords(int x, int y);
        int GetCellMineNearbyCount(ICell cell);

        void Fill(int height, int length, int mineCount);

        bool CellsStateCanBeChanged { get; }
    }
}
