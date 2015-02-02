using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace App1.Converters
{
    public class DecimalPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var doubleValue = (double)value; 
            string stringValue = doubleValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            string[] stringValueParts = stringValue.Split('.');
            string decimalValue = stringValueParts[1];
            return decimalValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
