using System;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Rhome.HomeMatic.Services;
using Thepagedot.Rhome.App.UWP;
using Thepagedot.Rhome.App.Shared.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Thepagedot.Rhome.App.UWP.Controls
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
            Channel = new Switcher("", 0, 0, "", true, null);
        }

        #region Switcher

        private async void OnOffSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = (sender as ToggleSwitch);
            if (toggleSwitch != null)
            {
                var switcher = toggleSwitch.DataContext as Switcher;
                if (switcher != null)
                {
                    toggleSwitch.IsEnabled = false;
                    if (switcher.State != toggleSwitch.IsOn)
                        await switcher.SetStateAsync(toggleSwitch.IsOn);
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
                    await shutter.UpAsync();
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
                    await shutter.DownAsync();
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
                    await shutter.StopAsync();
                    button.IsEnabled = true;
                }
            }
        }

        #endregion

        #region Temperature Slider

        private async void TemperatureSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var slider = (sender as Slider);
            var temperatureSlider = slider.DataContext as TemperatureSlider;

            if (temperatureSlider != null && slider.Value != temperatureSlider.Value)
                await temperatureSlider.ChangeTemperatureAsync(slider.Value);
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
