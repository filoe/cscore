using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CSCoreDemo.Converter
{
    [ValueConversion(typeof(TimeSpan), typeof(long))]
    public class TimeSpanMillisecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (long)((TimeSpan)value).TotalMilliseconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TimeSpan.FromMilliseconds((double)value);
        }
    }
}
