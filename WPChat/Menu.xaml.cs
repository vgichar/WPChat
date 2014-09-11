using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WPChat
{
    public partial class Menu : PhoneApplicationPage
    {
        public Menu()
        {
            InitializeComponent();

        }

        // on page load
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // clear BackStack
            while (NavigationService.BackStack.Count() > 0)
                NavigationService.RemoveBackEntry();
        }

        private void Friends_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Rooms_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RoomsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Requests_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FriendRequestsPage.xaml", UriKind.RelativeOrAbsolute));
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
    }
}