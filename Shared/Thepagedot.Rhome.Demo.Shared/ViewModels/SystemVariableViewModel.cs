using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.App.Shared.ViewModels;
using Thepagedot.Rhome.HomeMatic.Models;

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

        public SystemVariableViewModel(HomeControlService homeControlService)
        {
            _HomeControlService = homeControlService;

            if (IsInDesignMode)
            {
                SystemVariables = new ObservableCollection<SystemVariable>();
                SystemVariables.Add(new SystemVariable(1, "Anwesenheit", "true", "", "", "", "", 2, 2, true, "", "nicht anwesend", "anwesend"));
                SystemVariables.Add(new SystemVariable(2, "Alarmzone 1", "", "", "", "", "", 2, 2, true, "", "ausgelöst", "nicht ausgelöst"));
                SystemVariables.Add(new SystemVariable(3, "Stringtest", "Lorem ipsum", "", "", "", "", 4, 11, true, "", "", ""));
                SystemVariables.Add(new SystemVariable(3, "List Test", "1", "Var1;Var2;Var3", "", "", "", 16, 29, true, "", "", ""));
            }
        }

        public async Task RefreshAsync()
        {
            IsLoaded = false;

            if (IsLoading || IsLoaded)
                return;

            if (_HomeControlService.HomeMatic != null)
            {
                try
                {
                    SystemVariables = new ObservableCollection<SystemVariable>(await _HomeControlService.HomeMatic.GetSystemVariablesAsync());
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