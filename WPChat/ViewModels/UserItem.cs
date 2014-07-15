using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPChat.ViewModels
{
    public class UserItem : INotifyPropertyChanged
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
                List<RoomItem> list = _rooms.ToList();
                list.OrderBy(x => x.Users.Count).ThenBy(x => x.Name);
                _rooms.Clear();

                foreach (RoomItem ui in list)
                {
                    _rooms.Add(ui);
                }
                return _rooms;
            }
            set
            {
                _rooms = value;
                RaisePropertyChanged("Rooms");
            }
        }

        private ObservableCollection<MessageItem> _messages;
        public ObservableCollection<MessageItem> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        public UserItem(string username = "", StatusIndicator status = StatusIndicator.Offline) {
            Username = username;
            Status = status;
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

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Username == (obj as UserItem).Username;
        }
    }
}
