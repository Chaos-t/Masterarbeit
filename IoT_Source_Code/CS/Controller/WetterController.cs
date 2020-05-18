using Foreground.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreground.Controller
{
    public sealed class WetterController
    {
        public async static Task<OpenWeather> LoadOpenWeather()
        {

            try
            {
                OpenWeather weather = await SaveController.ReadFromJsonFile<OpenWeather>("OpenWeather.txt");

                return weather;
            }
            catch
            {

                OpenWeather weather = new OpenWeather
                {
                    API = "7a811b0575933b3d54da3594235aa4ec"
                };
                return weather;
            }
        }

        public static OpenWeather CreateModelFromJson(string json)
        {
            OpenWeather weather = JsonConvert.DeserializeObject<OpenWeather>(json);
            return weather;
        }
    }
}
