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

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class MainViewModel : AsyncViewModelBase
    {
        private ILocalStorageService _SettingsService;
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

        public MainViewModel(ILocalStorageService settingsService, HomeControlService homeControlService, IDialogService dialogService, IResourceService resourceService)
            : base (dialogService, resourceService)
        {
            _SettingsService = settingsService;
            _HomeControlService = homeControlService;

            if (IsInDesignMode)
            {
                Rooms = new List<Room>
                {
                    new HomeMaticRoom("Bedroom", 0, new List<int>()),
                    new HomeMaticRoom("Living room", 0, new List<int>()),
                    new HomeMaticRoom("Kitchen", 0, new List<int>())
                };
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
                    //TODO: Load strings from ResourceService
                    RaiseConnectionError("Connection Error", "Failed to connect");
                }
            }

            IsLoading = false;
        }
    }
}