using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.App.UWP.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Thepagedot.Rhome.App.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Init MainViewModel
            if (!App.Bootstrapper.MainViewModel.IsLoaded && !App.Bootstrapper.MainViewModel.IsLoading)
                await App.Bootstrapper.MainViewModel.RefreshAsync();

            // Initially set number od columns to the current state's value
            SetNumberOfColumnsByState(VisualStateManager.GetVisualStateGroups(MainGrid).First().CurrentState);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            HamburgerSplitView.IsPaneOpen = !HamburgerSplitView.IsPaneOpen;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Close hamburger pane
            HamburgerSplitView.IsPaneOpen = false;

            if (e.ClickedItem == MenuSettings)
                Frame.Navigate(typeof(SettingsPage));
            else if (e.ClickedItem == MenuSystemVariables)
                Frame.Navigate(typeof(SystemVariablePage));
            else if (e.ClickedItem == MenuPrograms)
                Frame.Navigate(typeof(ProgramPage));
        }

        private void gvRooms_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Set clicked room as current room
            var room = e.ClickedItem as Room;
            App.Bootstrapper.RoomViewModel.CurrentRoom = room;

            // Navigate to room page
            Frame.Navigate(typeof(RoomPage));
        }

        #region Room Grid

        private int _NumberOfColumns = 2;

        private void gvRooms_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Make sure that the item's width is alsways half the size
            ((ItemsWrapGrid)gvRooms.ItemsPanelRoot).ItemWidth = e.NewSize.Width / _NumberOfColumns - gvRooms.Padding.Left;
        }

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            SetNumberOfColumnsByState(e.NewState);
        }

        private void SetNumberOfColumnsByState(VisualState state)
        {
            if (state.Name.Equals(VisualStatePhone.Name))
                _NumberOfColumns = 2;
            else if (state.Name.Equals(VisualStateTablet.Name))
                _NumberOfColumns = 3;
            else if (state.Name.Equals(VisualStateDesktop.Name))
                _NumberOfColumns = 4;
        }

        #endregion
    }
}
