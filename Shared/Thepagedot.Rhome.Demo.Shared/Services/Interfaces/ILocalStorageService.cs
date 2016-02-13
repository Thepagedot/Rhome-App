using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Models;

namespace Thepagedot.Rhome.App.Shared.Services
{
    public interface ILocalStorageService
    {
        Task SaveToFileAsync(string fileName, object content);
        Task<T> LoadFromFileAsync<T>(string fileName);
    }
}
