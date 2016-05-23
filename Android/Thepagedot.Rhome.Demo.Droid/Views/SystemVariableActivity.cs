
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
using GalaSoft.MvvmLight.Helpers;
using Android.Support.V7.App;

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "System Variables", ParentActivity = typeof(MainActivity))]
	public class SystemVariableActivity : AppCompatActivity
	{
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Init view
			SetContentView(Resource.Layout.SystemVariables);
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);

			// Init system variable list view
			if (!App.Bootstrapper.SystemVariableViewModel.IsLoaded && !App.Bootstrapper.SystemVariableViewModel.IsLoading)
				await App.Bootstrapper.SystemVariableViewModel.RefreshAsync();

			FindViewById<ListView>(Resource.Id.lvSystemVariables).Adapter = App.Bootstrapper.SystemVariableViewModel.SystemVariables.GetAdapter(SystemVariableAdapter.GetView);
		}
	}
}

