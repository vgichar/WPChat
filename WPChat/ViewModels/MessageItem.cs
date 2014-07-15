using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPChat.ViewModels
{
    public class MessageItem : INotifyPropertyChanged
    {
        private DataContextType _type;
        public DataContextType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }
        
        private string _from;
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                RaisePropertyChanged("From");
            }
        }

        private string _to;
        public string To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
                RaisePropertyChanged("To");
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        private INotifyPropertyChanged _link;
        public INotifyPropertyChanged Link
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
                RaisePropertyChanged("Link");
            }
        }

        public MessageItem() {
            From = "";
            To = "";
            Text = "";
            Type = DataContextType.User;
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
}
