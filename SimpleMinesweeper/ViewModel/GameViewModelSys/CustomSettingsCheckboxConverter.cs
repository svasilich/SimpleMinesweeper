using System;
using System.Globalization;
using System.Windows.Data;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.ViewModel
{
    public class CustomSettingsCheckboxConverter : IValueConverter
    {
        #region Public methods
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GameType currentGameType = (GameType)value;
            return currentGameType == GameType.Custom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
