using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Other;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Tools;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class RoomViewModel : AsyncViewModelBase
    {
        private MergedRoom _CurrentRoom;
        public MergedRoom CurrentRoom
        {
            get { return _CurrentRoom; }
            set { _CurrentRoom = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Device> _Devices;
        public ObservableCollection<Device> Devices
        {
            get { return _Devices; }
            set { _Devices = value; RaisePropertyChanged(); }
        }

        private RelayCommand _RefreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(async () =>
                {
                    await RefreshAsync();
                }));
            }
        }

        public RoomViewModel(
            INavigationService navigationService,
            IResourceService resourceService,
            IDialogService dialogService) : base(navigationService, resourceService, dialogService)
        {
            Devices = new ObservableCollection<Device>();

            if (IsInDesignMode)
            {
                CurrentRoom = DesignData.GetDemoRoom();
            }
        }

        public async Task RefreshAsync()
        {
            IsLoaded = false;
            IsLoading = true;
            var devices = new List<Device>();

            // Add devices for each of the merged rooms
            foreach (var room in CurrentRoom.Rooms)
                foreach (var device in room.Devices)
                    devices.Add(device);

            Devices.Clear();
            foreach (var device in devices)
                Devices.Add(device);

            try
            {
                // Update states
                foreach (var room in CurrentRoom.Rooms)
                {
                    await room.UpdateStatesAsync();
                }

                IsLoaded = true;
            }
            catch (HttpRequestException)
            {
                await ShowConnectionErrorMessageAsync();
            }

            IsLoading = false;
        }
    }
}
