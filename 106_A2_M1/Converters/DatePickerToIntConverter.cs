using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _106_A2_M1.Converters
{
    public class DatePickerToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                return (int)((DateTime)value).Ticks; // Convert DateTime to integer (ticks)
            }

            return null; // Return null for invalid input
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return new DateTime((long)value); // Convert integer (ticks) to DateTime
            }

            return null; // Return null for invalid input
        }
    }
}
