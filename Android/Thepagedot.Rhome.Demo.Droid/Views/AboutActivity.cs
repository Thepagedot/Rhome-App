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
using Android.Support.V7.App;

namespace Thepagedot.Rhome.App.Droid.Views
{
    [Activity(Label = "About", ParentActivity = typeof(MainActivity))]
    public class AboutActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.About);

            // Init toolbar
            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // Set version according to the AppManifest
            var tvVersion = FindViewById<TextView>(Resource.Id.tvVersion);
            tvVersion.Text = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
        }
    }
}