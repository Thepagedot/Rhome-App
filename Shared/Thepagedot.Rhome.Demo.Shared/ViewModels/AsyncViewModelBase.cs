using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Tools;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
	public class AsyncViewModelBase : ViewModelBase
	{
		#region Services

		protected INavigationService _NavigationService;
		protected IResourceService _ResourceService;
		protected IDialogService _DialogService;


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

		public AsyncViewModelBase(INavigationService navigationService, IResourceService resourceService, IDialogService dialogService)
		{
			_NavigationService = navigationService;
			_ResourceService = resourceService;
			_DialogService = dialogService;

			// Register events
			ConnectionError += AsyncViewModelBase_ConnectionError;
		}

		private async void AsyncViewModelBase_ConnectionError(object sender, ConnectionErrorEventArgs e)
		{
			await ShowConnectionErrorMessageAsync();
		}

		protected async Task ShowConnectionErrorMessageAsync()
		{
			await _DialogService.ShowMessage(_ResourceService.GetString("connection_failed_message"), _ResourceService.GetString("connection_failed_title"));
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