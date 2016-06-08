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
        public static MergedRoom GetDemoRoom()
        {
            var room = new MergedRoom { Name = "Living room", Floor = 0 };
            var devices = new List<Device>
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

            var homeMaticRoom = new HomeMaticRoom("Living room", 0, null);
            homeMaticRoom.Devices = devices;

            return room;
        }

        public static ObservableCollection<MergedRoom> GetDemoRooms()
        {
            return new ObservableCollection<MergedRoom>
                {
                    new MergedRoom { Name = "Bedroom", Floor = 0 },
                    new MergedRoom { Name = "Living room", Floor = 0 },
                    new MergedRoom { Name = "Kitchen", Floor = 0 },
                };
        }

        public static ObservableCollection<SystemVariable> GetDemoSystemVariables()
        {
            var list = new ObservableCollection<SystemVariable>();
            list.Add(new HomeMaticSystemVariable(1, "Anwesenheit", "true", "", "", "", "", 2, 2, true, "", "nicht anwesend", "anwesend"));
            list.Add(new HomeMaticSystemVariable(2, "Alarmzone 1", "", "", "", "", "", 2, 2, true, "", "ausgelöst", "nicht ausgelöst"));
            list.Add(new HomeMaticSystemVariable(3, "Stringtest", "Lorem ipsum", "", "", "", "", 4, 11, true, "", "", ""));
            list.Add(new HomeMaticSystemVariable(3, "List Test", "1", "Var1;Var2;Var3", "", "", "", 16, 29, true, "", "", ""));
            return list;
        }
    }
}
