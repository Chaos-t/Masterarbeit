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
    public class TabController
    {
        static HttpClient http = null;

        private static void GetHttp()
        {
            http = NetworkinfoService.GetHttp();
        }

        public async static Task<Tabs> GetTabs(string ip)
        {
            try
            {
                GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8081/api/tabs/getActivated", ip));
                string result = await response.Content.ReadAsStringAsync();
                result = NetworkinfoService.Prettify(result);
                var data = JsonConvert.DeserializeObject<Tabs>(result);

                return data;
            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool> SetTabs(string ip, Tabs tabs)
        {
            try
            {
                if (http == null)
                    GetHttp();



                var json = JsonConvert.SerializeObject(tabs);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(String.Format("http://{0}:8081/api/tabs/setActivated", ip), httpContent);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
