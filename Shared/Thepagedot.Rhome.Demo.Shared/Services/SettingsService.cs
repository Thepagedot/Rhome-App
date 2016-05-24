using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Models;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Tools;

namespace Thepagedot.Rhome.App.Shared.Services
{
	public class SettingsService
	{
		private ILocalStorageService _LocalStorageService;
		private readonly string _SettingsFileName = "settings.json";

		public Settings Settings { get; set; }
		public bool IsLoaded { get; set; }
		private bool _HasChanges { get; set; }

		public SettingsService(ILocalStorageService localStorageService)
		{
			_LocalStorageService = localStorageService;
		}

		public async Task LoadSettingsAsync()
		{
			var settings = await _LocalStorageService.LoadFromFileAsync<Settings>(_SettingsFileName);
			if (settings != null)
			{
				Settings = settings;
			}
			else
			{
				Settings = new Settings();
			}

			IsLoaded = true;
		}

		public async Task SaveSettingsAsync()
		{
			if (Settings != null)
				await _LocalStorageService.SaveToFileAsync(_SettingsFileName, Settings);

			_HasChanges = true;
		}

		public bool HasChanges()
		{
			var result = _HasChanges;
			_HasChanges = false;
			return result;
		}
	}
}
