using System;
using Thepagedot.Rhome.App.Shared.Models;

namespace Thepagedot.Rhome.App.Shared
{
	public class Settings
	{
		public Configuration Configuration { get; set; }
		public bool IsDemoMode { get; set; }

		public Settings()
		{
			Configuration = new Configuration();
		}
	}
}