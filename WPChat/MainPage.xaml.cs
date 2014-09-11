using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPChat.Resources;
using System.IO.IsolatedStorage;
using Microsoft.AspNet.SignalR.Client;
using WPChat.ViewModels;
using System.Collections.ObjectModel;

namespace WPChat
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
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
/*            while (NavigationService.BackStack.Count() > 0)
                NavigationService.RemoveBackEntry();
 */
        }

        // On list item click => open chat room
        private void llsFriends_SelectionChanged(object sender, EventArgs e)
        {
            string username = (sender as Border).Tag as string;

            NavigationService.Navigate(new Uri(string.Format("/ChatPage.xaml?Name={0}&Type={1}", Uri.EscapeUriString(username), Uri.EscapeUriString(DataContextType.User.ToString())), UriKind.RelativeOrAbsolute));
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
        private void ApplicationBarIconButton_ClickAddFriend(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/AddPage.xaml?Type={0}", Uri.EscapeUriString(DataContextType.User.ToString())), UriKind.RelativeOrAbsolute));
        }

        // ApplicationBar GoTo rooms list
        private void ApplicationBarIconButton_ClickRooms(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RoomsPage.xaml", UriKind.RelativeOrAbsolute));
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

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FriendRequestsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            lpStatus.SelectedIndex = new ActiveStatusConverter().exec();
        }
    }
}