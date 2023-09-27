using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chess.Resources.Converters
{
    public class TimerDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if(value == null) return null;

            TimeSpan t = TimeSpan.FromMilliseconds(double.Parse(value.ToString()));
            return $"{t.Hours}:{t.Minutes}:{t.Seconds}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
