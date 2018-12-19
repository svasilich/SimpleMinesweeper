using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System;

namespace SimpleMinesweeper.Core.GameSettings
{
    public class SettingsManager : ISettingsManager
    {
        #region Members
        private SettingsItem currentSettings;
        #endregion

        #region Properties
        public List<SettingsItem> AvailableGameTypes { get; }

        public SettingsItem CurrentSettings
        {
            get => currentSettings;
            private set
            {
                currentSettings = value;
                OnCurrentGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        #endregion

        #region Constructor

        public SettingsManager()
        {
            AvailableGameTypes = new List<SettingsItem>()
            {
                new SettingsItem() {Type = GameType.Newbie, Height = 9, Width = 9, MineCount = 10},
                new SettingsItem() {Type = GameType.Advanced, Height = 16, Width = 16, MineCount = 40},
                new SettingsItem() {Type = GameType.Professional, Height = 16, Width = 30, MineCount = 99},
                new SettingsItem() {Type = GameType.Custom}
            };

            CurrentSettings = AvailableGameTypes.First();
        }

        #endregion

        #region Events
        public event EventHandler OnCurrentGameChanged;
        #endregion

        #region Set and get settings methods.
        public void SelectGameType(GameType gameType)
        {
            CurrentSettings = GetItemByType(gameType);
        }

        public void SetCustomSize(int height, int width, int mineCount)
        {
            var custom = GetItemByType(GameType.Custom);
            custom.Height = height;
            custom.Width = width;
            custom.MineCount = mineCount;

            if (CurrentSettings.Type == GameType.Custom)
                OnCurrentGameChanged?.Invoke(this, EventArgs.Empty);
        }

        public SettingsItem GetItemByType(GameType gameType)
        {
            return AvailableGameTypes.Find(x => x.Type == gameType);
        }
        #endregion

        #region Save/load logic

        public void Save(string fileName)
        {
            using (Stream settingsFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SettingsItem custom = GetItemByType(GameType.Custom);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveSettingsData));
                xmlSerializer.Serialize(settingsFile,
                    new SaveSettingsData()
                    {
                        Selected = CurrentSettings.Type,
                        CustomHeight = custom.Height,
                        CustomWidth = custom.Width,
                        CustomMineCount = custom.MineCount
                    });
            }
        }

        public void Load(string fileName)
        {
            using (Stream settingsFile = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveSettingsData));
                SaveSettingsData data = (SaveSettingsData)xmlSerializer.Deserialize(settingsFile);

                SetCustomSize(data.CustomHeight, data.CustomWidth, data.CustomMineCount);
                SelectGameType(data.Selected);
            }
        }

        public class SaveSettingsData
        {
            public GameType Selected { get; set; }
            public int CustomHeight { get; set; }
            public int CustomWidth { get; set; }
            public int CustomMineCount { get; set; }
        }

        #endregion
        
    }
}
