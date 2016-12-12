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
using HockeyApp;

namespace Thepagedot.Rhome.App.Droid.Views
{
	[Activity(Label = "About", ParentActivity = typeof(MainActivity))]
	public class AboutActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Init view
			SetContentView(Resource.Layout.About);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);
			SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar));
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);

			// Register Hockey App Feedback
			FeedbackManager.Register(this, App.HockeyAppKey);

			// Set version according to the AppManifest
			var tvVersion = FindViewById<TextView>(Resource.Id.tvVersion);
			tvVersion.Text = PackageManager.GetPackageInfo(PackageName, 0).VersionName;

			// Feedback button
			var btnFeedback = FindViewById<Button>(Resource.Id.btnFeedback);
			btnFeedback.Click += (sender, e) => FeedbackManager.ShowFeedbackActivity(this);

            // Simulate crash button
            var btnSimulateCrash = FindViewById<Button>(Resource.Id.btnSimulateCrash);
            btnSimulateCrash.Click += (sender, e) => { throw new Exception("Test Exception"); };
		}
	}
}