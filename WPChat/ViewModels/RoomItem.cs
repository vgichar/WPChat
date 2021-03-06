﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPChat.ViewModels
{
    public class RoomItem : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private ObservableCollection<UserItem> _users;
        public ObservableCollection<UserItem> Users
        {
            get
            {
                if (_users != null)
                {
                    _users.OrderBy(x => x.Status).ThenBy(x => x.Username);
                }
                else 
                {
                    _users = new ObservableCollection<UserItem>();
                }
                return _users;
            }
            set
            {
                _users = value;
                RaisePropertyChanged("Users");
            }
        }

        private ObservableCollection<MessageItem> _messages;
        public ObservableCollection<MessageItem> Messages
        {
            get
            {
                if (_messages == null)
                {
                    _messages = new ObservableCollection<MessageItem>();
                }
                return _messages;
            }
            set
            {
                _messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        public RoomItem(string name = "") {
            Name = name;
            Users = new ObservableCollection<UserItem>();
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

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Name == (obj as RoomItem).Name;
        }
    }
}
