using NamenschildDesktop.Model;
using NamenschildDesktop.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NamenschildDesktop.Controller
{
    public class WetterController
    {
        static HttpClient http = null;

        private static void GetHttp()
        {
            http = NetworkinfoService.GetHttp();
        }

        public async static Task<OpenWeather> GetOpenWeather(string ip)
        {
            try
            {
                GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8081/api/wetter/getOpenWeather", ip));
                string result = await response.Content.ReadAsStringAsync();
                result = NetworkinfoService.Prettify(result);
                var data = JsonConvert.DeserializeObject<OpenWeather>(result);

                return data;
            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool> SetOpenWeather(string ip, OpenWeather openWeather)
        {
            try
            {
                if (http == null)
                    GetHttp();



                var json = JsonConvert.SerializeObject(openWeather);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(String.Format("http://{0}:8081/api/wetter/setOpenWeather", ip), httpContent);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
