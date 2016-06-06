using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.HomeMatic.Models;
using Windows.UI.Xaml.Data;

namespace Thepagedot.Rhome.App.UWP.Converters
{
    public sealed class HideEmptyChannelsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<HomeMaticChannel>)
            {
                return ((IEnumerable<HomeMaticChannel>)value).Where(c => c.IsVisible);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
