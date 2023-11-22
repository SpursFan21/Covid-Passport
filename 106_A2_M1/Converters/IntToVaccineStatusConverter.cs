using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace _106_A2_M1.Converters
{
    public class IntToVaccineStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                switch (intValue)
                {
                    case 0:
                        return "Unvaccinated";
                    case 1:
                        return "Partial";
                    case 2:
                        return "Complete";
                    default:
                        // Handle other values if needed
                        return "Unknown";
                }
            }

            // Handle non-integer values if needed
            throw new ArgumentException("Value must be an integer");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If two-way binding is needed, implement this method accordingly
            throw new NotImplementedException();
        }
    }

}
