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
    public partial class DetailsPage : PhoneApplicationPage
    {
        private DataContextType type;
        private string name;

        public DetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            type = NavigationContext.QueryString["Type"] == "User" ? DataContextType.User : DataContextType.Room;
            name = NavigationContext.QueryString["Name"];

            this.tbTitle.Text = name;
            if (type == DataContextType.User)
            {
                llsUsers.Visibility = System.Windows.Visibility.Collapsed;
                llsRooms.Visibility = System.Windows.Visibility.Visible;
                foreach (UserItem ui in App.User.Friends)
                {
                    if (name == ui.Username)
                    {
                        this.tbStats.Text = string.Format("{0} active conversations", ui.Rooms.Count.ToString());
                        this.DataContext = ui;
                        break;
                    }
                }
            }
            else
            {
                foreach (RoomItem ri in App.User.Rooms)
                {
                    if (name == ri.Name)
                    {
                        this.tbStats.Text = string.Format("{0} active users", ri.Users.Count.ToString());
                        this.DataContext = ri;
                        break;
                    }
                }
            }
        }

        void abib_Click(object sender, EventArgs e)
        {
            if (type == DataContextType.User)
            {
                MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your friends list?", name), "Add user", MessageBoxButton.OKCancel);
                if (mbr == MessageBoxResult.OK)
                {
                    //App.User.addFriend(name);
                    Console.WriteLine(name);
                    App.User.friendRequest(name);
                }
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your favorite rooms list?", name), "Add room", MessageBoxButton.OKCancel);
                if (mbr == MessageBoxResult.OK)
                {
                    App.User.addRoom(name);
                }
            }
        }

        private void ApplicationBarIconButton_ClickExit(object sender, EventArgs e)
        {
            App.IsolatedStorageSettings.Save();
            Application.Current.Terminate();
        }

        private void ApplicationBarIconButton_ClickLogout(object sender, EventArgs e)
        {
            App.IsolatedStorageSettings.Remove("Username");
            App.IsolatedStorageSettings.Remove("Password");

            App.User.Logout();

            App.User = new OwnerUserItem();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButton_ClickDelete(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(string.Format("Are you sure you like to delete \"{0}\"", name), string.Format("Delete {0}", type.ToString()), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (type == DataContextType.User)
                {
                    App.User.removeFriend(name);
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    App.User.removeRoom(name);
                    NavigationService.Navigate(new Uri("/RoomsPage.xaml", UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void ApplicationBarIconButton_ClickChat(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void lss_SelectionChanged(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border b = sender as Border;

            if (type == DataContextType.Room)
            {
                MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your friends list?", b.Tag as string), "Add user", MessageBoxButton.OKCancel);

                if (mbr == MessageBoxResult.OK)
                {
                    //App.User.addFriend(b.Tag as string);
                    Console.WriteLine(b.Tag as string);
                    App.User.friendRequest(b.Tag as string);
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
    }
}