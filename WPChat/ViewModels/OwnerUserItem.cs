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
                return _rooms;
            }
            set
            {
                _rooms = value;
                RaisePropertyChanged("Rooms");
            }
        }

        private ObservableCollection<MessageItem> Messages;

        private ObservableCollection<UserItem> _friends;
        public ObservableCollection<UserItem> Friends
        {
            get
            {
                List<UserItem> list = _friends.ToList();
                list.OrderBy(x => x.Status).ThenBy(x => x.Username);
                _friends.Clear();

                foreach (UserItem ui in list)
                {
                    _friends.Add(ui);
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
            Messages = new ObservableCollection<MessageItem>();
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
            Random r = new Random();
            for (int i = 0; i < r.Next(10) + 5; i++)
            {
                users.Add(new UserItem()
                {
                    Username = name + " " + r.Next(10000),
                    Status = StatusIndicator.Offline,
                    Rooms = new ObservableCollection<RoomItem>()
                });
            }
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

        public void addFriend(string username)
        {
            // TODO:
            // add friend in server
            // /TODO
            this.Friends.Add(new UserItem()
            {
                Username = username,
                Status = StatusIndicator.Offline,
                Rooms = new ObservableCollection<RoomItem>()
            });
        }

        public void addRoom(string name)
        {
            // TODO:
            // add fav room in server
            // /TODO
            this.Rooms.Add(new RoomItem()
            {
                Name = name,
                Users = new ObservableCollection<UserItem>(),
                Messages = new ObservableCollection<MessageItem>()
            });
        }

        public void sendMessage(MessageItem mi)
        {
            if (mi.Type == DataContextType.Room)
            {
                RoomItem i = mi.Link as RoomItem;
                i.Messages.Add(mi);

                // TEMP
                i.Messages.Add(new MessageItem()
                {
                    From = mi.To,
                    To = App.User.Username,
                    Text = "Hello!",
                    Type = mi.Type,
                    Link = i
                });
                i.Messages.Add(new MessageItem()
                {
                    From = mi.To,
                    To = App.User.Username,
                    Text = "How Are you?!",
                    Type = mi.Type,
                    Link = i
                });
            }
            else
            {
                UserItem i = mi.Link as UserItem;
                i.Messages.Add(mi);

                // TEMP
                i.Messages.Add(new MessageItem()
                {
                    From = mi.To,
                    To = App.User.Username,
                    Text = "Hello!",
                    Type = mi.Type,
                    Link = i
                });
                i.Messages.Add(new MessageItem()
                {
                    From = mi.To,
                    To = App.User.Username,
                    Text = "How Are you?!",
                    Type = mi.Type,
                    Link = i
                });
            }

            // TODO:
            // send message to server
            // /TODO
        }

        public void joinRoom(RoomItem ri)
        {
            // TODO:
            // notify server
            // /TODO

            if (ri.Users == null) {
                ri.Users = new ObservableCollection<UserItem>();
            }

            ri.Users.Add(new UserItem() {
                Username = this.Username,
                Status = this.Status,
                Rooms = this.Rooms,
                Messages = this.Messages
            });
        }

        public void leaveRoom(RoomItem ri)
        {
            // TODO:
            // notify server
            // /TODO

            ri.Users.Remove(new UserItem()
            {
                Username = this.Username,
                Status = this.Status,
                Rooms = this.Rooms,
                Messages = this.Messages
            });
        }

        public async void CreateRoom(string name, Action callback)
        {
            bool isCreated = await App.Hub.Invoke<bool>("CreateRoom", name);
            if (!isCreated) {
                MessageBox.Show("Room already exists!\nPlease try another name", "Room Exists!", MessageBoxButton.OK);
            }
            else
            {
                Rooms.Add(new RoomItem() {
                    Name = name,
                    Users = new ObservableCollection<UserItem>(),
                    Messages = new ObservableCollection<MessageItem>()
                });
            }
            callback.Invoke();
        }
        public async void ChangeStatus(StatusIndicator status)
        {
            await App.Hub.Invoke<bool>("ChangeStatus", status);
        }

        public async void Logout()
        {
            await App.Hub.Invoke<bool>("Logout");
        }

        public async void Login(string username, string password, Action callback)
        {
            this.IsLoggedIn = await App.Hub.Invoke<bool>("Login", username, password);

            if (this.IsLoggedIn)
            {
                this.Username = username;
                this.Password = password;
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
