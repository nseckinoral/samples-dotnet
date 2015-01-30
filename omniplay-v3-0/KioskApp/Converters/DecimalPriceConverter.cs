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
            //var decimalValue = doubleValue-Math.Truncate(doubleValue);
            //var result = Math.Truncate(decimalValue*100);
            return doubleValue.ToString("0:0.#");
            //return result ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
