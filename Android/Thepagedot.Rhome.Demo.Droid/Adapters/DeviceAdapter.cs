using System;
using Thepagedot.Rhome.HomeMatic.Models;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using System.Linq;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.App.Droid;
using Android.App;

namespace Thepagedot.Rhome.App.Droid
{
    public class DeviceAdapter
    {
        public static View GetView(int position, Device device, View convertView)
        {
            var view = convertView ?? LayoutInflater.From(Application.Context).Inflate(Resource.Layout.Device, null);
            view.FindViewById<TextView>(Resource.Id.tvName).Text = device.Name;

            if (device is HomeMaticDevice)
            {
                var homeMaticDevice = (HomeMaticDevice)device;

                // Show low battery indicator, when at leat one channel has low battery
                if (homeMaticDevice.Channels.FirstOrDefault(c => c.IsLowBattery == true) != null)
                    view.FindViewById<ImageView>(Resource.Id.ivLowBat).Visibility = ViewStates.Visible;

                // Add channels to the list
                var channels = homeMaticDevice.Channels.Where(c => c.IsVisible).ToList();
                var adapter = new HomeMaticChannelAdapter(App.Context, 0, channels);
                var lvChannels = view.FindViewById<ListView>(Resource.Id.lvChannels);
                lvChannels.Adapter = adapter;
                ScollingHelpers.SetListViewHeightBasedOnChildren(lvChannels, 0);
            }

            return view;
        }
    }
}

