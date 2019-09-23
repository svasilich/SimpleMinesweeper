using System;
using System.Globalization;
using System.Windows.Data;

namespace SimpleMinesweeper.ViewModel
{
    public class CustomSettingsEnableConverter : IMultiValueConverter
    {

        #region Public methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool customGameCheckboxValue = (bool)values[0];
            if (!customGameCheckboxValue)
                return false;

            string errText = values[1].ToString();
            return string.IsNullOrEmpty(errText);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
