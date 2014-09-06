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
                    Console.WriteLine(name);
                    App.User.SendFriendRequest(name);
                }
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your favorite rooms list?", name), "Add room", MessageBoxButton.OKCancel);
                if (mbr == MessageBoxResult.OK)
                {
                    App.User.AddRoom(name);
                }
            }
        }

        private void ApplicationBarIconButton_ClickExit(object sender, EventArgs e)
        {
            App.ExitApp();
        }

        private void ApplicationBarIconButton_ClickLogout(object sender, EventArgs e)
        {
            App.User.Logout();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButton_ClickDelete(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(string.Format("Are you sure you like to delete \"{0}\"", name), string.Format("Delete {0}", type.ToString()), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (type == DataContextType.User)
                {
                    App.User.RemoveFriend(name);
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    App.User.RemoveRoom(name);
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
            string name = b.Tag as string;

            if (type == DataContextType.Room)
            {
                UserItem ui = App.User.Friends.FirstOrDefault(x => x.Username == name);
                if (ui == null)
                {
                    MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your friends list?", name), "Add user", MessageBoxButton.OKCancel);

                    if (mbr == MessageBoxResult.OK)
                    {
                        App.User.SendFriendRequest(name);
                    }
                }
                else
                {
                    NavigationService.Navigate(new Uri(string.Format("/ChatPage.xaml?Name={0}&Type={1}", Uri.EscapeUriString(name), Uri.EscapeUriString(DataContextType.User.ToString())), UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                RoomItem ri = App.User.Rooms.FirstOrDefault(x => x.Name == name);
                if (ri == null)
                {
                    MessageBoxResult mbr = MessageBox.Show(string.Format("Add \"{0}\" to your favorite rooms list?", name), "Add room", MessageBoxButton.OKCancel);

                    if (mbr == MessageBoxResult.OK)
                    {
                        App.User.AddRoom(name);
                    }
                }
                else
                {
                    NavigationService.Navigate(new Uri(string.Format("/ChatPage.xaml?Name={0}&Type={1}", Uri.EscapeUriString(name), Uri.EscapeUriString(DataContextType.Room.ToString())), UriKind.RelativeOrAbsolute));
                }
            }
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