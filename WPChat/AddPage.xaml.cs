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
    public partial class AddPage : PhoneApplicationPage
    {
        private DataContextType type;

        public AddPage()
        {
            InitializeComponent();

            // Set DataContext as OwnerUser
            this.DataContext = App.User;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            type = NavigationContext.QueryString["Type"] == "User" ? DataContextType.User : DataContextType.Room;

            tbTitle.Text = string.Format("Add {0}", type.ToString());

            if (type == DataContextType.Room)
            {
                btCreate.Visibility = System.Windows.Visibility.Visible;
                llsUsers.Visibility = System.Windows.Visibility.Collapsed;
                llsRooms.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ApplicationBarIconButton_ClickLogout(object sender, EventArgs e)
        {
            App.User.Logout();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButton_ClickExit(object sender, EventArgs e)
        {
            App.ExitApp();
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
                List<UserItem> list = new List<UserItem>();
                App.User.GetUsersByNameStart(name, list, () => {
                    llsUsers.ItemsSource = list;
                });
            }
            else
            {
                List<RoomItem> list = new List<RoomItem>();
                App.User.GetRoomsByNameStart(name, list, () =>
                {
                    llsRooms.ItemsSource = list;
                });
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
                    App.User.SendFriendRequest(b.Tag as string);
                }
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show(string.Format("Enter room: \"{0}\" ?", b.Tag as string), "Add room", MessageBoxButton.OKCancel);

                if (mbr == MessageBoxResult.OK)
                {
                    App.User.AddRoom(b.Tag as string, () =>
                    {
                        NavigationService.Navigate(new Uri(string.Format("/ChatPage.xaml?Name={0}&Type={1}", Uri.EscapeUriString(b.Tag as string), Uri.EscapeUriString(DataContextType.Room.ToString())), UriKind.RelativeOrAbsolute));
                    });
                }
            }
        }

        private void btCreate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show(string.Format("Are you sure you want to create new room with name \"{0}\"", tbSearchInput.Text), "Create room?", MessageBoxButton.OKCancel);

            if (mbr == MessageBoxResult.OK)
            {
                pbCreateRoom.Visibility = System.Windows.Visibility.Visible;
                App.User.CreateRoom(tbSearchInput.Text, () =>
                {
                    pbCreateRoom.Visibility = System.Windows.Visibility.Collapsed;
                });
                ApplicationBarIconButton_ClickRooms(sender, e);
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