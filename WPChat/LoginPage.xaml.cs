using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WPChat
{
    public partial class LoginPage : PhoneApplicationPage
    {
        // Constructor
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // auto fill data if possible
            if (!string.IsNullOrEmpty(App.User.Username) && !string.IsNullOrEmpty(App.User.Password))
            {
                tbUsername.Text = App.User.Username;
                tbPassword.Password = App.User.Password;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            
            // disable navigating away from page if not logged in
            if (!App.User.IsLoggedIn && e.Uri != new Uri("/RegisterPage.xaml", UriKind.RelativeOrAbsolute))
            {
                e.Cancel = true;
            }
        }

        // Login user
        private void btnLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // show loading bar
            pbLogin.Visibility = System.Windows.Visibility.Visible;

            var username = tbUsername.Text;
            var password = tbPassword.Password;
            var remember = ckbRemember.IsChecked;

            // try login in on server
            App.User.Login(username, password, () =>
            {
                // login successful
                if (App.User.IsLoggedIn)
                {
                    if (remember == true)
                    {
                        App.IsolatedStorageSettings["Username"] = username;
                        App.IsolatedStorageSettings["Password"] = password;
                    }

                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                }
                // login failed
                else
                {
                    MessageBox.Show("Invalid username or password\nPlease try again!", "Login Failed", MessageBoxButton.OK);
                }

                // hide loading
                pbLogin.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            // Call applicationBar Exit
            ApplicationBarIconButton_ClickExit(new object(), EventArgs.Empty);
        }

        // ApplicationBar Exit
        private void ApplicationBarIconButton_ClickExit(object sender, EventArgs e)
        {
            App.ExitApp(false);
        }

        // ApplicationBar Register
        private void ApplicationBarIconButton_ClickRegister(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.RelativeOrAbsolute));
        }

        // Login user on enter - name
        private void tbUsername_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnLogin_Tap(sender, new System.Windows.Input.GestureEventArgs());
            }
        }

        // Login user on enter - password
        private void tbPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnLogin_Tap(sender, new System.Windows.Input.GestureEventArgs());
            }
        }
    }
}