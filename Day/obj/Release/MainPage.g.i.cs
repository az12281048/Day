﻿#pragma checksum "E:\Coding\Windows Phone\works\Day\Day\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "392BEE2934281130E23800AD9BDE9A54"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34014
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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


namespace Day {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage myMainPage;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot myPivot;
        
        internal System.Windows.Controls.TextBlock textBlockDaoshu;
        
        internal System.Windows.Controls.ListBox listBoxDaoshu;
        
        internal System.Windows.Controls.TextBlock textBlockJishu;
        
        internal System.Windows.Controls.ListBox listBoxJishu;
        
        internal Microsoft.Phone.Shell.ApplicationBar applicationBar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Day;component/MainPage.xaml", System.UriKind.Relative));
            this.myMainPage = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("myMainPage")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.myPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("myPivot")));
            this.textBlockDaoshu = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockDaoshu")));
            this.listBoxDaoshu = ((System.Windows.Controls.ListBox)(this.FindName("listBoxDaoshu")));
            this.textBlockJishu = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockJishu")));
            this.listBoxJishu = ((System.Windows.Controls.ListBox)(this.FindName("listBoxJishu")));
            this.applicationBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("applicationBar")));
        }
    }
}
