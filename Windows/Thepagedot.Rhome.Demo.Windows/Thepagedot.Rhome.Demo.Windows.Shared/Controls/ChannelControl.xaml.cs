﻿using System;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.Demo.Win.ViewModels;
using Thepagedot.Rhome.HomeMatic.Models;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Thepagedot.Rhome.Demo.Win.Controls
{
    public sealed partial class ChannelControl : UserControl
    {
        public static readonly DependencyProperty DpChannel = DependencyProperty.Register("Channel", typeof(object), typeof(ChannelControl), new PropertyMetadata(default(object)));

        public object Channel
        {
            get { return (object)this.GetValue(DpChannel); }
            set { this.SetValueDp(DpChannel, value); }
        }

        public ChannelControl()
        {
            this.InitializeComponent();
            if (DesignMode.DesignModeEnabled)
                LoadDemoData();

            (this.Content as FrameworkElement).DataContext = this;
        }

        private void LoadDemoData()
        {
            var room = MainViewModel.Current.RoomList.First() as HomeMaticRoom;
            var device = room.Devices.First() as HomeMaticDevice;
            Channel = device.Channels.First() as Switcher;
        }

        #region Switcher

        private async void OnOffSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = (sender as ToggleSwitch);
            if (toggleSwitch != null)
            {
                var channel = toggleSwitch.DataContext as HomeMaticChannel;
                if (channel != null)
                {
                    toggleSwitch.IsEnabled = false;
                    await App.HomeMaticXmlApi.SendChannelUpdateAsync(channel.IseId, (sender as ToggleSwitch).IsOn);
                    toggleSwitch.IsEnabled = true;
                }
            }
        }

        #endregion

        #region Shutter

        private async void ShutterUp_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            if (button != null)
            {
                var shutter = button.DataContext as Shutter;
                if (shutter != null)
                {
                    button.IsEnabled = false;
                    await shutter.Up(App.HomeMaticXmlApi);
                    button.IsEnabled = true;
                }
            }
        }

        private async void ShutterDown_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            if (button != null)
            {
                var shutter = button.DataContext as Shutter;
                if (shutter != null)
                {
                    button.IsEnabled = false;
                    await shutter.Down(App.HomeMaticXmlApi);
                    button.IsEnabled = true;
                }
            }
        }

        private async void ShutterStop_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            if (button != null)
            {
                var shutter = button.DataContext as Shutter;
                if (shutter != null)
                {
                    button.IsEnabled = false;
                    await shutter.Stop(App.HomeMaticXmlApi);
                    button.IsEnabled = true;
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        void SetValueDp(DependencyProperty property, object value, [System.Runtime.CompilerServices.CallerMemberName] String p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }        
    }
}
