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
			var view = convertView ?? LayoutInflater.From(Application.Context).Inflate(Resource.Layout.ProgramItem, null);

			view.FindViewById<TextView>(Resource.Id.tvName).Text = program.Name;
			view.FindViewById<TextView>(Resource.Id.tvDescription).Text = program.Description;
            view.FindViewById<Button>(Resource.Id.btnRun).Click += (s, e) => App.Bootstrapper.ProgramViewModel.RunCommand.Execute(program);

            return view;
		}
	}
}