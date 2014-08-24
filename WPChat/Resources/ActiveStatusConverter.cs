using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPChat.ViewModels;

namespace WPChat.Resources
{
    public class ActiveStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (App.User.Status == StatusIndicator.Online)
            {
                return 0;
            }
            else if (App.User.Status == StatusIndicator.Away)
            {
                return 1;
            }
            else if (App.User.Status == StatusIndicator.Busy)
            {
                return 2;
            }
            else if (App.User.Status == StatusIndicator.Offline)
            {
                return 3;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
