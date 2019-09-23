namespace SimpleMinesweeper.Core.GameSettings
{
    public class SettingsItem
    {

        #region Properties

        public GameType Type { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int MineCount { get; set; }

        #endregion

        #region Constructor

        public SettingsItem()
        {
            Height = SettingsHelper.MinHeight;
            Width = SettingsHelper.MinWidth;
            MineCount = SettingsHelper.MinMineCount;
        }

        #endregion

    }
}
