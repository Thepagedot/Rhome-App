using System;
using Android.OS;
using Android.App;
using Android.Views;

namespace Thepagedot.Rhome.App.Droid
{
	public static class AppCompatHelper
	{
		/// <summary>
		/// Extension method for activites to apply version specific settings globally
		/// </summary>
		/// <param name="activity">Activity.</param>
		public static void CompatMode(this Activity activity)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) 
			{
				// Set status bar color
				activity.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
				activity.Window.SetStatusBarColor(activity.Resources.GetColor(Resource.Color.HomeMaticBlue));
			}
		}
	}
}

