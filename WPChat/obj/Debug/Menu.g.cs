﻿#pragma checksum "C:\Users\Vojdan\Documents\Visual Studio 2013\Projects\WPChat\WPChat\Menu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2DD956B6E508045C3436EF41E29C74C6"
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
    
    
    public partial class Menu : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button Friends_btn;
        
        internal System.Windows.Controls.Button Rooms_btn;
        
        internal System.Windows.Controls.Button Requests_btn;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WPChat;component/Menu.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Friends_btn = ((System.Windows.Controls.Button)(this.FindName("Friends_btn")));
            this.Rooms_btn = ((System.Windows.Controls.Button)(this.FindName("Rooms_btn")));
            this.Requests_btn = ((System.Windows.Controls.Button)(this.FindName("Requests_btn")));
        }
    }
}
