using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SimpleMinesweeper.ViewModel
{
    public class ValidErrorMessageVisibilityConverter : IValueConverter
    {
        #region Public methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string errText = (string)value;
            if (string.IsNullOrWhiteSpace(errText))
                return Visibility.Hidden;
            else
                return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
