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
using Thepagedot.Tools.Xamarin.Android;
using Android.Support.V4.Widget;

namespace Thepagedot.Rhome.App.Droid.Views
{
    [Activity(Label = "Messages", ParentActivity = typeof(MainActivity))]
    public class MessagesActivity : AppCompatActivity
    {
        ListView lvMessages;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Messages);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);

            // Init swipe to refresh
            var slSwipeContainer = FindViewById<SwipeRefreshLayout>(Resource.Id.slSwipeContainer);
            slSwipeContainer.SetColorSchemeResources(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
            slSwipeContainer.Refresh += SlSwipeContainer_Refresh;

            lvMessages = FindViewById<ListView>(Resource.Id.lvMessages);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MessagesMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        private async void SlSwipeContainer_Refresh(object sender, EventArgs e)
        {
            await App.Bootstrapper.MessageViewModel.RefreshAsync();
            //lvDevices.Adapter = App.Bootstrapper.RoomViewModel.CurrentRoom.Devices.GetAdapter(DeviceAdapter.GetView); // TODO: Solve this via clever data binding!
            (sender as SwipeRefreshLayout).Refreshing = false;
        }
    }
}