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
    public class VeranstaltungsController
    {
        static HttpClient http = null;


        private static void GetHttp()
        {
            http = NetworkinfoService.GetHttp();
        }


        public async static Task<Veranstaltung[]> GetVeranstaltung(string ip)
        {
            try
            {
                if (http == null)
                    GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8081/api/veranstaltung/getAktuell", ip));
                var result = await response.Content.ReadAsStringAsync();
                if (result != "")
                {
                    result = NetworkinfoService.Prettify(result);
                    Veranstaltung[] events = JsonConvert.DeserializeObject<Veranstaltung[]>(result);
                    Save.SaveVeranstaltung(ip, events);
                    return events;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool> SetVeranstaltung(string ip, Veranstaltung[] veranstaltung)
        {
            try
            {
                if (http == null)
                    GetHttp();



                var json = JsonConvert.SerializeObject(veranstaltung);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(String.Format("http://{0}:8081/api/veranstaltung/setAktuell", ip), httpContent);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
