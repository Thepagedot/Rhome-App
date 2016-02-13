using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Rhome.App.Shared.Other;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class MainViewModel : AsyncViewModelBase
    {
        private SettingsService _SettingsService;
        private HomeControlService _HomeControlService;

        private List<Room> _Rooms;
        public List<Room> Rooms
        {
            get { return _Rooms; }
            set { _Rooms = value; RaisePropertyChanged(); }
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

        public MainViewModel(SettingsService settingsService, HomeControlService homeControlService, IDialogService dialogService, IResourceService resourceService)
            : base (dialogService, resourceService)
        {
            _SettingsService = settingsService;
            _HomeControlService = homeControlService;

            if (IsInDesignMode)
            {
                Rooms = DesignData.GetDemoRooms();
            }
        }

        public async Task RefreshAsync()
        {
            IsLoaded = false;
            IsLoading = true;

            if (_HomeControlService.HomeMatic != null)
            {
                try
                {
                    Rooms = (await _HomeControlService.HomeMatic.GetRoomsWidthDevicesAsync()).ToList();
                    IsLoaded = true;
                }
                catch (HttpRequestException)
                {
                    await ShowConnectionErrorMessageAsync();
                }
            }

            IsLoading = false;
        }
    }
}