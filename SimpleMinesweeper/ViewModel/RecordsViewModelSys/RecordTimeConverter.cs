using System;
using System.Globalization;
using System.Windows.Data;

namespace SimpleMinesweeper.ViewModel
{
    public class RecordTimeConverter : IValueConverter
    {
        #region Public methods
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? seconds = (int?)value;

            if (seconds.HasValue)
                return seconds.ToString();

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
