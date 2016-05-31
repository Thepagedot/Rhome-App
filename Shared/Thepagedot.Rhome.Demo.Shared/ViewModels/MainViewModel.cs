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
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using Thepagedot.Tools;
using Thepagedot.Rhome.HomeMatic.Services;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
	public class MainViewModel : AsyncViewModelBase
	{
		private SettingsService _SettingsService;
		private HomeControlService _HomeControlService;
		private RoomViewModel _RoomViewModel;

		private ObservableCollection<Room> _Rooms;
		public ObservableCollection<Room> Rooms
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

		#region Navigation Commands

		private RelayCommand<Room> _NavigateToRoomCommand;
		public RelayCommand<Room> NavigateToRoomCommand
		{
			get
			{
				return _NavigateToRoomCommand ?? (_NavigateToRoomCommand = new RelayCommand<Room>((Room room) =>
				{
					// TODO: Check if linking to the RoomViewModel may be a bad practice.
					// Alternative would be to navigate to the room page with the current room as parameter
					// The roompage then can set the current room of the room view model
					_RoomViewModel.CurrentRoom = room;
					_NavigationService.NavigateTo(ViewNames.Room);
				}));
			}
		}

		private RelayCommand _NavigateToSystemVariableCommand;
		public RelayCommand NavigateToSystemVariableCommand
		{
			get { return _NavigateToSystemVariableCommand ?? (_NavigateToSystemVariableCommand = new RelayCommand(() => { _NavigationService.NavigateTo(ViewNames.SystemVariable); })); }
		}

		private RelayCommand _NavigateToProgramCommand;
		public RelayCommand NavigateToProgramCommand
		{
			get { return _NavigateToProgramCommand ?? (_NavigateToProgramCommand = new RelayCommand(() => { _NavigationService.NavigateTo(ViewNames.Program); })); }
		}

		private RelayCommand _NavigateToMessagesCommand;
		public RelayCommand NavigateToMessagesCommand
		{
			get
			{
				return _NavigateToMessagesCommand ?? (_NavigateToMessagesCommand = new RelayCommand(() =>
				{
					_NavigationService.NavigateTo(ViewNames.Messages);
				}));
			}
		}

		private RelayCommand _NavigateToSettingsCommand;
		public RelayCommand NavigateToSettingsCommand
		{
			get
			{
				return _NavigateToSettingsCommand ?? (_NavigateToSettingsCommand = new RelayCommand(() =>
				{
					_NavigationService.NavigateTo(ViewNames.Settings);
				}));
			}
		}

		private RelayCommand _NavigateToAboutCommand;
		public RelayCommand NavigateToAboutCommand
		{
			get { return _NavigateToAboutCommand ?? (_NavigateToAboutCommand = new RelayCommand(() => { _NavigationService.NavigateTo(ViewNames.About); })); }
		}

		#endregion

		public MainViewModel(
			INavigationService navigationService,
			IResourceService resourceService,
			IDialogService dialogService,
			SettingsService settingsService,
			HomeControlService homeControlService,
			RoomViewModel roomViewModel) : base(navigationService, resourceService, dialogService)
		{
			_SettingsService = settingsService;
			_HomeControlService = homeControlService;
			_RoomViewModel = roomViewModel;

			//Rooms = new ObservableCollection<Room>();

			if (IsInDesignMode)
			{
				Rooms = DesignData.GetDemoRooms();
			}
		}

		/// <summary>
		/// Refreshs the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="force">Force.</param>
		public async Task RefreshAsync(bool force = false)
		{
			if (!force && (IsLoaded || IsLoading) && !_SettingsService.HasChanges())
				return;

			IsLoaded = false;
			IsLoading = true;


			// Init SettingsService
			if (!_SettingsService.IsLoaded)
				await _SettingsService.LoadSettingsAsync();

			// Init HomeControlService
			await _HomeControlService.InitAsync(_SettingsService.Settings.Configuration, _SettingsService.Settings.IsDemoMode);

			// Load HomeMatic
			// Check connection
			if (_HomeControlService.HomeControls.ContainsKey("HomeMatic") && await _HomeControlService.HomeControls["HomeMatic"].CheckConnectionAsync())
			{
				//var rooms = await _HomeControlService.MergeRooms();
				var rooms = await _HomeControlService.HomeControls["HomeMatic"].GetRoomsWidthDevicesAsync();
				Rooms = new ObservableCollection<Room>(rooms);
				IsLoaded = true;
			}
			else
			{
				// CCU is not reachable
				Rooms = new ObservableCollection<Room>();
				await ShowConnectionErrorMessageAsync();
			}

			IsLoading = false;
		}
	}
}