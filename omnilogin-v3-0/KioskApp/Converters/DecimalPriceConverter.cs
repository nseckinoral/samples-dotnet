﻿using System;
using Windows.UI.Xaml.Data;

namespace KioskApp.Converters
{
    public class DecimalPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double price = (double)value;
            string stringValue = price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            string[] priceValueParts = stringValue.Split('.');
            return priceValueParts[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
