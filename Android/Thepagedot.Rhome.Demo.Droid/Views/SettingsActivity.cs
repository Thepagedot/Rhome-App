
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

using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Net;
using System.Threading.Tasks;
using Thepagedot.Rhome.HomeMatic.Services;
using Thepagedot.Rhome.HomeMatic.Models;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.App.Droid;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using JimBobBennett.MvvmLight.AppCompat;
using Thepagedot.Rhome.App.Shared.ViewModels;
using Thepagedot.Tools.Xamarin.Android;

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "Settings", ParentActivity = typeof(MainActivity))]
	public class SettingsActivity : AppCompatActivityBase
	{
		// ViewModel
		//public SettingsViewModel SettingsViewModel { get; set; }
		public SettingsViewModel SettingsViewModel
		{
			get
			{
				return App.Bootstrapper.SettingsViewModel;
			}
		}

		// Public UI elements for binding
		public EditText EtName { get; set; }
		public EditText EtIpAddress { get; set; }
		public Switch SwDemoMode { get; set; }
		public ListView LvCentralUnits { get; set; }

		// Bindings
		private Binding EtNameBinding;
		private Binding EtIpAddressBinding;
		public Binding SwDemoModeBinding;

		protected override async void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Init View
			SetContentView(Resource.Layout.Settings);
			SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);

			//SettingsViewModel = App.Bootstrapper.SettingsViewModel;
			await SettingsViewModel.InitializeAsync();

			SwDemoMode = FindViewById<Switch>(Resource.Id.swDemoMode);
			SwDemoModeBinding = this.SetBinding(() => SettingsViewModel.IsDemoMode, () => SwDemoMode.Selected, BindingMode.TwoWay);

			SettingsViewModel.IsDemoMode = true;

			// Init ListView of Central Units
			LvCentralUnits = FindViewById<ListView>(Resource.Id.lvCentralUnits);
			LvCentralUnits.Adapter = App.Bootstrapper.SettingsViewModel.CentralUnits.GetAdapter(CentralUnitAdapter.GetView);
			LvCentralUnits.ItemLongClick += LvCentralUnits_ItemLongClick;
		}

		private void LvCentralUnits_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			var centralUnitToDelete = App.Bootstrapper.SettingsViewModel.CentralUnits.ElementAt(e.Position);
			if (centralUnitToDelete != null)
			{
				var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
				builder.SetMessage(Resource.String.confirm_delete_message);
				builder.SetPositiveButton(Resource.String.delete, (ev, se) => App.Bootstrapper.SettingsViewModel.DeleteCentralUnitCommand.Execute(centralUnitToDelete));
				builder.SetNegativeButton(Android.Resource.String.Cancel, (ev, se) => { });
				builder.Show();
			}
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.SettingsMenu, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.menu_add:
					ShowAddEditDialog(null);
					break;
			}

			return base.OnOptionsItemSelected(item);
		}

		private void ShowAddEditDialog(CentralUnit centralUnit)
		{
			var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
			var dialogView = LayoutInflater.Inflate(Resource.Layout.AddHomeControlSystemDialog, null);
			builder.SetView(dialogView);
			builder.SetNeutralButton(Android.Resource.String.Cancel, (sender, e) => { });
			builder.SetPositiveButton(Android.Resource.String.Ok, (e, s) => App.Bootstrapper.SettingsViewModel.AddCentralUnitCommand.Execute(null));

			// Bindings
			EtName = dialogView.FindViewById<EditText>(Resource.Id.etName);
			EtNameBinding = this.SetBinding(() => SettingsViewModel.NewCentralUnitName, () => EtName.Text, BindingMode.TwoWay);
			EtIpAddress = dialogView.FindViewById<EditText>(Resource.Id.etIpAddress);
			EtIpAddressBinding = this.SetBinding(() => SettingsViewModel.NewCentralUnitAddress, () => EtIpAddress.Text, BindingMode.TwoWay);

			// Fill spinner
			var items = Enum.GetValues(typeof(CentralUnitBrand)).OfType<object>().Select(s => s.ToString()).ToArray();
			var spinnerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);
			spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			dialogView.FindViewById<Spinner>(Resource.Id.spBrand).Adapter = spinnerAdapter;

			builder.Show();
		}
	}
}