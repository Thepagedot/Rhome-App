using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public SystemVariableViewModel(HomeControlService homeControlService)
        {
            _HomeControlService = homeControlService;
        }

        public async Task InitializeAsync()
        {
            SystemVariables = new ObservableCollection<SystemVariable>(await _HomeControlService.HomeMatic.GetSystemVariablesAsync());
        }
    }
}
