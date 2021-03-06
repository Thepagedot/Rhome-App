﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.HomeMatic.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Thepagedot.Tools;

namespace Thepagedot.Rhome.App.Shared.ViewModels
{
	public class SettingsViewModel : AsyncViewModelBase
	{
		private SettingsService _SettingsService;
		private HomeControlService _HomeControlService;

		private ObservableCollection<CentralUnit> _CentralUnits;
		public ObservableCollection<CentralUnit> CentralUnits
		{
			get { return _CentralUnits; }
			set { _CentralUnits = value; RaisePropertyChanged(); }
		}

		private bool _IsDemoMode;
		public bool IsDemoMode
		{
			get { return _IsDemoMode; }
			set
			{
				_IsDemoMode = value;
				RaisePropertyChanged();
			}
		}

		#region New CentralUnit fields

		private string _NewCentralUnitName;
		public string NewCentralUnitName
		{
			get { return _NewCentralUnitName; }
			set { _NewCentralUnitName = value; RaisePropertyChanged(); }
		}

		private string _NewCentralUnitAddress;
		public string NewCentralUnitAddress
		{
			get { return _NewCentralUnitAddress; }
			set { _NewCentralUnitAddress = value; RaisePropertyChanged(); }
		}

		private CentralUnitBrand _NewCentralUnitBrand;
		public CentralUnitBrand NewCentralUnitBrand
		{
			get { return _NewCentralUnitBrand; }
			set { _NewCentralUnitBrand = value; RaisePropertyChanged(); }
		}

		#endregion

		private RelayCommand _AddCentralUnitCommand;
		public RelayCommand AddCentralUnitCommand
		{
			get
			{
				return _AddCentralUnitCommand ?? (_AddCentralUnitCommand = new RelayCommand(async () =>
				{
					// Create new central unit depending on the selected brand
					CentralUnit centralUnit;
					switch (NewCentralUnitBrand)
					{
						default:
						case CentralUnitBrand.HomeMatic:
							centralUnit = new Ccu(NewCentralUnitName, NewCentralUnitAddress); break;
					}

					await AddCentralUnitAsync(centralUnit);
					ResetNewCentralUnitValues();
				}));
			}
		}

		private RelayCommand<CentralUnit> _DeleteCentralUnitCommand;
		public RelayCommand<CentralUnit> DeleteCentralUnitCommand
		{
			get
			{
				return _DeleteCentralUnitCommand ?? (_DeleteCentralUnitCommand = new RelayCommand<CentralUnit>(async (centralUnit) =>
				{
					await DeleteCentralUnitAsync(centralUnit);
				}));
			}
		}

		public SettingsViewModel(
			INavigationService navigationService,
			IResourceService resourceService,
			IDialogService dialogService,
			SettingsService settingsService,
			HomeControlService homeControlService) : base(navigationService, resourceService, dialogService)
		{
			_SettingsService = settingsService;
			_HomeControlService = homeControlService;

			CentralUnits = new ObservableCollection<CentralUnit>();
			ResetNewCentralUnitValues();
		}

		public async Task InitializeAsync()
		{
			if (!_SettingsService.IsLoaded)
				await _SettingsService.LoadSettingsAsync();

			CentralUnits = new ObservableCollection<CentralUnit>(_SettingsService.Settings.Configuration.CentralUnits);
			IsDemoMode = true; //_SettingsService.Settings.IsDemoMode;
		}

		private void ResetNewCentralUnitValues()
		{
			NewCentralUnitName = "";
			NewCentralUnitAddress = "";
			NewCentralUnitBrand = 0;
		}

		public async Task SaveSettingsAsync()
		{
			await _SettingsService.SaveSettingsAsync();
		}

		public async Task AddCentralUnitAsync(CentralUnit centralUnit)
		{
			// Add central unit to list and configuration
			CentralUnits.Add(centralUnit);

			_SettingsService.Settings.Configuration.CentralUnits.Add(centralUnit);
			await _HomeControlService.InitAsync(_SettingsService.Settings.Configuration);

			// Save settings
			await _SettingsService.SaveSettingsAsync();
		}

		public async Task DeleteCentralUnitAsync(CentralUnit centralUnit)
		{
			// Delete central unit from list and configuration
			CentralUnits.Remove(centralUnit);
			_SettingsService.Settings.Configuration.CentralUnits.Remove(centralUnit);
			await _HomeControlService.InitAsync(_SettingsService.Settings.Configuration);

			// Save settings
			await _SettingsService.SaveSettingsAsync();
		}
	}
}