using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Thepagedot.Rhome.App.Shared.Services;

namespace Thepagedot.Rhome.App.Droid.Services
{
    public class ResourceService : IResourceService
    {
        public string GetString(string key)
        {
            var resId = App.Context.Resources.GetIdentifier(key, "string", App.Context.PackageName);
            return App.Context.GetString(resId);
        }
    }
}