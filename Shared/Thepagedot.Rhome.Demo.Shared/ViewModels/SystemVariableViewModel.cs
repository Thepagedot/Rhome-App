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
using Thepagedot.Rhome.App.Shared.ViewModels;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Tools;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class SystemVariableViewModel : AsyncViewModelBase
    {
        private HomeControlService _HomeControlService;

        private ObservableCollection<SystemVariable> _SystemVariables;
        public ObservableCollection<SystemVariable> SystemVariables
        {
            get { return _SystemVariables; }
            set { _SystemVariables = value; RaisePropertyChanged();  }
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

        public SystemVariableViewModel(
            INavigationService navigationService,
            IResourceService resourceService,
            IDialogService dialogService,
            HomeControlService homeControlService) : base(navigationService, resourceService, dialogService)
        {
            _HomeControlService = homeControlService;

            if (IsInDesignMode)
            {
                SystemVariables = DesignData.GetDemoSystemVariables();
            }
        }

        public async Task RefreshAsync()
        {
            IsLoaded = false;
            IsLoading = true;

            SystemVariables = new ObservableCollection<SystemVariable>();
            try
            {
                foreach (var platform in _HomeControlService.Platforms)
                {
                    var platformVariables = await platform.Value.GetSystemVariablesAsync();
                    foreach (var v in platformVariables)
                        SystemVariables.Add(v);
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