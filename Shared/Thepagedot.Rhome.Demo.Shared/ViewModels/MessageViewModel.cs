using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.Base.Models;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class MessageViewModel : AsyncViewModelBase
    {
        private HomeControlService _HomeControlService;

        private ObservableCollection<Message> _Messages;
        public ObservableCollection<Message> Messages
        {
            get { return _Messages; }
            set { _Messages = value; RaisePropertyChanged(); }
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


        public MessageViewModel(
            INavigationService navigationService,
            IResourceService resourceService,
            IDialogService dialogService,
            HomeControlService homeControlService) : base(navigationService, resourceService, dialogService)
        {
            _HomeControlService = homeControlService;

            if (IsInDesignMode)
            {
                Messages = new ObservableCollection<Message>();
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
                    Messages = new ObservableCollection<Message>(await _HomeControlService.HomeMatic.GetSystemNotificationsAsync());
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
