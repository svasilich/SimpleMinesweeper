using System;
using System.Collections.Generic;
using System.ComponentModel;
using SimpleMinesweeper.Core.GameSettings;

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
        int Width { get; }
        int MinesCount { get; }
        int FlagsCount { get; }
        List<List<ICell>> Cells { get; }

        event EventHandler OnStateChanged;
        event EventHandler OnFilled;
        event EventHandler OnFlagsCountChanged;

        void SetGameSettings(SettingsItem settings);

        ICell GetCellByCoords(int x, int y);
        int GetCellMineNearbyCount(ICell cell);

        void Fill(int height, int length, int mineCount);
        void Fill();

        bool CellsStateCanBeChanged { get; }
    }
}
