using BackgroundApplication2.Model;
using BackgroundApplication2.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BackgroundApplication2.Controller
{
    public sealed class AktuellesController
    {
        public static string SaveProfil(Aktuelles aktuelles)
        {
            SaveController.WriteAktuellesToJsonFile("Aktuelles.txt", aktuelles);
            MessageController.SendMessage("Aktuelles",JsonConvert.SerializeObject(aktuelles));
            return "Data saved";
        }

        public static string GetProfil()
        {
            return JsonConvert.SerializeObject(SaveController.ReadFromJsonFileAktuelles("Aktuelles.txt"));
        }



    }
}
