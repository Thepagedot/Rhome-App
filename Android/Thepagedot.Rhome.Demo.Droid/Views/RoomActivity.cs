
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

using Android.Support.V4.Widget;
using Thepagedot.Rhome.App.Droid;
using Thepagedot.Tools.Xamarin.Android;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using JimBobBennett.MvvmLight.AppCompat;

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "Room", ParentActivity = typeof(MainActivity), NoHistory = true)]
	public class RoomActivity : AppCompatActivityBase
	{
		ListView lvDevices;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Init view
			SetContentView(Resource.Layout.Room);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);
			Title = App.Bootstrapper.RoomViewModel.CurrentRoom.Name;
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);

			// Init swipe to refresh
			var slSwipeContainer = FindViewById<SwipeRefreshLayout>(Resource.Id.slSwipeContainer);
			slSwipeContainer.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
			slSwipeContainer.Refresh += SlSwipeContainer_Refresh;

			// Init ListView
			lvDevices = FindViewById<ListView>(Resource.Id.lvDevices);
			lvDevices.Adapter = App.Bootstrapper.RoomViewModel.CurrentRoom.Devices.GetAdapter(DeviceAdapter.GetView);
		}

		protected override async void OnResume()
		{
			base.OnResume();

			if (!App.Bootstrapper.RoomViewModel.IsLoading)
			{
				await App.Bootstrapper.RoomViewModel.RefreshAsync();
				lvDevices.Adapter = App.Bootstrapper.RoomViewModel.CurrentRoom.Devices.GetAdapter(DeviceAdapter.GetView); // TODO: Solve this via clever data binding!
			}
		}

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            //ActivityS
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
		{
			//MenuInflater.Inflate(Resource.Menu.RoomMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		async void SlSwipeContainer_Refresh(object sender, EventArgs e)
		{
			await App.Bootstrapper.RoomViewModel.RefreshAsync();
			lvDevices.Adapter = App.Bootstrapper.RoomViewModel.CurrentRoom.Devices.GetAdapter(DeviceAdapter.GetView); // TODO: Solve this via clever data binding!
			(sender as SwipeRefreshLayout).Refreshing = false;
		}
	}
}