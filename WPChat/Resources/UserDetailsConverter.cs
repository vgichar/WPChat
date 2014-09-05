using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPChat.Resources
{
    class UserDetailsConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string username = value.ToString();
            ViewModels.UserItem ui = App.User.Friends.FirstOrDefault(x => x.Username == username);
            if (ui != null)
            {
                return "Visible";
            }
            else
                return "Collapsed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
