﻿using System;
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
    public partial class OwnerUserItem : INotifyPropertyChanged
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
                    _rooms.OrderBy(x => x.Users.Count).ThenBy(x => x.Name);
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
                    _friends.OrderBy(x => x.Status).ThenBy(x => x.Username);
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

        private ObservableCollection<string> _friendRequests;
        public ObservableCollection<string> FriendRequests
        {
            get
            {
                if (_friendRequests == null)
                {
                    _friendRequests = new ObservableCollection<string>();
                }
                return _friendRequests;
            }
            set
            {
                _friendRequests = value;
                RaisePropertyChanged("FriendRequests");
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
            FriendRequests = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public partial class OwnerUserItem{

        public async void GetUserByName(string name, UserItem userItem, Action callback)
        {
            UserItem ui = await App.Hub.Invoke<UserItem>("GetUserByName", name);
            App.Dispatcher.BeginInvoke(() =>
            {
                foreach (RoomItem ri in ui.Rooms)
                {
                    foreach (RoomItem ori in Rooms)
                    {
                        if (ri.Name == ori.Name)
                        {
                            ori.Users.Remove(ori.Users.First(x => x.Username == ui.Username));
                            ori.Users.Add(ui);

                            ri.Name = ori.Name;
                            ri.Users = ori.Users;
                            ri.Messages = ori.Messages;
                        }
                    }
                }

                userItem.Username = ui.Username;
                userItem.Status = ui.Status;
                userItem.Messages = ui.Messages;
                userItem.Rooms = ui.Rooms;

                callback.Invoke();
            });
        }

        public async void GetRoomByName(string name, RoomItem roomItem, Action callback)
        {
            RoomItem ri = await App.Hub.Invoke<RoomItem>("GetRoomByName", name);
            App.Dispatcher.BeginInvoke(() =>
            {
                foreach (UserItem ui in ri.Users)
                {
                    foreach (UserItem oui in Friends)
                    {
                        if (ui.Username == oui.Username)
                        {
                            oui.Rooms.Remove(oui.Rooms.First(x => x.Name == ri.Name));
                            oui.Rooms.Add(ri);

                            ui.Username = oui.Username;
                            ui.Status = oui.Status;
                            ui.Messages = oui.Messages;
                            ui.Rooms = oui.Rooms;
                        }
                    }
                }

                roomItem.Name = ri.Name;
                roomItem.Users = ri.Users;
                roomItem.Messages = ri.Messages;

                callback.Invoke();
            });
        }

        public async void GetUsersByNameStart(string name, IList list, Action callback)
        {
            List<UserItem> l = await App.Hub.Invoke<List<UserItem>>("GetUsersByNameStart", name);
            App.Dispatcher.BeginInvoke(() =>
            {
                l.ForEach(x => list.Add(x));

                callback.Invoke();
            });
        }

        public async void GetRoomsByNameStart(string name, IList list, Action callback)
        {
            List<RoomItem> l = await App.Hub.Invoke<List<RoomItem>>("GetRoomsByNameStart", name);
            App.Dispatcher.BeginInvoke(() =>
            {
                l.ForEach(x => list.Add(x));

                callback.Invoke();
            });
        }

        public async void RemoveFriend(string username)
        {
            bool res = await App.Hub.Invoke<bool>("RemoveFriend", username);
            App.Dispatcher.BeginInvoke(() =>
            {
                if (res)
                {
                    Friends.Remove(Friends.First(x => x.Username == username));
                }
            });
        }

        public async void RemoveRoom(string name)
        {
            bool res = await App.Hub.Invoke<bool>("RemoveRoom", name);
            App.Dispatcher.BeginInvoke(() =>
            {
                if (res)
                {
                    Rooms.Remove(Rooms.First(x => x.Name == name));
                }
            });
        }

        public async void AddFriend(string username)
        {
            await App.Hub.Invoke("AddFriend", username);
            App.Dispatcher.BeginInvoke(() =>
            {
                UserItem ui = new UserItem();
                GetUserByName(username, ui, () =>
                {
                    Friends.Add(ui);
                });
            });
        }

        public async void AddRoom(string name, Action callback)
        {
            RoomItem ri = await App.Hub.Invoke<RoomItem>("AddRoom", name);
            App.Dispatcher.BeginInvoke(() =>
            {
                foreach (UserItem ui in ri.Users)
                {
                    foreach (UserItem oui in Friends)
                    {
                        if (ui.Username == oui.Username)
                        {
                            oui.Rooms.Remove(oui.Rooms.First(x => x.Name == ri.Name));
                            oui.Rooms.Add(ri);

                            ui.Username = oui.Username;
                            ui.Status = oui.Status;
                            ui.Messages = oui.Messages;
                            ui.Rooms = oui.Rooms;
                        }
                    }
                }

                Rooms.Add(ri);
            });

            callback.Invoke();
        }

        public async void SendMessage(MessageItem mi)
        {
            await App.Hub.Invoke("SendMessage", mi);
            App.Dispatcher.BeginInvoke(() =>
            {
                if (mi.Type == DataContextType.User)
                {
                    Friends.First(x => x.Username == mi.To).Messages.Add(mi);
                }
                else
                {
                    Rooms.First(x => x.Name == mi.To).Messages.Add(mi);
                }
            });
        }

        public async void CreateRoom(string name, Action callback)
        {
            bool isCreated = await App.Hub.Invoke<bool>("CreateRoom", name); App.Dispatcher.BeginInvoke(() =>
             {
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
             });
        }

        public async void ChangeStatus(StatusIndicator status)
        {
            bool res = await App.Hub.Invoke<bool>("ChangeStatus", status);
            App.Dispatcher.BeginInvoke(() => {
                if (res)
                {
                    this.Status = status;
                }
            });
        }

        public async void Logout()
        {
            bool res = await App.Hub.Invoke<bool>("Logout");

            if (res)
            {
                App.IsolatedStorageSettings.Remove("Username");
                App.IsolatedStorageSettings.Remove("Password");
                App.User = new OwnerUserItem();
            }
        }

        public async void Login(string username, string password, Action callback)
        {
            OwnerUserItem oui = await App.Hub.Invoke<OwnerUserItem>("Login", username, password);
            App.Dispatcher.BeginInvoke(() =>
            {
                if (oui != null)
                {
                    this.Username = username;
                    this.Password = password;
                    this.IsLoggedIn = true;
                    this.Status = oui.Status;
                    App.Dispatcher.BeginInvoke(() =>
                    {
                        this.FriendRequests = new ObservableCollection<string>(oui.FriendRequests);
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
            });
        }

        public async void Register(string username, string password, Action callback)
        {
            bool isRegistered = await App.Hub.Invoke<bool>("Register", username, password);
            App.Dispatcher.BeginInvoke(() =>
            {
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
            });
        }

        public async void SendFriendRequest(string name)
        {
            await App.Hub.Invoke("SendFriendRequest", name);
        }

        public async void AcceptFriendRequest(string name)
        {
            await App.Hub.Invoke("AcceptFriendRequest", name);
        }

        public async void DenyFriendRequest(string name)
        {
            await App.Hub.Invoke("DenyFriendRequest", name);
        }
    }
}