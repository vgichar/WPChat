<<<<<<< HEAD
﻿#pragma checksum "C:\korisnicki_winPhone\WPChat\WPChat\DetailsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FB6E11583CE78AB29BCA279BFF8A0B6A"
=======
﻿#pragma checksum "C:\Users\vojda_000\documents\visual studio 2013\Projects\WPChat\WPChat\DetailsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E6366387D8493FD4E9CB363919A917A2"
>>>>>>> origin/master
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WPChat {
    
    
    public partial class DetailsPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock tbTitle;
        
        internal System.Windows.Controls.TextBlock tbStats;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.LongListSelector llsUsers;
        
        internal Microsoft.Phone.Controls.LongListSelector llsRooms;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/WPChat;component/DetailsPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tbTitle = ((System.Windows.Controls.TextBlock)(this.FindName("tbTitle")));
            this.tbStats = ((System.Windows.Controls.TextBlock)(this.FindName("tbStats")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.llsUsers = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("llsUsers")));
            this.llsRooms = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("llsRooms")));
        }
    }
}

