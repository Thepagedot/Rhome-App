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

namespace Thepagedot.Rhome.App.Droid
{
	public class App : Application
	{
		public static readonly string HockeyAppKey = "cd1d9c28c27b48cf9cd9179d17496a5a";

		public static Bootstrapper Bootstrapper = new Bootstrapper();
	}
}