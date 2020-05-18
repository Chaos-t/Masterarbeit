using BackgroundApplication2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BackgroundApplication2.Controller
{
    public sealed class TabsController
    {
        public static String SaveTab(Tabs tabs)
        {

            SaveController.WriteTabsToJsonFile("Tabs.txt", tabs);
            MessageController.SendMessage("Tabs",JsonConvert.SerializeObject(tabs));
            return "Data saved";
        }

        public static string GetTab()
        {
            return JsonConvert.SerializeObject(SaveController.ReadFromJsonFileTab("Tabs.txt")); 
        }
    }
}
