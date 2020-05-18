using BackgroundApplication2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication2.Controller
{
    public sealed class LeistenController
    {
        public static string SaveProfil(Infoleiste info)
        {
            SaveController.WriteInfoLeisteToJsonFile("InfoProfil.txt", info);
            MessageController.SendMessage("InfoLeiste", JsonConvert.SerializeObject(info));
            return "Data saved";
        }

        public static string GetProfil()
        {
            return JsonConvert.SerializeObject(SaveController.ReadFromJsonFileInfoLeiste("InfoProfil.txt"));
        } 
    }
}
