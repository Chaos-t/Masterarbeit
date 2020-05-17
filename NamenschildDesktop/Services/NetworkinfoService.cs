using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Net.NetworkInformation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace NamenschildDesktop.Services

{

    public static class NetworkinfoService
    {
        private static HttpClient http;

        public static void ChangeHttp(string user, string password)
        {
            http = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes(user+":"+password);
            var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            http.DefaultRequestHeaders.Authorization = header;
        }

        public static HttpClient GetHttp()
        {
            return http;
        }

        public static async Task<bool> Authentification(string ip)
        {
            try
            {
                var response = await http.GetAsync(String.Format("http://{0}:8080/api/iot/device/information", ip));
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> IsAppInstalled(string ip, int repeat = 0)
        {
            try
            {
                var response = await http.GetAsync(String.Format("http://{0}:8081/api/device/getActive", ip));
                if (response.Content.ReadAsStringAsync().Result == "true")
                {
                    return true;
                }
                return false;
            }
            catch
            {
                if (repeat < 4)
                {
                    Thread.Sleep(1000);
                    repeat++;
                    return await IsAppInstalled(ip, repeat);
                }
                else return false;
            }
        }

       

        public static void OpenAppBrowser(string ip)
        {
            //Browser öffnen zum Deployen der appx
            System.Diagnostics.Process.Start(String.Format("http://{0}:8080/#Apps%20manager", ip));
        }

        public async static Task RestartApp(string ip)
        {
            try
            {
                var response = await http.PostAsync(String.Format("http://{0}:8080/api/control/restart", ip), null);

            }
            catch { }
            
        }

        public async static Task<bool> ValidateIP(string ip)
        {
            try
            {



                // Ping's the IP.
                Ping pingSender = new Ping();
                IPAddress address = IPAddress.Parse(ip);

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                // Wait 10 seconds for a reply.
                int timeout = 5000;
                PingReply reply = await pingSender.SendPingAsync(address, timeout, buffer);

                if (reply.Status == IPStatus.Success)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                int idx = e.Message.LastIndexOf('.');
                System.Windows.Forms.MessageBox.Show(e.Message.Substring(0, idx) + ".", "Keine korrekte Eingabe!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static string Prettify(string toPrettify)
        {

            var charsToRemove = new string[] { "\\" };
            foreach (var c in charsToRemove)
            {
                toPrettify = toPrettify.Replace(c, string.Empty);
            }
            return toPrettify.Substring(1, toPrettify.Length - 2);
        }
    }

}