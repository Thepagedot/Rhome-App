using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.App.Shared.ViewModels;
using GalaSoft.MvvmLight.Views;
using Thepagedot.Rhome.App.Shared.Other;
using JimBobBennett.MvvmLight.AppCompat;
using Thepagedot.Rhome.App.Droid.Views;
using Thepagedot.Tools;
using Thepagedot.Tools.Xamarin.Android;

namespace Thepagedot.Rhome.App.Droid
{
	public class Bootstrapper
	{
		public Bootstrapper()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			// Register Interface implementations
			SimpleIoc.Default.Register<IResourceService, ResourceService>();
			SimpleIoc.Default.Register<ILocalStorageService, LocalStorageService>();
			SimpleIoc.Default.Register<IDialogService, AppCompatDialogService>();
			SimpleIoc.Default.Register<INavigationService>(() => CreateNavigationService());

			// Register platform independent services
			SimpleIoc.Default.Register<SettingsService>();
			SimpleIoc.Default.Register<HomeControlService>();

			// Register ViewModels
			SimpleIoc.Default.Register<MainViewModel>();
			SimpleIoc.Default.Register<RoomViewModel>();
			SimpleIoc.Default.Register<SettingsViewModel>();
			SimpleIoc.Default.Register<SystemVariableViewModel>();
			SimpleIoc.Default.Register<ProgramViewModel>();
			SimpleIoc.Default.Register<MessageViewModel>();
		}

		// Services
		public HomeControlService HomeControlService { get { return SimpleIoc.Default.GetInstance<HomeControlService>(); } }
		public SettingsService SettingsService { get { return SimpleIoc.Default.GetInstance<SettingsService>(); } }

		// ViewModels
		public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
		public RoomViewModel RoomViewModel { get { return SimpleIoc.Default.GetInstance<RoomViewModel>(); } }
		public SettingsViewModel SettingsViewModel { get { return SimpleIoc.Default.GetInstance<SettingsViewModel>(); } }
		public SystemVariableViewModel SystemVariableViewModel { get { return SimpleIoc.Default.GetInstance<SystemVariableViewModel>(); } }
		public ProgramViewModel ProgramViewModel { get { return SimpleIoc.Default.GetInstance<ProgramViewModel>(); } }
		public MessageViewModel MessageViewModel { get { return SimpleIoc.Default.GetInstance<MessageViewModel>(); } }

		/// <summary>
		/// Creates and configures a NavigationService.
		/// </summary>
		/// <returns>The navigation service.</returns>
		private INavigationService CreateNavigationService()
		{
			var navigationService = new AppCompatNavigationService();
			navigationService.Configure(ViewNames.Room, typeof(RoomActivity));
			navigationService.Configure(ViewNames.Messages, typeof(MessagesActivity));
			navigationService.Configure(ViewNames.Settings, typeof(SettingsActivity));
			navigationService.Configure(ViewNames.SystemVariable, typeof(SystemVariableActivity));
			navigationService.Configure(ViewNames.Program, typeof(ProgramActivity));
			navigationService.Configure(ViewNames.About, typeof(AboutActivity));
			return navigationService;
		}
	}
}