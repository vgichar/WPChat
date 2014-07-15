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

namespace WPChat
{
    public partial class AddPage : PhoneApplicationPage
    {
        private DataContextType type;

        public AddPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            type = NavigationContext.QueryString["Type"] == "User" ? DataContextType.User : DataContextType.Room;

            tbTitle.Text += string.Format(" {0}", type.ToString());

            if (type == DataContextType.Room)
            {
                btCreate.Visibility = System.Windows.Visibility.Visible;
                llsUsers.Visibility = System.Windows.Visibility.Collapsed;
                llsRooms.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ApplicationBarIconButton_ClickLogout(object sender, EventArgs e)
        {
            App.IsolatedStorageSettings.Remove("Username");
            App.IsolatedStorageSettings.Remove("Password");

            App.User.Logout();

            App.User = new OwnerUserItem();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButton_ClickExit(object sender, EventArgs e)
        {
            App.IsolatedStorageSettings.Save();
            Application.Current.Terminate();
        }

        private void ApplicationBarIconButton_ClickFriends(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButton_ClickRooms(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RoomsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void tbSearchInput_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) 
            {
                this.Focus();
            }

            var name = tbSearchInput.Text;
            if (type == DataContextType.User)
            {
                llsUsers.ItemsSource = App.User.getUsersByNameStart(name).OrderBy(x => x.Status).ThenBy(x=>x.Username).ToList();
            }
            else
            {
                llsRooms.ItemsSource = App.User.getRoomsByNameStart(name).OrderBy(x => x.Name).ToList();;
            }
        }

        private void lss_SelectionChanged(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border b = sender as Border;

            if (type == DataContextType.User)
            {
                MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your friends list?", b.Tag as string), "Add user", MessageBoxButton.OKCancel);

                if (mbr == MessageBoxResult.OK)
                {
                    App.User.addFriend(b.Tag as string);
                }
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your favorite rooms list?", b.Tag as string), "Add room", MessageBoxButton.OKCancel);

                if (mbr == MessageBoxResult.OK)
                {
                    App.User.addRoom(b.Tag as string);
                }
            }
        }

        private void btCreate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show(string.Format("Are you sure you want to create new room with name \"{0}\"", tbSearchInput.Text), "Create room?", MessageBoxButton.OKCancel);

            if (mbr == MessageBoxResult.OK) 
            {
                pbCreateRoom.Visibility = System.Windows.Visibility.Visible;
                App.User.CreateRoom(tbSearchInput.Text, ()=>{
                    pbCreateRoom.Visibility = System.Windows.Visibility.Collapsed;
                });
                ApplicationBarIconButton_ClickRooms(sender, e);
            }
        }
    }
}