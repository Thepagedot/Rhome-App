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

        public static ObservableCollection<Room> GetDemoRooms()
        {
            return new ObservableCollection<Room>
                {
                    new HomeMaticRoom("Bedroom", 0, new List<int>()),
                    new HomeMaticRoom("Living room", 0, new List<int>()),
                    new HomeMaticRoom("Kitchen", 0, new List<int>())
                };
        }

        public static ObservableCollection<SystemVariable> GetDemoSystemVariables()
        {
            var list = new ObservableCollection<SystemVariable>();
            list.Add(new SystemVariable(1, "Anwesenheit", "true", "", "", "", "", 2, 2, true, "", "nicht anwesend", "anwesend"));
            list.Add(new SystemVariable(2, "Alarmzone 1", "", "", "", "", "", 2, 2, true, "", "ausgelöst", "nicht ausgelöst"));
            list.Add(new SystemVariable(3, "Stringtest", "Lorem ipsum", "", "", "", "", 4, 11, true, "", "", ""));
            list.Add(new SystemVariable(3, "List Test", "1", "Var1;Var2;Var3", "", "", "", 16, 29, true, "", "", ""));
            return list;
        }
    }
}
