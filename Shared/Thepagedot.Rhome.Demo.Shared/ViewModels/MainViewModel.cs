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
using Thepagedot.Rhome.Base.Services;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
	public class MainViewModel : AsyncViewModelBase
	{
		private SettingsService _SettingsService;
		private HomeControlService _HomeControlService;
		private RoomService _RoomService;
        private RoomViewModel _RoomViewModel;

		private ObservableCollection<MergedRoom> _Rooms;
		public ObservableCollection<MergedRoom> Rooms
		{
			get { return _Rooms; }
			set { _Rooms = value; RaisePropertyChanged(); }
		}

		private string _StatusMessage;
		public string StatusMessage
		{
			get { return _StatusMessage; }
			set { _StatusMessage = value; RaisePropertyChanged(); }
		}

		private RelayCommand _RefreshCommand;
		public RelayCommand RefreshCommand
		{
			get
			{
				return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(async () =>
				{
					await RefreshAsync(true);
				}));
			}
		}

		#region Navigation Commands

		private RelayCommand<MergedRoom> _NavigateToRoomCommand;
		public RelayCommand<MergedRoom> NavigateToRoomCommand
		{
			get
			{
				return _NavigateToRoomCommand ?? (_NavigateToRoomCommand = new RelayCommand<MergedRoom>((MergedRoom room) =>
				{
					// TODO: Check if linking to the RoomViewModel may be a bad practice.
					// Alternative would be to navigate to the room page with the current room as parameter
					// The room page then can set the current room of the room view model
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
            RoomService roomService,
            RoomViewModel roomViewModel) : base(navigationService, resourceService, dialogService)
		{
			_SettingsService = settingsService;
			_HomeControlService = homeControlService;
            _RoomService = roomService;
            _RoomViewModel = roomViewModel;

			Rooms = new ObservableCollection<MergedRoom>();

			if (IsInDesignMode)
			{
				Rooms = DesignData.GetDemoRooms();
			}
		}

		/// <summary>
		/// Refreshes the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="force">Force.</param>
		public async Task RefreshAsync(bool force = false)
		{
			if (!force && (IsLoaded || IsLoading) && !_SettingsService.HasChanges())
				return;

			IsLoaded = false;
			IsLoading = true;

			var connectionErrorOccured = false;

			// Init SettingsService
			if (!_SettingsService.IsLoaded)
			{
				StatusMessage = _ResourceService.GetString("status_loading_settings");
				await _SettingsService.LoadSettingsAsync();
			}

            // Init HomeControlService
			await _HomeControlService.InitAsync(_SettingsService.Settings.Configuration, _SettingsService.Settings.IsDemoMode);

            // Load data if any platforms are available
            if (_HomeControlService.Platforms.Any())
            {
                var rooms = new List<Room>();

                foreach (var platform in _HomeControlService.Platforms)
                {
                    StatusMessage = string.Format(_ResourceService.GetString("status_loading_platform"), platform.Value.GetName());

                    // Check connection
                    if (await platform.Value.CheckConnectionAsync())
                    {
						// Load rooms
                        var platformRooms = await platform.Value.GetRoomsWidthDevicesAsync();
                        rooms.AddRange(platformRooms);
                        IsLoaded = true;
                    }
                    else
                    {
                        // CCU is not reachable
                        Rooms.Clear();
                        IsLoaded = false;
						connectionErrorOccured = true;
                    }
                }

				if (connectionErrorOccured)
				{
					await ShowConnectionErrorMessageAsync();
				}

                var mergedRooms = _RoomService.MergeRooms(rooms);
                Rooms.Clear();
                foreach (var room in mergedRooms)
                    Rooms.Add(room);
            }
            else
            {
                // Clear list, if no platform is available. Especially needed, after switching off demo mode.
                Rooms.Clear();
            }

			// Update status message
			if (!Rooms.Any())
			{
				StatusMessage = _ResourceService.GetString("status_not_connected");
			}
			else if (connectionErrorOccured)
			{
				StatusMessage = _ResourceService.GetString("status_partially_connected");
			}
			else
			{
				StatusMessage = _ResourceService.GetString("status_connected");
			}

			if (_SettingsService.Settings.IsDemoMode)
			{
				// Override with "Demo Mode"
				StatusMessage = _ResourceService.GetString("demo_mode");
			}

            IsLoading = false;
		}
	}
}