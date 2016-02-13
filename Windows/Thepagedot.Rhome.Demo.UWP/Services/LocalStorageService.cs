using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Models;
using Thepagedot.Rhome.App.Shared.Services;
using Windows.Storage;

namespace Thepagedot.Rhome.App.UWP.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly JsonSerializerSettings _JsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };

        public async Task SaveToFileAsync(string fileName, object content)
        {
            var json = JsonConvert.SerializeObject(content, Formatting.Indented, _JsonSerializerSettings);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, json);
        }

        public async Task<T> LoadFromFileAsync<T>(string fileName)
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                var json = await FileIO.ReadTextAsync(file);

                T content = JsonConvert.DeserializeObject<T>(json, _JsonSerializerSettings);
                return content;
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
            catch (JsonException)
            {
                return default(T);
            }
        }
    }
}
