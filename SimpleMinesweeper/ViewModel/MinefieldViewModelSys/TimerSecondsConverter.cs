using System;
using System.Globalization;
using System.Windows.Data;

namespace SimpleMinesweeper.ViewModel
{
    public class TimerSecondsConverter : IValueConverter
    {

        #region Public methods
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tiks = (int)value;
            if (tiks >= 60000) // 100 * 60
                tiks = 999 * 60 + 59; // "999:59"

            int seconds = tiks % 60;
            int minutes = tiks / 60;
            return string.Format("{0:d2}:{1:d2}", minutes, seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
