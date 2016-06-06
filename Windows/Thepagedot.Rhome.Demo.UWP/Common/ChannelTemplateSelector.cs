using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Thepagedot.Rhome.App.UWP.Common
{
    public class ChannelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SwitcherTemplate { get; set; }
        public DataTemplate ContactTemplate { get; set; }
        public DataTemplate DoorHandleTemplate { get; set; }
        public DataTemplate ShutterTemplate { get; set; }
        public DataTemplate TemperatureSliderTemplate { get; set; }
        public DataTemplate InformationTemplate { get; set; }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is HomeMatic.Models.Switcher)
                return SwitcherTemplate;
            if (item is HomeMatic.Models.Contact)
                return ContactTemplate;
            if (item is HomeMatic.Models.DoorHandle)
                return DoorHandleTemplate;
            if (item is HomeMatic.Models.Shutter)
                return ShutterTemplate;
            if (item is HomeMatic.Models.TemperatureSlider)
                return TemperatureSliderTemplate;
            if (item is HomeMatic.Models.Information)
                return InformationTemplate;

            return base.SelectTemplateCore(item, container);
        }
    }
}
