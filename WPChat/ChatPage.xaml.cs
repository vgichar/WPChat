﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPChat.ViewModels;
using System.Windows.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace WPChat
{
    public partial class ChatPage : PhoneApplicationPage
    {
        private DataContextType type;
        private string name;

        public ChatPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            type = NavigationContext.QueryString["Type"] == "User" ? DataContextType.User : DataContextType.Room;
            name = NavigationContext.QueryString["Name"];

            if (App.UnseenMessages.Contains(name))
            {
                App.UnseenMessages.Remove(name);
            }

            this.tbTitle.Text = name;
            if (type == DataContextType.Room)
            {
                btnStatusIndicator.Visibility = System.Windows.Visibility.Collapsed;
                foreach (RoomItem ri in App.User.Rooms) 
                {
                    if (ri.Name == name) 
                    {
                        DataContext = ri;
                        tbStats.Text = string.Format("{0} active users", ri.Users.Count.ToString());
                        break;
                    }
                }
            }
            else
            {
                foreach (UserItem ui in App.User.Friends)
                {
                    if (ui.Username == name)
                    {
                        DataContext = ui;
                        tbStats.Text = string.Format("{0} active conversations", ui.Rooms.Count.ToString());
                        break;
                    }
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (llsMessages.ItemsSource.Count > 0)
                llsMessages.ScrollTo(llsMessages.ItemsSource[llsMessages.ItemsSource.Count - 1]);
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (tbMessage.MaxLength - tbMessage.Text.Length >= 0)
            {
                tbRemainingChar.Text = string.Format("{0} remaining characters", tbMessage.MaxLength - tbMessage.Text.Length);
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                sendButton_Tap(sender, EventArgs.Empty);
            }
        }

        private void sendButton_Tap(object sender, EventArgs e)
        {
            App.User.SendMessage(new MessageItem()
            {
                From = App.User.Username,
                To = tbTitle.Text,
                Text = tbMessage.Text,
                Type = type,
            });

            tbMessage.Text = "";
            tbMessage.Focus();

            tbRemainingChar.Text = string.Format("{0} remaining characters", tbMessage.MaxLength);
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

        private void ApplicationBarIconButton_ClickDetails(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/DetailsPage.xaml?Type={0}&Name={1}", Uri.EscapeUriString(type.ToString()), Uri.EscapeUriString(name)), UriKind.RelativeOrAbsolute));
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

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            PhoneApplicationPage_Loaded(sender, e);
        }

        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPicker lp = sender as ListPicker;


            foreach (StatusIndicator si in Enum.GetValues(typeof(StatusIndicator)))
            {
                if ((lp.SelectedItem as ListPickerItem)!=null && si.ToString() == (lp.SelectedItem as ListPickerItem).Tag.ToString())
                {
                    App.User.ChangeStatus(si);
                }
            }
        }
    }
}