using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Services;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
    public class AsyncViewModelBase : ViewModelBase
    {
        #region Services

        protected IDialogService _DialogService;
        protected IResourceService _ResourceService;

        #endregion

        #region Events

        public event ConnectionErrorEventHandler ConnectionError;
        public delegate void ConnectionErrorEventHandler(object sender, ConnectionErrorEventArgs e);
        public void RaiseConnectionError(string title, string message)
        {
            if (ConnectionError != null)
                ConnectionError(this, new ConnectionErrorEventArgs(title, message));
        }

        #endregion

        #region Properties

        private bool _IsLoading = false;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; RaisePropertyChanged(); }
        }

        private bool _IsLoaded = false;
        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set { _IsLoaded = value; RaisePropertyChanged(); }
        }

        #endregion

        public AsyncViewModelBase(IDialogService dialogService, IResourceService resourceService)
        {
            _DialogService = dialogService;
            _ResourceService = resourceService;

            // Register events
            ConnectionError += AsyncViewModelBase_ConnectionError;
        }

        private async void AsyncViewModelBase_ConnectionError(object sender, ConnectionErrorEventArgs e)
        {
            await _DialogService.ShowMessageDialogAsync(_ResourceService.GetString("ConnectionErrorTitle"), _ResourceService.GetString("ConnectionErrorMessage"));
        }
    }

    public class ConnectionErrorEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public ConnectionErrorEventArgs(string title, string message) : base()
        {
            this.Title = title;
            this.Message = message;
        }
    }
}