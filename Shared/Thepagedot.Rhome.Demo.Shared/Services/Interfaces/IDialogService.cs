using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thepagedot.Rhome.App.Shared.Services
{
    public interface IDialogService
    {
        Task ShowMessageDialogAsync(string title, string message);
    }
}
