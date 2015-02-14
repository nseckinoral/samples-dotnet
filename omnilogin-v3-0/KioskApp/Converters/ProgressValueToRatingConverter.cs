using System;
using Windows.UI.Xaml.Data;

namespace KioskApp.Converters
{
    public class ProgressValueToRatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)value * 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
