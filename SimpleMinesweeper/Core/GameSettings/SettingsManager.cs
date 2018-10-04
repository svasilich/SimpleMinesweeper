using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace SimpleMinesweeper.Core.GameSettings
{
    public class SettingsManager 
    {
        #region Properties
        public List<SettingsItem> AvailableGameTypes { get; }

        public SettingsItem Current { get; private set; }
        
        #endregion

        #region ctor

        public SettingsManager()
        {
            AvailableGameTypes = new List<SettingsItem>()
            {
                new SettingsItem() {Type = GameType.Newbie, Height = 9, Width = 9, MineCount = 10},
                new SettingsItem() {Type = GameType.Advanced, Height = 16, Width = 16, MineCount = 40},
                new SettingsItem() {Type = GameType.Professional, Height = 16, Width = 30, MineCount = 99},
                new SettingsItem() {Type = GameType.Custom}
            };

            Current = AvailableGameTypes.First();
        }

        #endregion

        public void SelectGameType(GameType gameType)
        {
            Current = GetItemByType(gameType);
        }

        public void SetCustomSize(int height, int width, int mineCount)
        {
            var custom = GetItemByType(GameType.Custom);
            custom.Height = height;
            custom.Width = width;
            custom.MineCount = mineCount;
        }

        public SettingsItem GetItemByType(GameType gameType)
        {
            return AvailableGameTypes.Find(x => x.Type == gameType);
        }

        public void Save(string fileName)
        {
            using (Stream settingsFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SettingsItem custom = GetItemByType(GameType.Custom);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveSettingsData));
                xmlSerializer.Serialize(settingsFile, 
                    new SaveSettingsData()
                    {
                        Selected = Current.Type,
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

        #region Subtypes

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
