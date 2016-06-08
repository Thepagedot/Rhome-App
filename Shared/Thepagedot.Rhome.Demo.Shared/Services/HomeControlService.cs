using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Models;
using Thepagedot.Rhome.Base.Interfaces;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Rhome.HomeMatic.Services;

namespace Thepagedot.Rhome.App.Shared.Services
{
	public class HomeControlService
	{
		private readonly SettingsService _SettingsService;

		public Dictionary<string, IHomeControlAdapter> Platforms;

		public HomeControlService(SettingsService settingsService)
		{
			_SettingsService = settingsService;
		}

		/// <summary>
		/// Initialized the HomeControlService with or without a previously loaded Configuration instance
		/// </summary>
		/// <returns></returns>
		/// <param name="config">Configuration instance</param>
		public async Task InitAsync(Configuration config = null, bool isDemoMode = false)
		{
			// Load config from SettingsService, if none has been provided
			if (config == null)
			{
				if (!_SettingsService.IsLoaded)
					await _SettingsService.LoadSettingsAsync();

				config = _SettingsService.Settings.Configuration;
			}

			// Load central units
			Platforms = new Dictionary<string, IHomeControlAdapter>();
			if (config.CentralUnits != null)
			{
				if (isDemoMode)
				{
					// Demo Mode
					Platforms.Add("HomeMatic", new HomeMaticXmlApiAdapter(new Ccu("Mock", "localhost"), true));
				}
				else
				{
					// HomeMatic
					var homeMaticCentral = config.CentralUnits.FirstOrDefault(c => c.Brand == Base.Models.CentralUnitBrand.HomeMatic);
					if (homeMaticCentral != null && homeMaticCentral is Ccu)
						Platforms.Add("HomeMatic", new HomeMaticXmlApiAdapter(homeMaticCentral as Ccu));
				}
			}
		}
	}
}
