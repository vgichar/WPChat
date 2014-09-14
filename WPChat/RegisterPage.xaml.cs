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
    public partial class RegisterPage : PhoneApplicationPage
    {
        // Constructor
        public RegisterPage()
        {
            InitializeComponent();

            DataContext = App.User;
        }

        // ApplicationBar Login
        private void ApplicationBarIconButton_ClickLogin(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        // Register user
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            // show loading bar
            pbRegister.Visibility = System.Windows.Visibility.Visible;

            var username = tbUsername.Text;
            var password = tbPassword.Password;

            // try registering on server
            App.User.Register(username, password, () => {
                // register successful
                if (App.User.Username == App.User.Password && App.User.Username == "")
                {
                    MessageBox.Show("Username already taken\nPlease try again!", "Registration Failed", MessageBoxButton.OK);
                }
                // register failed
                else
                {
                    NavigationService.GoBack();
                }

                // hide loading
                pbRegister.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        // ApplicationBar Exit
        private void ApplicationBarIconButton_ClickExit(object sender, EventArgs e)
        {
            App.ExitApp(false);
        }

        // Register user on enter - name
        private void tbUsername_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) {
                btnRegister.Focus();
                registerButton_Click(sender, new RoutedEventArgs());
            }
        }

        // Register user on enter - password
        private void tbPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnRegister.Focus();
                registerButton_Click(sender, new RoutedEventArgs());
            }
        }
    }
}