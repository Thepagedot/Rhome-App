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
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Tools;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class ProgramViewModel : AsyncViewModelBase
    {
        private HomeControlService _HomeControlService;

        private ObservableCollection<Program> _Programs;
        public ObservableCollection<Program> Programs
        {
            get { return _Programs; }
            set { _Programs = value; RaisePropertyChanged(); }
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

        private RelayCommand<Program> _RunCommand;
        public RelayCommand<Program> RunCommand
        {
            get
            {
                return _RunCommand ?? (_RunCommand = new RelayCommand<Program>(async (program) =>
                {
                    await program.RunAsync();
                }));
            }
        }

        public ProgramViewModel(
            INavigationService navigationService,
            IResourceService resourceService,
            IDialogService dialogService,
            HomeControlService homeControlService) : base(navigationService, resourceService, dialogService)
        {
            _HomeControlService = homeControlService;

            if (IsInDesignMode)
            {
                Programs = new ObservableCollection<Program>();
                Programs.Add(new Program(1, true, "", "Lorem ipsum", "amit die virtuellen Wired Kanäle der CCU angezeigt werden, muss der Name der Komponente auf der CCU in einen X Beliebigen umbenannt werden.", true, true, null));
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
                    Programs = new ObservableCollection<Program>(await _HomeControlService.HomeMatic.GetProgramsAsync());
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
