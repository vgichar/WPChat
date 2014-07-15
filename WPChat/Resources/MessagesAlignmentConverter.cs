using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WPChat.ViewModels;

namespace WPChat.Resources
{
    public class MessagesAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string from = value as string;

            if (from == App.User.Username)
            {
                return HorizontalAlignment.Right;
            }
            else 
            {
                return HorizontalAlignment.Left;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
