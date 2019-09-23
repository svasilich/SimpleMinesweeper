using System;
using System.Collections.Generic;
using SimpleMinesweeper.Core.GameSettings;

namespace SimpleMinesweeper.Core
{
    public interface IMinefield
    {
        FieldState State { get; }

        int Height { get; }

        int Width { get; }

        int MinesCount { get; }

        int FlagsCount { get; }

        List<List<ICell>> Cells { get; }

        bool CellsStateCanBeChanged { get; }

        void SetGameSettings(SettingsItem settings);

        ICell GetCellByCoords(int x, int y);

        int GetCellMineNearbyCount(ICell cell);
        
        void Fill();
        
        event EventHandler OnStateChanged;

        event EventHandler OnFilled;

        event EventHandler OnFlagsCountChanged;
    }
}
