﻿#pragma checksum "C:\Users\vojda_000\documents\visual studio 2013\Projects\WPChat\WPChat\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3BCBE491F9C30841F08E6DB209A01831"
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
    
    
    public partial class LoginPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar pbLogin;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox tbUsername;
        
        internal System.Windows.Controls.PasswordBox tbPassword;
        
        internal System.Windows.Controls.CheckBox ckbRemember;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WPChat;component/LoginPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pbLogin = ((System.Windows.Controls.ProgressBar)(this.FindName("pbLogin")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.tbUsername = ((System.Windows.Controls.TextBox)(this.FindName("tbUsername")));
            this.tbPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("tbPassword")));
            this.ckbRemember = ((System.Windows.Controls.CheckBox)(this.FindName("ckbRemember")));
        }
    }
}

