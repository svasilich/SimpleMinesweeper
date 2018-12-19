using System;
using System.Collections.Generic;

namespace SimpleMinesweeper.Core.GameSettings
{
    public interface ISettingsManager
    {
        List<SettingsItem> AvailableGameTypes { get; }
        SettingsItem CurrentSettings { get; }

        event EventHandler OnCurrentGameChanged;

        SettingsItem GetItemByType(GameType gameType);
        void Load(string fileName);
        void Save(string fileName);
        void SelectGameType(GameType gameType);
        void SetCustomSize(int height, int width, int mineCount);
    }
}