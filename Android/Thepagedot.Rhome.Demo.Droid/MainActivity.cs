using System;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using System.Linq;
using GalaSoft.MvvmLight.Helpers;
using JimBobBennett.MvvmLight.AppCompat;
using Thepagedot.Tools.Xamarin.Android;
using HockeyApp;

using Thepagedot.Rhome.App.Shared.ViewModels;

#if (!DEBUG)
using HockeyApp.Metrics;
#endif

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "Rhome Alpha", MainLauncher = true)]
	public class MainActivity : AppCompatActivityBase
	{
		// ViewModel
		public MainViewModel MainViewModel { get; set; }

		// Public UI elements for binding
		public SwipeRefreshLayout SlSwipeRefreshLayout { get; set; }
		public DrawerLayout DrawerLayout { get; set; }

		// Bindings
		public Binding RefreshBinding { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Init view
			SetContentView(Resource.Layout.Main);
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);
			DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

			MainViewModel = App.Bootstrapper.MainViewModel;

#if (!DEBUG)
            // Register Hockey App if not in Debug mode
            CrashManager.Register(this, App.HockeyAppKey);
            MetricsManager.Register(this, Application, App.HockeyAppKey);
            UpdateManager.Register(this, App.HockeyAppKey); // Remove this for store builds!
#endif

			// Init navigation drawer
			FindViewById<NavigationView>(Resource.Id.nav_view).NavigationItemSelected += NavigationView_NavigationItemSelected;
			var drawerToggle = new ActionBarDrawerToggle(this, DrawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
			DrawerLayout.AddDrawerListener(drawerToggle);
			drawerToggle.SyncState();

			// Init swipe to refresh
			SlSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.slSwipeContainer);
			SlSwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
			SlSwipeRefreshLayout.Refresh += SlSwipeContainer_Refresh;
			SlSwipeRefreshLayout.Post(() => { RefreshBinding = this.SetBinding(() => MainViewModel.IsLoading, () => SlSwipeRefreshLayout.Refreshing, BindingMode.TwoWay); });
		}

		protected override async void OnResume()
		{
			base.OnResume();

			await MainViewModel.RefreshAsync();

			// Init GridView (after ViewModel is loaded)
			var gvRooms = FindViewById<ExpandableHeightGridView>(Resource.Id.gvRooms);
			gvRooms.Adapter = MainViewModel.Rooms.GetAdapter(RoomAdapter.GetView);
            var a = App.Bootstrapper.SystemVariableViewModel.SystemVariables.GetAdapter(SystemVariableAdapter.GetView);
            var b = App.Bootstrapper.MainViewModel.Rooms.GetAdapter(RoomAdapter.GetView);

            gvRooms.ItemClick += GvRooms_ItemClick;
			gvRooms.IsExpanded = true;
		}

		protected override void OnPause()
		{
			base.OnPause();
			UpdateManager.Unregister();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			UpdateManager.Unregister();
		}

		async void SlSwipeContainer_Refresh(object sender, EventArgs e)
		{
			var gvRooms = FindViewById<GridView>(Resource.Id.gvRooms);
			//ScollingHelpers.SetListViewHeightBasedOnChildren(gvRooms, Resources.GetDimension(Resource.Dimension.default_margin)); //TODO: Check if still needed
			await App.Bootstrapper.MainViewModel.RefreshAsync(true);
			(sender as SwipeRefreshLayout).Refreshing = false;
		}

		void GvRooms_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			// User selected a room. Navigate to it.
			var room = MainViewModel.Rooms.ElementAt(e.Position);
			MainViewModel.NavigateToRoomCommand.Execute(room);
		}

		void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
		{
			DrawerLayout.CloseDrawers();

			switch (e.MenuItem.ItemId)
			{
				default: Toast.MakeText(this, e.MenuItem.TitleFormatted + " not implemented yet.", ToastLength.Short).Show(); break;
				case Resource.Id.nav_settings: App.Bootstrapper.MainViewModel.NavigateToSettingsCommand.Execute(null); break;
				case Resource.Id.nav_system_variables: App.Bootstrapper.MainViewModel.NavigateToSystemVariableCommand.Execute(null); break;
				case Resource.Id.nav_programs: App.Bootstrapper.MainViewModel.NavigateToProgramCommand.Execute(null); break;
				case Resource.Id.nav_messages: App.Bootstrapper.MainViewModel.NavigateToMessagesCommand.Execute(null); break;
				case Resource.Id.nav_about: App.Bootstrapper.MainViewModel.NavigateToAboutCommand.Execute(null); break;
			}
		}
	}
}