using NamenschildDesktop;
using NamenschildDesktop.Model;
using NamenschildDesktop.Services;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;

namespace BackgroundApplication2.Controller
{
    public static class HardwareController
    {
        static HttpClient http = null;

        private static void GetHttp()
        {
            http = NetworkinfoService.GetHttp();
        }

        public async static Task<HardwareInfo> GetData(string ip)
        {
            try
            {
                GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8081/api/device/getNetwork", ip));
                string result = await response.Content.ReadAsStringAsync();
                result = NetworkinfoService.Prettify(result);
                var data = JsonConvert.DeserializeObject<HardwareInfo>(result);

                return data;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        

        public async static Task<bool> SetDeviceName(string ip, string name)
        {
            try
            {
                if (http == null)
                    GetHttp();

                var param = new Dictionary<string, string> {
                { "newdevicename", Base64Encode(name)}
            };

                var content = new FormUrlEncodedContent(param);
                var urlVar = await content.ReadAsStringAsync();

                var response = await http.PostAsync(String.Format("http://{0}:8080/api/iot/device/name?{1}", ip, urlVar), content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async static Task<bool> SetPassword(string ip, string OldPassword, string NewPassword)
        {
            try
            {
                LoginData login = new LoginData{ Username = "Administrator", Password = NewPassword };
                await SetLoginData(ip, login);

                if (http == null)
                    GetHttp();

                var param = new Dictionary<string, string> {
                { "oldpassword", Base64Encode(OldPassword)},
                { "newpassword", Base64Encode(NewPassword)}
            };

                var content = new FormUrlEncodedContent(param);
                var urlVar = await content.ReadAsStringAsync();

                var response = await http.PostAsync(String.Format("http://{0}:8080/api/iot/device/password?{1}", ip, urlVar), content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async static Task UninstallAPP(string ip)
        {
            await Progressbar_1.UninstallApp(ip);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public async static Task InstallApp(string ip)
        {
            await Progressbar_1.InstallApp(ip);
        }



        public async static Task<bool> UploadFinished(string ip)
        {
            try
            {
                if (http == null)
                    GetHttp();


                var response = await http.GetAsync(String.Format("http://{0}:8080/api/app/packagemanager/state", ip));


                if (response.StatusCode == HttpStatusCode.NoContent)
                {

                    Thread.Sleep(new TimeSpan(0, 0, 10));
                    return await UploadFinished(ip);

                }

                var content = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async static Task StartPruefung(string ip, bool activate)
        {
            if (http == null)
                GetHttp();

            Zustand zustand = new Zustand
            {
                Active = activate
            };

            var json = JsonConvert.SerializeObject(zustand);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync(String.Format("http://{0}:8081/api/pruefung/activate", ip), httpContent);
        }

        public async static Task<bool> UploadApp(string ip, string path, string fileName)
        {

            var param = new Dictionary<string, string> {
                { "package", fileName}

            };

            var urlparam = new FormUrlEncodedContent(param);
            var urlVar = await urlparam.ReadAsStringAsync();

            string[] load = Save.LoadLogin(ip);




            var client = new RestClient(String.Format("http://{0}:8080/api/app/packagemanager/package?{1}", ip, urlVar))
            {
                Authenticator = new HttpBasicAuthenticator(load[1], load[2])
            };

            var request = new RestRequest(Method.POST);
            // add files to upload (works with compatible verbs)

            FileStream stream = File.Open(path, FileMode.Open);
            request.Files.Add(new FileParameter
            {
                Name = "fileAttach",
                Writer = (s) =>
                {
                    stream.CopyTo(s);
                    stream.Dispose();
                },
                FileName = fileName,
                ContentLength = stream.Length
            });

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            return response.IsSuccessful;
        }

        public static byte[] StreamToByteArray(Stream input)
        {
            using (var memoryStream = new MemoryStream())
            {
                input.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }



        public static async Task DeleteAPP(string ip, string name)
        {
            if (http == null)
                GetHttp();

            var param = new Dictionary<string, string> {
                { "package", name }
            };

            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.DeleteAsync(String.Format("http://{0}:8080/api/app/packagemanager/package?{1}", ip, urlVar));
        }

        public static async Task SetDefault(string ip, string name)
        {

            if (http == null)
                GetHttp();

            var param = new Dictionary<string, string> {
                { "appid", Base64Encode(name) }
            };

            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.PostAsync(String.Format("http://{0}:8080/api/iot/appx/default?{1}", ip, urlVar), content);


        }

        public static async Task StartupHeadlessApp(string ip, string name)
        {

            if (http == null)
                GetHttp();

            var param = new Dictionary<string, string> {
                { "appid", Base64Encode(name) }
            };

            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.PostAsync(String.Format("http://{0}:8080/api/iot/appx/startupHeadlessApp?{1}", ip, urlVar), content);


        }

        public static async Task StopHeadlessApp(string ip, string name)
        {

            if (http == null)
                GetHttp();

            var param = new Dictionary<string, string> {
                { "appid", Base64Encode(name) }
            };

            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.DeleteAsync(String.Format("http://{0}:8080/api/iot/appx/startupHeadlessApp?{1}", ip, urlVar));


        }

        public static async Task<BitmapImage> GetScreenShot(string ip)
        {
            try
            {
                if (http == null)
                    GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8080/api/iot/screenshot", ip));

                StreamContent img = (StreamContent)response.Content;
                byte[] byteArrayIn = await img.ReadAsByteArrayAsync();

                var ms = new MemoryStream(byteArrayIn);
                var bitmap = new Bitmap(Image.FromStream(ms));

                BitmapImage bmImg = new BitmapImage();

                using (MemoryStream memStream2 = new MemoryStream())
                {
                    bitmap.Save(memStream2, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;

                    bmImg.BeginInit();
                    bmImg.CacheOption = BitmapCacheOption.OnLoad;
                    bmImg.UriSource = null;
                    bmImg.StreamSource = memStream2;
                    bmImg.EndInit();
                }
                return bmImg;

            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool> SetLoginData(string ip, LoginData loginData)
        {
            try
            {
                if (http == null)
                    GetHttp();

                var json = JsonConvert.SerializeObject(loginData);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(String.Format("http://{0}:8081/api/device/setLogin", ip), httpContent);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
