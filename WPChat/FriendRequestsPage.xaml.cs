using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPChat.ViewModels;
using WPChat.Resources;

namespace WPChat
{
    public partial class FriendRequests : PhoneApplicationPage
    {
        public FriendRequests()
        {
            InitializeComponent();

            // Set DataContext as OwnerUser
            this.DataContext = App.User;
        }

        private void lss_SelectionChanged(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string name = (sender as Border).Tag as string;
            MessageBoxResult mbr = MessageBox.Show(string.Format("Accept friend request from {0}", name), "Respond to friend request", MessageBoxButton.OKCancel);
            if (mbr == MessageBoxResult.OK)
            {
                App.User.AcceptFriendRequest(name);
                App.User.FriendRequests.Remove(name);
            }
            else
            {
                App.User.DenyFriendRequest(name);
                App.User.FriendRequests.Remove(name);
            }
        }

        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPicker lp = sender as ListPicker;


            foreach (StatusIndicator si in Enum.GetValues(typeof(StatusIndicator)))
            {
                if ((lp.SelectedItem as ListPickerItem) != null && si.ToString() == (lp.SelectedItem as ListPickerItem).Tag.ToString())
                {
                    App.User.ChangeStatus(si);
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            lpStatus.SelectedIndex = new ActiveStatusConverter().exec();
        }
    }
}