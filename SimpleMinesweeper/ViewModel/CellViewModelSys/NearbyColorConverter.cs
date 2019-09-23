using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SimpleMinesweeper.ViewModel
{
    public class NearbyColorConverter : IValueConverter
    {
        #region Public methods
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nearby = (int)value;

            Color textColor;
            switch (nearby)
            {
                case 1:
                    textColor = Colors.Blue;
                    break;
                case 2:
                    textColor = Colors.Green;
                    break;
                case 3:
                    textColor = Colors.Red;
                    break;
                case 4:
                    textColor = Colors.DarkBlue;
                    break;
                case 5:
                    textColor = Colors.Brown;
                    break;
                case 6:
                    textColor = Color.FromRgb(255, 128, 0);
                    break;
                case 7:
                    textColor = Colors.Black;
                    break;
                case 8:
                    textColor = Colors.DarkGray;
                    break;
                default:
                    textColor = Colors.White;
                    break;
            }

            return new SolidColorBrush(textColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }

        #endregion
    }
}
