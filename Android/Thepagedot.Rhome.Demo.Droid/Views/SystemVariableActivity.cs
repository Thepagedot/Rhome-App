
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
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SystemVariables);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);

            var lvSystemVariables = FindViewById<ListView>(Resource.Id.lvSystemVariables);
            lvSystemVariables.Adapter = App.Bootstrapper.SystemVariableViewModel.SystemVariables.GetAdapter(SystemVariableAdapter.GetView);
		}
	}
}

