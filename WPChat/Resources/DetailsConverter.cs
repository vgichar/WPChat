using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPChat.Resources
{
    public class DetailsConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value.ToString();
            var ui = App.User.Friends.FirstOrDefault(x => x.Username == name);
            var ri = App.User.Rooms.FirstOrDefault(x => x.Name == name);
            if (ui != null || ri != null)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
