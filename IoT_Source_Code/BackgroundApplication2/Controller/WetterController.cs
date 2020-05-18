using BackgroundApplication2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication2.Controller
{
    public sealed class WetterController
    {
        public static string SaveAssignment(OpenWeather weather)
        {
            SaveController.WriteOpenWeatherToJsonFile("OpenWeather.txt", weather);
            MessageController.SendMessage("Weather", JsonConvert.SerializeObject(weather));
            return "Data saved";
        }

        public static string GetAssignment()
        {
            return JsonConvert.SerializeObject(SaveController.ReadFromJsonFileWeather("OpenWeather.txt"));
        }



    }
}
