﻿#pragma checksum "C:\Users\Vojdan\Documents\Visual Studio 2013\Projects\WPChat\WPChat\AddPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8BA8B47FCA507F957EB71F2C4F9C16A6"
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
    
    
    public partial class AddPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar pbCreateRoom;
        
        internal System.Windows.Controls.TextBlock tbTitle;
        
        internal Microsoft.Phone.Controls.ListPicker lpStatus;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.LongListSelector llsUsers;
        
        internal Microsoft.Phone.Controls.LongListSelector llsRooms;
        
        internal System.Windows.Controls.TextBox tbSearchInput;
        
        internal System.Windows.Controls.Button btCreate;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WPChat;component/AddPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pbCreateRoom = ((System.Windows.Controls.ProgressBar)(this.FindName("pbCreateRoom")));
            this.tbTitle = ((System.Windows.Controls.TextBlock)(this.FindName("tbTitle")));
            this.lpStatus = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("lpStatus")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.llsUsers = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("llsUsers")));
            this.llsRooms = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("llsRooms")));
            this.tbSearchInput = ((System.Windows.Controls.TextBox)(this.FindName("tbSearchInput")));
            this.btCreate = ((System.Windows.Controls.Button)(this.FindName("btCreate")));
        }
    }
}

