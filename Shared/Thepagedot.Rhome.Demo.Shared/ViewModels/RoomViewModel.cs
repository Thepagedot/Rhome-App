using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Other;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.HomeMatic.Models;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class RoomViewModel : AsyncViewModelBase
    {
        private HomeControlService _HomeControlService;

        private Room _CurrentRoom;
        public Room CurrentRoom
        {
            get { return _CurrentRoom; }
            set { _CurrentRoom = value; RaisePropertyChanged(); }
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

        public RoomViewModel(HomeControlService homeControlService, IDialogService dialogService, IResourceService resourceService)
            : base(dialogService, resourceService)
        {
            _HomeControlService = homeControlService;

            if (IsInDesignMode)
            {
                CurrentRoom = DesignData.GetDemoRoom();
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
                    await _HomeControlService.HomeMatic.UpdateStatesForRoomAsync(_CurrentRoom);
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
