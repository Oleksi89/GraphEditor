﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphApplication.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? "Visible" : "Hidden";
            }

            throw new ArgumentException("Argument value is not bool");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
