using System;
using System.Linq;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPChat.Resources;
using System.IO.IsolatedStorage;
using System.IO;
using WPChat.ViewModels;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPChat
{
    public partial class App : Application
    {
        private string _serverUrl = "http://192.168.0.105:8080/";
        private string _hubName = "OwnerUserHub";

        public static OwnerUserItem User = new OwnerUserItem();
        public static IHubProxy Hub;
        public static HubConnection Connection;
        public static Dispatcher Dispatcher = Deployment.Current.Dispatcher;
        public static List<string> UnseenMessages = new List<string>();

        #region autoinit
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        public static IsolatedStorageSettings IsolatedStorageSettings
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings;
            }
        }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Language display initialization
            InitializeHub();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            if (IsolatedStorageSettings.Contains("Username") && IsolatedStorageSettings.Contains("Password"))
            {
                User.Username = IsolatedStorageSettings["Username"] as string;
                User.Password = IsolatedStorageSettings["Password"] as string;
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (IsolatedStorageSettings.Contains("Username") && IsolatedStorageSettings.Contains("Password"))
            {
                User.Username = IsolatedStorageSettings["Username"] as string;
                User.Password = IsolatedStorageSettings["Password"] as string;
            }
        }

        public static async void ExitApp(bool logout = true)
        {
            if (logout)
            {
                await Hub.Invoke("Logout");
            }
            IsolatedStorageSettings.Save();
            Application.Current.Terminate();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Connection.Stop();
            IsolatedStorageSettings.Save();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Connection.Stop();
            IsolatedStorageSettings.Save();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }
        #endregion


        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

        #endregion

        private async void InitializeHub()
        {
            Connection = new HubConnection(_serverUrl);
            Hub = Connection.CreateHubProxy(_hubName);

            Hub.On("ReceiveMessage", (MessageItem mi) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (mi.Type == DataContextType.User)
                    {
                        App.User.Friends.First(x => x.Username == mi.From).Messages.Add(mi);
                    }
                    else
                    {
                        App.User.Rooms.First(x => x.Name == mi.To).Messages.Add(mi);
                    }

                    if (!App.UnseenMessages.Contains(mi.From) && (Application.Current.RootVisual as PhoneApplicationFrame).CurrentSource != new Uri(string.Format("/ChatPage.xaml?Name={0}&Type={1}", Uri.EscapeUriString(mi.From), Uri.EscapeUriString(mi.Type.ToString())), UriKind.RelativeOrAbsolute))
                    {
                        MessageBoxResult mbr = MessageBox.Show(string.Format("{0} sent you a message.\nVisit chat?", mi.From), "You have new message!", MessageBoxButton.OKCancel);
                        if (mbr == MessageBoxResult.OK)
                        {
                            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(string.Format("/ChatPage.xaml?Name={0}&Type={1}", Uri.EscapeUriString(mi.From), Uri.EscapeUriString(mi.Type.ToString())), UriKind.RelativeOrAbsolute));
                        }
                    }
                    App.UnseenMessages.Add(mi.From);
                });
            });

            Hub.On("FriendStatusChanged", (string friendName, StatusIndicator status) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    App.User.Friends.First(x => x.Username == friendName).Status = status;
                });
            });

            Hub.On("FriendRequestReceived", (string username) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    App.User.FriendRequests.Add(username);
                    if ((Application.Current.RootVisual as PhoneApplicationFrame).CurrentSource != new Uri("/FriendRequestsPage.xaml", UriKind.RelativeOrAbsolute))
                    {
                        MessageBoxResult mbr = MessageBox.Show(string.Format("{0} sent you a friend request.\nView friend requests?", username), "You have new friend request!", MessageBoxButton.OKCancel);
                        if (mbr == MessageBoxResult.OK)
                        {
                            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/FriendRequestsPage.xaml", UriKind.RelativeOrAbsolute));
                        }
                    }
                });
            });


            // TO DO: Change OwnerUSerITEM friend to UserItem or send custom object from the server
            Hub.On("FriendRequestAccepted", (UserItem friend) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    App.User.Friends.Add(friend);
                    
                    foreach (RoomItem ri in friend.Rooms)
                    {
                        RoomItem room = App.User.Rooms.FirstOrDefault(x => x.Name == ri.Name);
                        if (room != null)
                        {
                            friend.Rooms.Remove(ri);
                            friend.Rooms.Add(ri);
                        }
                    }
                });
            });

            Hub.On("NewMemberInRoom", (string memberName, string roomName) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (memberName != App.User.Username)
                    {
                        RoomItem ri = App.User.Rooms.First(x => x.Name == roomName);
                        UserItem ui = App.User.Friends.FirstOrDefault(x => x.Username == memberName);
                        if (ui == null)
                        {
                            ui = new UserItem() { Username = memberName };
                        }

                        ri.Users.Add(ui);
                    }
                });
            });

            Hub.On("FriendJoinRoom", (string memberName, string roomName) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    UserItem ui = App.User.Friends.First(x => x.Username == memberName);
                    RoomItem ri = App.User.Rooms.FirstOrDefault(x => x.Name == roomName);
                    if (ri == null)
                    {
                        ri = new RoomItem() { Name = roomName };
                    }

                    ui.Rooms.Add(ri);
                });
            });

            Hub.On("RemoveMemberFromRoom", (string memberName, string roomName) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (memberName != App.User.Username)
                    {
                        RoomItem ri = App.User.Rooms.First(x => x.Name == roomName);
                        UserItem ui = App.User.Friends.FirstOrDefault(x => x.Username == memberName);
                        if (ui == null)
                        {
                            ui = new UserItem() { Username = memberName };
                        }

                        ri.Users.Remove(ui);
                    }
                });
            });

            Hub.On("FriendLeaveRoom", (string memberName, string roomName) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    UserItem ui = App.User.Friends.First(x => x.Username == memberName);
                    RoomItem ri = App.User.Rooms.FirstOrDefault(x => x.Name == roomName);
                    if (ri == null)
                    {
                        ri = new RoomItem() { Name = roomName };
                    }

                    ui.Rooms.Remove(ri);
                });
            });

            try
            {
                Connection.StateChanged += (StateChange obj) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (obj.NewState == ConnectionState.Disconnected)
                        {
                            MessageBoxResult mbr = MessageBox.Show("Connection lost, please try later", "No connection!", MessageBoxButton.OK);
                            if (mbr == MessageBoxResult.OK)
                            {
                                App.ExitApp(false);
                            }
                        }
                        else if (obj.NewState == ConnectionState.Connected)
                        {
                            if (App.IsolatedStorageSettings.Contains("Username") && App.IsolatedStorageSettings.Contains("Password"))
                            {
                                App.User.Login(App.IsolatedStorageSettings["Username"] as string, App.IsolatedStorageSettings["Password"] as string, () =>
                                {
                                    if (App.User.IsLoggedIn == false)
                                    {
                                        App.IsolatedStorageSettings.Remove("Username");
                                        App.IsolatedStorageSettings.Remove("Password");

                                        App.User = new OwnerUserItem();

                                        Dispatcher.BeginInvoke(() =>
                                        {
                                            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
                                        });
                                    }
                                    else
                                    {
                                        Dispatcher.BeginInvoke(() =>
                                        {
                                            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Menu.xaml", UriKind.RelativeOrAbsolute));
                                        });
                                    }
                                });
                            }
                            else
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
                                });
                            }
                        }
                    });
                };
                await Connection.Start();
            }
            catch (Exception ex)
            {
                MessageBoxResult mbr = MessageBox.Show("Cannot connect, please try later", "No connection!", MessageBoxButton.OK);
                if (mbr == MessageBoxResult.OK)
                {
                    App.ExitApp(false);
                }
            }
        }
    }
}