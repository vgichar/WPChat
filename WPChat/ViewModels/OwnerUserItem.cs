using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPChat.ViewModels
{
    public class OwnerUserItem : INotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                RaisePropertyChanged("Username");
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                _isLoggedIn = value;
                RaisePropertyChanged("IsLoggedIn");
            }
        }

        private StatusIndicator _status;
        public StatusIndicator Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        private ObservableCollection<RoomItem> _rooms;
        public ObservableCollection<RoomItem> Rooms
        {
            get
            {
                if (_rooms != null)
                {
                    List<RoomItem> list = _rooms.ToList();
                    list.OrderBy(x => x.Users.Count).ThenBy(x => x.Name);
                    _rooms.Clear();

                    foreach (RoomItem ui in list)
                    {
                        _rooms.Add(ui);
                    }
                }
                else
                {
                    _rooms = new ObservableCollection<RoomItem>();
                }
                return _rooms;
            }
            set
            {
                _rooms = value;
                RaisePropertyChanged("Rooms");
            }
        }

        private ObservableCollection<UserItem> _friends;
        public ObservableCollection<UserItem> Friends
        {
            get
            {
                if (_friends != null)
                {
                    List<UserItem> list = _friends.ToList();
                    list.OrderBy(x => x.Status).ThenBy(x => x.Username);
                    _friends.Clear();

                    foreach (UserItem ui in list)
                    {
                        _friends.Add(ui);
                    }
                }
                else 
                {
                    _friends = new ObservableCollection<UserItem>();
                }
                return _friends;
            }
            set
            {
                _friends = value;
                RaisePropertyChanged("Friends");
            }
        }

        public OwnerUserItem()
        {
            Username = "";
            Password = "";
            IsLoggedIn = false;
            Status = StatusIndicator.Online;
            Friends = new ObservableCollection<UserItem>();
            Rooms = new ObservableCollection<RoomItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public UserItem getUserByName(string name)
        {

            // TODO:
            // get UserItem from server by name
            // /TODO

            // TEMP
            UserItem ui = new UserItem()
            {
                Username = name,
                Status = StatusIndicator.Busy,
                Rooms = new ObservableCollection<RoomItem>() {
                    new RoomItem(){
                        Name = "myRoom",
                        Users = new ObservableCollection<UserItem>()
                    }
                }
            };

            ui.Rooms.ElementAt(0).Users.Add(ui);
            return ui;
        }

        public RoomItem getRoomByName(string name)
        {

            // TODO:
            // get RoomItem from server by name
            // /TODO

            // TEMP
            RoomItem ri = new RoomItem()
            {
                Name = name,
                Users = new ObservableCollection<UserItem>()
            };

            return ri;
        }

        public List<UserItem> getUsersByNameStart(string name)
        {

            // TODO:
            // get UserItem from server by name
            // /TODO

            // TEMP

            List<UserItem> users = new List<UserItem>();
            //List<UserItem> users = App.Hub.Invoke("GetUsersByNameStart", name);
            // ^ problem
            return users;
        }

        public List<RoomItem> getRoomsByNameStart(string name)
        {

            // TODO:
            // get RoomItem from server by name
            // /TODO

            // TEMP
            List<RoomItem> rooms = new List<RoomItem>();
            Random r = new Random();
            for (int i = 0; i < r.Next(10) + 5; i++)
            {
                rooms.Add(new RoomItem()
                {
                    Name = name + " " + r.Next(10000),
                    Users = new ObservableCollection<UserItem>() {
                        new UserItem(),
                        new UserItem()
                    }
                });
            }
            return rooms;
        }

        public async void addFriend(string username)
        {
            // DONE
            // add friend in server
            // DONE
            await App.Hub.Invoke("AddFriend", Username, username);

            this.Friends.Add(new UserItem()
            {
                Username = username,
                Status = StatusIndicator.Offline,
                Rooms = new ObservableCollection<RoomItem>()
            });
        }

        public async void addRoom(string name)
        {
            // DONE:
            // add fav room in server
            // /DONE

            await App.Hub.Invoke("AddRoom", Username ,name);

            this.Rooms.Add(new RoomItem()
            {
                Name = name,
                Users = new ObservableCollection<UserItem>(),
                Messages = new ObservableCollection<MessageItem>()
            });
        }

        public async void sendMessage(MessageItem mi)
        {
            await App.Hub.Invoke("SendMessage", mi);

            if (mi.Type == DataContextType.User)
            {
                Friends.First(x => x.Username == mi.To).Messages.Add(mi);
            }
            else
            {
                Rooms.First(x => x.Name == mi.To).Messages.Add(mi);
            }
        }

        public async void CreateRoom(string name, Action callback)
        {
            bool isCreated = await App.Hub.Invoke<bool>("CreateRoom", Username, name);
            if (!isCreated) {
                MessageBox.Show("Room already exists!\nPlease try another name", "Room Exists!", MessageBoxButton.OK);
            }
            else
            {
                RoomItem ri = new RoomItem()
                {
                    Name = name,
                    Users = new ObservableCollection<UserItem>(),
                    Messages = new ObservableCollection<MessageItem>()
                };
                Rooms.Add(ri);
            }
            callback.Invoke();
        }
        public async void ChangeStatus(StatusIndicator status)
        {
            await App.Hub.Invoke<bool>("ChangeStatus", Username, status);
        }

        public async void Logout()
        {
            await App.Hub.Invoke<bool>("Logout", Username);
        }

        public async void Login(string username, string password, Action callback)
        {
            OwnerUserItem oui = await App.Hub.Invoke<OwnerUserItem>("Login", username, password);

            if (oui != null)
            {
                this.Username = username;
                this.Password = password;
                this.IsLoggedIn = true;
                this.Status = oui.Status;
                App.Dispatcher.BeginInvoke(() =>
                {
                    this.Friends = new ObservableCollection<UserItem>(oui.Friends);
                    this.Rooms = new ObservableCollection<RoomItem>(oui.Rooms);

                    foreach (RoomItem ri in this.Rooms)
                    {
                        foreach (UserItem ui in this.Friends)
                        {
                            if (ri.Users.Contains(ui)) {
                                RoomItem room = ui.Rooms.First(x => x.Name == ri.Name);
                                UserItem user = ri.Users.First(x => x.Username == ui.Username);

                                ui.Rooms.Remove(room);
                                ui.Rooms.Add(ri);

                                ri.Users.Remove(user);
                                ri.Users.Add(ui);
                            }
                        }
                    }
                });
            }

            callback.Invoke();
        }

        public async void Register(string username, string password, Action callback)
        {
            bool isRegistered = await App.Hub.Invoke<bool>("Register", username, password);

            if(isRegistered)
            {
                this.Username = username;
                this.Password = password;
                this.Status = StatusIndicator.Online;
            }
            else
            {
                this.Username = "";
                this.Password = "";
            }

            callback.Invoke();
        }
    }
}