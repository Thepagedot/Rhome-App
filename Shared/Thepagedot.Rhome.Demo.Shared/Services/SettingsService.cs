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


		public SettingsService(ILocalStorageService localStorageService)
		{
			_LocalStorageService = localStorageService;
		}

		public async Task LoadSettingsAsync()
		{
			var configuration = await _LocalStorageService.LoadFromFileAsync<Settings>(_SettingsFileName);
			if (configuration != null)
			{
				Settings = configuration;
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
		}
	}
}
