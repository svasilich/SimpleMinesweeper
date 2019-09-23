using System;
using System.Globalization;
using System.Windows.Data;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;

namespace SimpleMinesweeper.ViewModel
{
    public class CustomGameTypeCommandConverter : IMultiValueConverter
    {
        #region Public methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            SettingsItem settings = new SettingsItem();
            settings.Type = GameType.Custom;
            if (int.TryParse(values[0].ToString(), out int width))
                settings.Width = width;

            if (int.TryParse(values[1].ToString(), out int height))
                settings.Height = height;

            if (int.TryParse(values[2].ToString(), out int minesCount))
                settings.MineCount = minesCount;

            return settings;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
