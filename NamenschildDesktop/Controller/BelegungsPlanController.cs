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
    public class BelegungsPlanController
    {
        static HttpClient http = null;

        


        private static void GetHttp()
        {
            http = NetworkinfoService.GetHttp();
        }

        public async static Task<BelegungsPlan> GetSchedule(string ip)
        {
            try
            {
                GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8081/api/belegungsplan/getBelegungsplan", ip));
                string result = await response.Content.ReadAsStringAsync();
                result = NetworkinfoService.Prettify(result);
                var data = JsonConvert.DeserializeObject<BelegungsPlan>(result);

                return data;
            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool> SetSchedule(string ip, BelegungsPlan belegungsPlan)
        {
            try
            {
                if (http == null)
                    GetHttp();



                var json = JsonConvert.SerializeObject(belegungsPlan);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(String.Format("http://{0}:8081/api/belegungsplan/setBelegungsplan", ip), httpContent);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
