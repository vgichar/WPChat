using System;
using System.Collections;
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

        public async void getUserByName(string name, UserItem userItem, Action callback)
        {
            UserItem ui = await App.Hub.Invoke<UserItem>("GetUserByName", name);
            userItem = ui;

            foreach (RoomItem ri in ui.Rooms) {
                foreach (RoomItem ori in Rooms) {
                    if (ri.Name == ori.Name) {
                        ori.Users.Remove(ori.Users.First(x => x.Username == ui.Username));
                        ori.Users.Add(ui);

                        ui.Rooms.Remove(ri);
                        ui.Rooms.Add(ori);
                    }
                }
            }

            callback();
        }

        public async void getRoomByName(string name, RoomItem roomItem, Action callback)
        {
            RoomItem ri = await App.Hub.Invoke<RoomItem>("GetRoomByName", name);
            roomItem = ri;



            foreach (UserItem ui in ri.Users)
            {
                foreach (UserItem oui in Friends)
                {
                    if (ui.Username == oui.Username)
                    {
                        oui.Rooms.Remove(oui.Rooms.First(x => x.Name == ri.Name));
                        oui.Rooms.Add(ri);

                        ri.Users.Remove(ui);
                        ri.Users.Add(oui);
                    }
                }
            }

            callback();
        }

        public async void getUsersByNameStart(string name, IList list, Action callback)
        {
            List<UserItem> l = await App.Hub.Invoke<List<UserItem>>("GetUsersByNameStart", name);

            l.ForEach(x => list.Add(x));

            callback();
        }

        public async void getRoomsByNameStart(string name, IList list, Action callback)
        {
            List<RoomItem> l = await App.Hub.Invoke<List<RoomItem>>("GetRoomsByNameStart", name);

            l.ForEach(x => list.Add(x));

            callback();
        }

        public async void addFriend(string username)
        {
            await App.Hub.Invoke("AddFriend", Username, username);

            UserItem ui = new UserItem();
            getUserByName(username, ui, () =>
            {
                Friends.Add(ui);
            });
        }

        public async void addRoom(string name)
        {
            await App.Hub.Invoke("AddRoom", Username, name);

            RoomItem ri = new RoomItem();
            getRoomByName(name, ri, () =>
            {
                Rooms.Add(ri);
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
            if (!isCreated)
            {
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
                            if (ri.Users.Contains(ui))
                            {
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

            if (isRegistered)
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