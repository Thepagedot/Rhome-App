
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
using JimBobBennett.MvvmLight.AppCompat;
using Android.Support.V7.App;
using GalaSoft.MvvmLight.Helpers;

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "Programs", ParentActivity = typeof(MainActivity))]
	public class ProgramActivity : AppCompatActivity
    {
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Init view
			SetContentView(Resource.Layout.Programs);
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);

			// Init program listview
			if (!App.Bootstrapper.ProgramViewModel.IsLoaded && !App.Bootstrapper.ProgramViewModel.IsLoading)
				await App.Bootstrapper.ProgramViewModel.RefreshAsync();
			FindViewById<ListView>(Resource.Id.lvPrograms).Adapter = App.Bootstrapper.ProgramViewModel.Programs.GetAdapter(ProgramAdapter.GetView);
		}
	}
}