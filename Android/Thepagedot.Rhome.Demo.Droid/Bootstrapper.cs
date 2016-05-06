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
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.App.Shared.ViewModels;
using GalaSoft.MvvmLight.Views;
using Thepagedot.Rhome.App.Shared.Other;
using Thepagedot.Rhome.App.Droid;
using Thepagedot.Rhome.App.Droid.Services;
using JimBobBennett.MvvmLight.AppCompat;
using Thepagedot.Rhome.App.Droid.Views;

namespace Thepagedot.Rhome.App.Droid
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IResourceService, ResourceService>();
            SimpleIoc.Default.Register<ILocalStorageService, LocalStorageService>();
            SimpleIoc.Default.Register<IDialogService, AppCompatDialogService>();
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

        public HomeControlService HomeControlService { get { return SimpleIoc.Default.GetInstance<HomeControlService>(); } }
        public SettingsService SettingsService { get { return SimpleIoc.Default.GetInstance<SettingsService>(); } }

        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
        public RoomViewModel RoomViewModel { get { return SimpleIoc.Default.GetInstance<RoomViewModel>(); } }
        public SettingsViewModel SettingsViewModel { get { return SimpleIoc.Default.GetInstance<SettingsViewModel>(); } }
        public SystemVariableViewModel SystemVariableViewModel { get { return SimpleIoc.Default.GetInstance<SystemVariableViewModel>(); } }
        public ProgramViewModel ProgramViewModel { get { return SimpleIoc.Default.GetInstance<ProgramViewModel>(); } }
        public MessageViewModel MessageViewModel { get { return SimpleIoc.Default.GetInstance<MessageViewModel>(); } }

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