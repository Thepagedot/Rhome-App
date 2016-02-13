using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thepagedot.Rhome.Base.Models;
using Thepagedot.Rhome.HomeMatic.Models;

namespace Thepagedot.Rhome.App.Shared.Other
{
    public static class DesignData
    {
        public static Room GetDemoRoom()
        {
            var room = new HomeMaticRoom("Living room", 0, new List<int>());
            room.Devices = new List<Device>
                {
                    new HomeMaticDevice("Testdevice 1", 0, "")
                    {
                        Channels = new List<HomeMaticChannel>
                        {
                            new Switcher("Testswitcher", 1, 1, "", true, null)
                        }
                    },

                    new HomeMaticDevice("Testdevice 2", 0, "")
                    {
                        Channels = new List<HomeMaticChannel>
                        {
                            new Shutter("Testshutter", 1, 1, "", true, null)
                        }
                    },

                    new HomeMaticDevice("Testdevice 3", 0, "")
                    {
                        Channels = new List<HomeMaticChannel>
                        {
                            new TemperatureSlider("Testslider", 1, 1, "", true, null)
                        }
                    }
                };

            return room;
        }
    }
}
