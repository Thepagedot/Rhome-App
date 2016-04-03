
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
using Thepagedot.Tools.Xamarin.Android;
using Android.Support.V7.App;
using GalaSoft.MvvmLight.Helpers;

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "Programs", ParentActivity = typeof(MainActivity))]
	public class ProgramActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProgramItem);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);

            var lvPrograms = FindViewById<ListView>(Resource.Id.lvPrograms);
            lvPrograms.Adapter = App.Bootstrapper.ProgramViewModel.Programs.GetAdapter(ProgramAdapter.GetView);
        }
	}
}

