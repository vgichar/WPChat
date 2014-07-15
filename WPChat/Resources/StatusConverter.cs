using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPChat.Resources
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string status = value.ToString();
            if (status == "Online")
            {
                return new SolidColorBrush(Colors.Green);
            }
            if (status == "Away")
            {
                return new SolidColorBrush(Colors.Orange);
            }
            if (status == "Busy")
            {
                return new SolidColorBrush(Colors.Red);
            }
            if (status == "Offline")
            {
                return new SolidColorBrush(Colors.LightGray);
            }
            return new SolidColorBrush(Colors.Purple);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
