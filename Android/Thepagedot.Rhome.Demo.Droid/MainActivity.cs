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
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Rhome.HomeMatic.Services;
using System.Linq;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Droid;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using JimBobBennett.MvvmLight.AppCompat;

namespace Thepagedot.Rhome.App.Droid
{
    [Activity(Label = "Rhome", MainLauncher = true)]
    public class MainActivity : AppCompatActivityBase
    {
        DrawerLayout drawerLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
			this.CompatMode();
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init navigation drawer
            FindViewById<NavigationView>(Resource.Id.nav_view).NavigationItemSelected += NavigationView_NavigationItemSelected;
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            // Init swipe to refresh
            var slSwipeContainer = FindViewById<SwipeRefreshLayout>(Resource.Id.slSwipeContainer);
            slSwipeContainer.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
            slSwipeContainer.Refresh += SlSwipeContainer_Refresh;

            // Init GridView
            var gvRooms = FindViewById<GridView>(Resource.Id.gvRooms);
            gvRooms.Adapter = App.Bootstrapper.MainViewModel.Rooms.GetAdapter(RoomAdapter.GetNoteView);
            gvRooms.ItemClick += GvRooms_ItemClick;

			ScollingHelpers.SetListViewHeightBasedOnChildren(gvRooms, Resources.GetDimension(Resource.Dimension.default_margin));
        }

		protected override async void OnResume()
		{
			base.OnResume();

            // Init MainViewModel
            if (!App.Bootstrapper.MainViewModel.IsLoaded && !App.Bootstrapper.MainViewModel.IsLoading)
                await App.Bootstrapper.MainViewModel.RefreshAsync();

            var gvRooms = FindViewById<GridView>(Resource.Id.gvRooms);
			ScollingHelpers.SetListViewHeightBasedOnChildren(gvRooms, Resources.GetDimension(Resource.Dimension.default_margin));
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
                default:
                    Toast.MakeText(this, e.MenuItem.TitleFormatted + " clicked.", ToastLength.Short).Show();
                    break;
                case Resource.Id.nav_settings:
                    App.Bootstrapper.MainViewModel.NavigateToSettingsCommand.Execute(null);
                    break;
            }
        }
    }
}