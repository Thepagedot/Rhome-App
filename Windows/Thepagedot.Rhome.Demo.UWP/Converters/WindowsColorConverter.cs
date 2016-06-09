using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using System.Reflection;

namespace Thepagedot.Rhome.App.UWP.Converters
{
    public class WindowsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var a = (byte)value.GetType().GetProperty("A").GetValue(value);
                var r = (byte)value.GetType().GetProperty("R").GetValue(value);
                var g = (byte)value.GetType().GetProperty("G").GetValue(value);
                var b = (byte)value.GetType().GetProperty("B").GetValue(value);
                return Color.FromArgb(a, r, g, b);
            }
            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
