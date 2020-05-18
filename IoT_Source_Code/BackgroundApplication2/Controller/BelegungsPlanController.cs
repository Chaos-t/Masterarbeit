using BackgroundApplication2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication2.Controller
{
    public sealed class BelegungsPlanController
    {
        public static string SaveAssignment(BelegungsPlan belegungsPlan)
        {
            SaveController.WriteBelegungsPlanToJsonFile("BelegungsPlan.txt", belegungsPlan);
            MessageController.SendMessage("BelegungsPlan",JsonConvert.SerializeObject(belegungsPlan));
            return "Data saved";
        }

        public static string GetAssignment()
        {
            return JsonConvert.SerializeObject(SaveController.ReadFromJsonFileBelegungsPlan("BelegungsPlan.txt"));
        }
    }
}
