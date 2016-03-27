using System;
using Thepagedot.Rhome.HomeMatic.Models;
using Android.Views;
using Android.Widget;
using Android.App;

namespace Thepagedot.Rhome.App.Droid
{
	public class ProgramAdapter
	{
		public static View GetView(int position, Program program, View convertView)
		{
			var view = convertView ?? LayoutInflater.From(Application.Context).Inflate(Resource.Layout.SystemVariable, null);

			view.FindViewById<TextView>(Resource.Id.tvName).Text = program.Name;
			//view.FindViewById<TextView>(Resource.Id.btnStart) = 

			return view;
		}
	}
}