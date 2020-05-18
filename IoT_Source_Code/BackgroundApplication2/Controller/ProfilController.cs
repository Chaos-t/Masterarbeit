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
    public sealed class ProfilController
    {

        public static string SaveProfil([ReadOnlyArray] Profil[] profil)
        {
            SaveController.WriteProfilToJsonFile("Profil.txt", profil);
            MessageController.SendMessage("Profil",JsonConvert.SerializeObject(profil));
            return "Data saved";
        }

        public static string GetProfil()
        {
            return JsonConvert.SerializeObject(SaveController.ReadFromJsonFileProfil("Profil.txt"));
        }
    }
}
