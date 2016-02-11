using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.App.Shared.Services;
using Thepagedot.Rhome.App.Shared.ViewModels;
using Thepagedot.Rhome.App.UWP.Services;

namespace Thepagedot.Rhome.App.UWP
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IResourceService, ResourceService>();
            SimpleIoc.Default.Register<ILocalStorageService, LocalStorageService>();

            SimpleIoc.Default.Register<SettingsService>();
            SimpleIoc.Default.Register<HomeControlService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RoomViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<SystemVariableViewModel>();
        }

        public HomeControlService HomeControlService { get { return SimpleIoc.Default.GetInstance<HomeControlService>(); }}
        public SettingsService SettingsService { get { return SimpleIoc.Default.GetInstance<SettingsService>(); }}

        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }}
        public RoomViewModel RoomViewModel { get { return SimpleIoc.Default.GetInstance<RoomViewModel>(); }}
        public SettingsViewModel SettingsViewModel { get { return SimpleIoc.Default.GetInstance<SettingsViewModel>(); }}
        public SystemVariableViewModel SystemVariableViewModel { get { return SimpleIoc.Default.GetInstance<SystemVariableViewModel>(); }}
    }
}