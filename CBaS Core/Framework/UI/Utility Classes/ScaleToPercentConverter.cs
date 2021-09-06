using System;
using System.Globalization;
using System.Windows.Data;

namespace CBaSCore.Framework.UI.Utility_Classes
{
    public class ScaleToPercentConverter : IValueConverter
    {
        /// <summary>
        /// Convert a fraction to a percentage.
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Round to an integer value whilst converting.
            return (double)(int)((double)value * 100.0);
        }

        /// <summary>
        /// Convert a percentage back to a fraction.
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 100.0;
        }
    }
}