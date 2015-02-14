using System;
using Windows.UI.Xaml.Data;

namespace KioskApp.Converters
{
    public class BasePriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (Math.Truncate((double)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
