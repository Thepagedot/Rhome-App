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
        private HomeControlService _HomeControlService;
        private readonly string _ConfigFileName = "configuration.json";

        public Configuration Configuration { get; set; }
        public bool IsLoaded { get; set; }


        public SettingsService(ILocalStorageService localStorageService, HomeControlService homeControlService)
        {
            _LocalStorageService = localStorageService;
            _HomeControlService = homeControlService;
        }

        public void Refresh()
        {
            // Reset
            _HomeControlService.HomeMatic = null;

            // Load central units
            if (Configuration.CentralUnits != null)
            {
                // HomeMatic
                var homeMaticCentral = Configuration.CentralUnits.FirstOrDefault(c => c.Brand == Base.Models.CentralUnitBrand.HomeMatic);
                if (homeMaticCentral != null && homeMaticCentral is Ccu)
                    _HomeControlService.HomeMatic = new HomeMatic.Services.HomeMaticXmlApi(homeMaticCentral as Ccu);
            }
        }

        public async Task LoadSettingsAsync()
        {
            var configuration = await _LocalStorageService.LoadFromFileAsync<Configuration>(_ConfigFileName);
            if (configuration != null)
            {
                Configuration = configuration;
                Refresh();
            }
            else
            {
                Configuration = new Configuration();
            }

            IsLoaded = true;
        }

        public async Task SaveSettingsAsync()
        {
            if (Configuration != null)
                await _LocalStorageService.SaveToFileAsync(_ConfigFileName, Configuration);
        }
    }
}
