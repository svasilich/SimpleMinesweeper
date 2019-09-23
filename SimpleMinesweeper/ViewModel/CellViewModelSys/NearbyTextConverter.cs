using System;
using System.Globalization;
using System.Windows.Data;

namespace SimpleMinesweeper.ViewModel
{
    public class NearbyTextConverter : IValueConverter
    {
        #region Public methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nearby = (int)value;

            if (nearby > 0 && nearby <= 8)
                return nearby;

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
