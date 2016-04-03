using System;
using Thepagedot.Rhome.HomeMatic.Models;
using Android.Views;
using Android.App;
using Android.Widget;

namespace Thepagedot.Rhome.App.Droid
{
	public class SystemVariableAdapter
	{
		public static View GetView(int position, SystemVariable systemVariable, View convertView)
		{
			var view = convertView ?? LayoutInflater.From(Application.Context).Inflate(Resource.Layout.SystemVariableItem, null);

			view.FindViewById<TextView>(Resource.Id.tvName).Text = systemVariable.Name;
            view.FindViewById<TextView>(Resource.Id.tvValue).Text = systemVariable.ValueString;

			return view;
		}
	}
}