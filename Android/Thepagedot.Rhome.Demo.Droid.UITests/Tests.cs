using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace Thepagedot.Rhome.App.Droid.UITests
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = ConfigureApp.Android.StartApp();

			// Ensure demo mode is enabled
			EnableDemoMode();
        }

        [Test]
        public void ClickingButtonTwiceShouldChangeItsLabel()
        {
			app.Repl();
        }

		private void EnableDemoMode()
		{
			// Open settings menu
			app.Tap(x => x.Marked("Open drawer"));
			app.ScrollDown(x => x.Marked("Settings"));
			app.Tap(x => x.Marked("Settings"));
			app.WaitForElement(x => x.Id("swDemoMode"));

			// Enable demo mode if not already done
			var switchValue = app.Query(c => c.Id("swDemoMode").Invoke("isChecked").Value<bool>()).First();
			if (switchValue == false)
			{
				app.Tap(x => x.Id("swDemoMode"));
			}

			// Navigate back to main menu
			app.Tap(x => x.Marked("Navigate up"));
			app.WaitForElement(x => x.Id("tvStatus"));
		}
    }
}