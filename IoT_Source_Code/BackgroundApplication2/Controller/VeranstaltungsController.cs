using BackgroundApplication2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication2.Controller
{
    public sealed class VeranstaltungsController
    {
        public static string SaveText([ReadOnlyArray] Veranstaltung[] veranstaltung)
        {
            SaveController.WriteVeranstaltungToJsonFile("Veranstaltung.txt", veranstaltung);
            MessageController.SendMessage("Veranstaltung",JsonConvert.SerializeObject(veranstaltung));
            return "Data saved";
        }

        public static string GetText()
        {
            return JsonConvert.SerializeObject(SaveController.ReadFromJsonFileVeranstaltung("Veranstaltung.txt"));
        }
    }
}
