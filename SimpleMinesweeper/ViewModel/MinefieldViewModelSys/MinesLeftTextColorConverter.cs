using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SimpleMinesweeper.ViewModel
{
    public class MinesLeftTextColorConverter : IValueConverter
    {

        #region Public methods
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int left = (int)value;
            if (left >= 0)
                return Colors.YellowGreen;

            return Colors.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
