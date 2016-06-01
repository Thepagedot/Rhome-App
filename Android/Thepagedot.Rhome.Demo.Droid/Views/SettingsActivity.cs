
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
using Thepagedot.Tools.Xamarin.Android.Converters;
using Android.Support.Design.Widget;

namespace Thepagedot.Rhome.App.Droid
{
	[Activity(Label = "Settings", ParentActivity = typeof(MainActivity))]
	public class SettingsActivity : AppCompatActivityBase
	{
		// ViewModel
		public SettingsViewModel SettingsViewModel { get; set; }

		// Public UI elements for binding
		public EditText EtName { get; set; }
		public EditText EtIpAddress { get; set; }
		public Switch SwDemoMode { get; set; }
		public ListView LvCentralUnits { get; set; }
        public TextView TvHomeSystemsEmpty { get; set; }

        // Bindings
        private readonly List<Binding> _Bindings = new List<Binding>();

        protected override async void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Init View
			SetContentView(Resource.Layout.Settings);
			SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			this.SetSystemBarBackground(Resource.Color.HomeMaticBlue);

			SettingsViewModel = App.Bootstrapper.SettingsViewModel;
			await SettingsViewModel.InitializeAsync();

			SwDemoMode = FindViewById<Switch>(Resource.Id.swDemoMode);
			_Bindings.Add(this.SetBinding(() => SettingsViewModel.IsDemoMode, () => SwDemoMode.Checked, BindingMode.TwoWay));

            TvHomeSystemsEmpty = FindViewById<TextView>(Resource.Id.tvHomeSystemsEmpty);
            _Bindings.Add(this.SetBinding(() => SettingsViewModel.CentralUnits.Count, () => TvHomeSystemsEmpty.Visibility).ConvertSourceToTarget(BoolToNegatedVisibilityConverter.Convert));

			var btnAddHomeControl = FindViewById<FloatingActionButton>(Resource.Id.btnAddHomeControl);
			btnAddHomeControl.Click += (sender, e) => ShowAddEditDialog(null);

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

		private void ShowAddEditDialog(CentralUnit centralUnit)
		{
			var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
			var dialogView = LayoutInflater.Inflate(Resource.Layout.AddHomeControlSystemDialog, null);
			builder.SetView(dialogView);
			builder.SetNeutralButton(Android.Resource.String.Cancel, (sender, e) => { });
			builder.SetPositiveButton(Android.Resource.String.Ok, (e, s) => App.Bootstrapper.SettingsViewModel.AddCentralUnitCommand.Execute(null));

			// Bindings
			EtName = dialogView.FindViewById<EditText>(Resource.Id.etName);
            _Bindings.Add(this.SetBinding(() => SettingsViewModel.NewCentralUnitName, () => EtName.Text, BindingMode.TwoWay));
			EtIpAddress = dialogView.FindViewById<EditText>(Resource.Id.etIpAddress);
            _Bindings.Add(this.SetBinding(() => SettingsViewModel.NewCentralUnitAddress, () => EtIpAddress.Text, BindingMode.TwoWay));

			// Fill spinner
			var items = Enum.GetValues(typeof(CentralUnitBrand)).OfType<object>().Select(s => s.ToString()).ToArray();
			var spinnerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);
			spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			dialogView.FindViewById<Spinner>(Resource.Id.spBrand).Adapter = spinnerAdapter;
			dialogView.FindViewById<Spinner>(Resource.Id.spBrand).Enabled = false;

			builder.Show();
		}
	}
}