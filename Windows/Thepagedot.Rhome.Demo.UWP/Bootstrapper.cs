using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Thepagedot.Rhome.App.Shared.Other;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.App.Shared.ViewModels;
using Thepagedot.Rhome.App.UWP.Services;
using Thepagedot.Rhome.App.UWP.Views;
using Thepagedot.Tools;

namespace Thepagedot.Rhome.App.UWP
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IResourceService, ResourceService>();
            SimpleIoc.Default.Register<ILocalStorageService, LocalStorageService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<INavigationService>(() => CreateNavigationService());

            SimpleIoc.Default.Register<SettingsService>();
            SimpleIoc.Default.Register<HomeControlService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RoomViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<SystemVariableViewModel>();
            SimpleIoc.Default.Register<ProgramViewModel>();
            SimpleIoc.Default.Register<MessageViewModel>();
        }

        public HomeControlService HomeControlService { get { return SimpleIoc.Default.GetInstance<HomeControlService>(); }}
        public SettingsService SettingsService { get { return SimpleIoc.Default.GetInstance<SettingsService>(); }}

        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }}
        public RoomViewModel RoomViewModel { get { return SimpleIoc.Default.GetInstance<RoomViewModel>(); }}
        public SettingsViewModel SettingsViewModel { get { return SimpleIoc.Default.GetInstance<SettingsViewModel>(); }}
        public SystemVariableViewModel SystemVariableViewModel { get { return SimpleIoc.Default.GetInstance<SystemVariableViewModel>(); }}
        public ProgramViewModel ProgramViewModel { get { return SimpleIoc.Default.GetInstance<ProgramViewModel>(); }}
        public MessageViewModel MessageViewModel { get { return SimpleIoc.Default.GetInstance<MessageViewModel>(); } }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(ViewNames.Room, typeof(RoomPage));
            navigationService.Configure(ViewNames.Settings, typeof(SettingsPage));
            navigationService.Configure(ViewNames.SystemVariable, typeof(SystemVariablePage));
            navigationService.Configure(ViewNames.Program, typeof(ProgramPage));
            navigationService.Configure(ViewNames.Messages, typeof(MessagePage));
            navigationService.Configure(ViewNames.About, typeof(AboutPage));

            return navigationService;
        }
    }
}