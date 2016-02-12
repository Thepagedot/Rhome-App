using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Services;
using Windows.UI.Popups;

namespace Thepagedot.Rhome.App.UWP.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowMessageDialogAsync(string title, string message)
        {
            var dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
        }
    }
}
