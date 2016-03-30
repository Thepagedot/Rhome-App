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
    [Application]
    public class App : Application
    {
        public static Bootstrapper Bootstrapper = new Bootstrapper();

        public App(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override async void OnCreate()
        {
            base.OnCreate();
        }
    }
}