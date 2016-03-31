
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
using Thepagedot.Tools.Xamarin.Android;

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "ProgramActivity")]			
	public class ProgramActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);
			// Create your application here
		}
	}
}

