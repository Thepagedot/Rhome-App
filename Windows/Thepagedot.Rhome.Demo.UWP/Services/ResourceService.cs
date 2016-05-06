using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Tools;
using Windows.ApplicationModel.Resources;

namespace Thepagedot.Rhome.App.UWP.Services
{
    public class ResourceService : IResourceService
    {
        public string GetString(string key)
        {
            var resLoader = new ResourceLoader();
            return resLoader.GetString(key);
        }
    }
}
