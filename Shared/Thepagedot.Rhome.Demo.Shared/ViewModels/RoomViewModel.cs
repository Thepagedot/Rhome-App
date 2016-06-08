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
        private HomeControlService _HomeControlService;

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
            IDialogService dialogService,
            HomeControlService homeControlService) : base(navigationService, resourceService, dialogService)
        {
            _HomeControlService = homeControlService;

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

            try
            {
                // HACK: Every platform checks every room here. This is not necessary!
                foreach (var platform in _HomeControlService.Platforms)
                {
                    foreach (var room in CurrentRoom.Rooms)
                    {
                        await platform.Value.UpdateStatesForRoomAsync(room);
                        foreach (var device in room.Devices)
                            devices.Add(device);
                    }
                }

                Devices.Clear();
                foreach (var device in devices)
                    Devices.Add(device);

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
