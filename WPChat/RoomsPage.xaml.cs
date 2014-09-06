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
using System.Collections.ObjectModel;

namespace WPChat
{
    public partial class RoomsPage : PhoneApplicationPage
    {
        // Constructor
        public RoomsPage()
        {
            InitializeComponent();

            // Set DataContext as OwnerUser
            this.DataContext = App.User;
        }

        // on page load
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // clear BackStack
            while (NavigationService.BackStack.Count() > 0)
                NavigationService.RemoveBackEntry();
        }

        // On list item click => open chat room
        private void llsRooms_SelectionChanged(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string roomname = (sender as Border).Tag as string;
            NavigationService.Navigate(new Uri(string.Format("/ChatPage.xaml?Name={0}&Type={1}", Uri.EscapeUriString(roomname), Uri.EscapeUriString(DataContextType.Room.ToString())), UriKind.RelativeOrAbsolute));
        }

        // ApplicationBar Logout
        private void ApplicationBarIconButton_ClickLogout(object sender, EventArgs e)
        {
            App.User.Logout();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        // ApplicationBar Exit
        private void ApplicationBarIconButton_ClickExit(object sender, EventArgs e)
        {
            App.ExitApp();
        }

        // ApplicationBar Add Room
        private void ApplicationBarIconButton_ClickAddRoom(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/AddPage.xaml?Type={0}", Uri.EscapeUriString(DataContextType.Room.ToString())), UriKind.RelativeOrAbsolute));
        }

        // ApplicationBar GoTo friends list
        private void ApplicationBarIconButton_ClickFriends(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPicker lp = sender as ListPicker;


            foreach (StatusIndicator si in Enum.GetValues(typeof(StatusIndicator)))
            {
                if (si.ToString() == (lp.SelectedItem as ListPickerItem).Tag.ToString())
                {
                    App.User.ChangeStatus(si);
                }
            }
        }
    }
}