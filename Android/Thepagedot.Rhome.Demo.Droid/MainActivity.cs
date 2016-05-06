using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Rhome.HomeMatic.Services;
using System.Linq;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Droid;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using JimBobBennett.MvvmLight.AppCompat;
using Thepagedot.Tools.Xamarin.Android;
using HockeyApp;
using HockeyApp.Metrics;

namespace Thepagedot.Rhome.App.Droid
{
    [Activity(Label = "Rhome", MainLauncher = true)]
    public class MainActivity : AppCompatActivityBase
    {
        DrawerLayout drawerLayout;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Register Hockey App
            CrashManager.Register(this, App.HockeyAppKey);
            MetricsManager.Register(this, Application, App.HockeyAppKey);
            UpdateManager.Register(this, App.HockeyAppKey); // Remove this for store builds!

            // Init navigation drawer
            FindViewById<NavigationView>(Resource.Id.nav_view).NavigationItemSelected += NavigationView_NavigationItemSelected;
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            // Init swipe to refresh
            var slSwipeContainer = FindViewById<SwipeRefreshLayout>(Resource.Id.slSwipeContainer);
            slSwipeContainer.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
            slSwipeContainer.Refresh += SlSwipeContainer_Refresh;

            // Init MainViewModel
            if (!App.Bootstrapper.MainViewModel.IsLoaded && !App.Bootstrapper.MainViewModel.IsLoading)
                await App.Bootstrapper.MainViewModel.RefreshAsync();

			// Init GridView
			var gvRooms = FindViewById<ExpandableHeightGridView>(Resource.Id.gvRooms);
            gvRooms.Adapter = App.Bootstrapper.MainViewModel.Rooms.GetAdapter(RoomAdapter.GetNoteView);
            gvRooms.ItemClick += GvRooms_ItemClick;
			gvRooms.IsExpanded = true;
        }

		protected override void OnResume()
		{
			base.OnResume();
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

        async void SlSwipeContainer_Refresh (object sender, EventArgs e)
		{
			var gvRooms = FindViewById<GridView>(Resource.Id.gvRooms);
			ScollingHelpers.SetListViewHeightBasedOnChildren(gvRooms, Resources.GetDimension(Resource.Dimension.default_margin));
            await App.Bootstrapper.MainViewModel.RefreshAsync();
            (sender as SwipeRefreshLayout).Refreshing = false;
        }

        void GvRooms_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            var room = App.Bootstrapper.MainViewModel.Rooms.ElementAt(e.Position);
            App.Bootstrapper.MainViewModel.NavigateToRoomCommand.Execute(room);
        }

        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();

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