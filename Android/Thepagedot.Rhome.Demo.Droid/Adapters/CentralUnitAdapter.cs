using System;
using Thepagedot.Rhome.Base.Interfaces;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.App.Droid;
using Android.App;

namespace Thepagedot.Rhome.App.Droid
{
    public class CentralUnitAdapter
    {
        public static View GetView(int position, CentralUnit centralUnit, View convertView)
        {
            var view = convertView ?? LayoutInflater.From(Application.Context).Inflate(Resource.Layout.HomeControlApiItem, null);

            view.FindViewById<TextView>(Resource.Id.tvName).Text = centralUnit.Name;
            view.FindViewById<TextView>(Resource.Id.tvAddress).Text = centralUnit.Address;
            view.FindViewById<TextView>(Resource.Id.tvBrand).Text = centralUnit.Brand.ToString();

            return view;
        }
    }
}