using BackgroundApplication2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Windows.System.Threading;

namespace BackgroundApplication2.Controller
{
    public sealed class HardwareController
    {
        static Boolean ipv4 = false;
        static Boolean ipv6 = false;
        static String networkName = null;
        static IEnumerable<Process> appData = null;
        static HttpClient http = null;
        static EasClientDeviceInformation deviceInformation = new EasClientDeviceInformation();
        static string realname = "";
        static LoginData Login = null;

        public static void InitHttp()
        {
            if (http == null)
            {
                http = new HttpClient();
                Login = JsonConvert.DeserializeObject<LoginData>(GetLogin(3));
                var byteArray = Encoding.ASCII.GetBytes(Login.Username+":"+Login.Password);
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                http.DefaultRequestHeaders.Authorization = header;
            }
        }

        public static void GetAppData()
        {
            IAsyncAction asyncAction = ThreadPool.RunAsync(async (handler) =>
            {
                if (appData == null)
                {

                    InitHttp();
                    var all = await HelpGetConnect(http);

                    appData = all.Processes.Where(elem => elem.AppName == "Foreground");
                    GetRealname(http);
                }
            });
        }

        public static String Activate()
        {
            InitHttp();
            var answer = ActivateConnect(http).Result;

            return answer;
        }

        public static String ActivateExam(Boolean activate)
        {
            InitHttp();
            var answer = "";
            if (activate)
            {
                
                answer = ActivateExam(http).Result;
                
            }
            else
            {
                answer = DeactivateExam(http).Result;
            }
            return answer;
        }
        public static String Deactivate()
        {
            InitHttp();
            var answer = DeactivateConnect(http).Result;

            return answer;
        }

        private static async Task<RootObject> HelpGetConnect(HttpClient http)
        {
            try
            {
                var response = await http.GetAsync(String.Format("http://{0}:8080/api/resourcemanager/processes", GetCurrentIPAddress()));
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<RootObject>(result);

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static async void GetRealname(HttpClient http)
        {
            var response = await http.GetAsync(String.Format("http://{0}:8080/api/iot/device/information", GetCurrentIPAddress()));
            var result = await response.Content.ReadAsStringAsync();
            SecondRootObject data = JsonConvert.DeserializeObject<SecondRootObject>(result);

            realname = data.DeviceName;
        }

        private static async Task<String> ActivateConnect(HttpClient http)
        {
            var param = new Dictionary<string, string> {
                { "appid", Encode64("Foreground_dfdr0qb8gsad2!App")},
                { "package", Encode64("undefined") }
            };
       
            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.PostAsync(String.Format("http://{0}:8080/api/taskmanager/app?{1}", GetCurrentIPAddress(), urlVar), content);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        private static async Task<String> DeactivateConnect(HttpClient http)
        {
            var param = new Dictionary<string, string> {                
                { "package", Encode64("Foreground_1.0.0.0_arm__dfdr0qb8gsad2") }
            };

            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.DeleteAsync(String.Format("http://{0}:8080/api/taskmanager/app?{1}", GetCurrentIPAddress(), urlVar));
            var result = await response.Content.ReadAsStringAsync();


            return result;
        }

        private static async Task<String> ActivateExam(HttpClient http)
        {        
            var param = new Dictionary<string, string> {
                { "appid", Encode64("ForeGroundPrufung_dfdr0qb8gsad2!App")},
                { "package", Encode64("undefined") }
            };

            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.PostAsync(String.Format("http://{0}:8080/api/taskmanager/app?{1}", GetCurrentIPAddress(), urlVar), content);
            var result = await response.Content.ReadAsStringAsync();

            return result;

        }

        private static async Task<String> DeactivateExam(HttpClient http)
        {
            var param = new Dictionary<string, string> {
                { "package", Encode64("ForeGroundPrufung_1.0.0.0_arm__dfdr0qb8gsad2") }
            };

            var content = new FormUrlEncodedContent(param);
            var urlVar = await content.ReadAsStringAsync();
            var response = await http.DeleteAsync(String.Format("http://{0}:8080/api/taskmanager/app?{1}", GetCurrentIPAddress(), urlVar));
            var result = await response.Content.ReadAsStringAsync();


            return result;
        }
        public static string GetCurrentIPAddress()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp != null
                  && icp.NetworkAdapter != null
                  && icp.NetworkAdapter.NetworkAdapterId != null)
            {
                var name = icp.ProfileName;

                var hostnames = NetworkInformation.GetHostNames();

                foreach (var hn in hostnames)
                {
                    if (hn.IPInformation != null
                        && hn.IPInformation.NetworkAdapter != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                                                   != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                    == icp.NetworkAdapter.NetworkAdapterId
                        && hn.Type == HostNameType.Ipv4)
                    {
                        networkName = name;
                        ipv4 = true;
                        return hn.CanonicalName;
                    }
                

                    else if (hn.IPInformation != null
                        && hn.IPInformation.NetworkAdapter != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                                                   != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                    == icp.NetworkAdapter.NetworkAdapterId
                        && hn.Type == HostNameType.Ipv6)
                    {
                        networkName = name;
                        ipv6 = true;
                        return hn.CanonicalName;
                    }

                }
            }

            var hostnames2 = NetworkInformation.GetHostNames();
            foreach (var hn in hostnames2)
            {
                if (hn.IPInformation != null
                    && hn.IPInformation.NetworkAdapter != null
                    && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                                               != null                  
                    && hn.Type == HostNameType.Ipv4)
                {
                    networkName = "LAN";
                    ipv4 = true;                                   
                    return hn.CanonicalName;
                }
            }

            return null;
        }



        private static string Encode64(string strn)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(strn);
            string appName64 = System.Convert.ToBase64String(toEncodeAsBytes);
            appName64 = appName64.Replace(" ", "20%");
            return appName64;
        }

        public static String GetDeviceName()
        {
            if (realname.Equals(""))
            {
                InitHttp();
                GetRealname(http);
            }
            return realname;
        }

        public static String GetHardwareID()
        {
            return deviceInformation.Id.ToString();
        }

        public static String GetVersion()
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong major = (version & 0xFFFF000000000000L) >> 48;
            ulong minor = (version & 0x0000FFFF00000000L) >> 32;
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            ulong revision = (version & 0x000000000000FFFFL);
            var osVersion = $"{major}.{minor}.{build}.{revision}";

            return osVersion;
        }

        public static String GetNetworkName()
        {
            if (networkName == null)
            {
                GetCurrentIPAddress();
            }
            return networkName;
        }

        public static Boolean IsIPv4()
        {
            if (GetCurrentIPAddress() != null)
                return ipv4;
            return false;
        }

        public static Boolean IsIPv6()
        {
            if (GetCurrentIPAddress() != null)
                return ipv6;
            return false;
        }


        //Eigentliche Methoden

        public static Boolean GetDeviceInfo()
        {
            GetAppData();

            while (appData == null) ;


            if (appData.ElementAtOrDefault(0) != null)
                return (bool)appData.ElementAt(0).IsRunning;
            else
                return false;
        }

        public static string GetNetwork()
        {
            HardwareInfo Hf = new HardwareInfo
            {
                HarwareId = GetHardwareID(),
                Ip = GetCurrentIPAddress(),
                Ipv4 = IsIPv4(),
                Ipv6 = IsIPv6(),
                Name = GetDeviceName(),
                Network = GetNetworkName(),
                Version = GetVersion()
            };

            return JsonConvert.SerializeObject(Hf);
        }

        public static string SetActivate(bool active)
        {
            InitHttp();

            if (active)
                return Activate();
            else
                return Deactivate();
        }

        public static string GetLogin(int tries)
        {
            Login = SaveController.ReadFromJsonFileLogin("LoginData.txt");
            return JsonConvert.SerializeObject(Login);
        }

        public static string SetLogin(LoginData login)
        {
            Login = login;
            SaveController.WriteLoginToJsonFile("LoginData.txt",login);
            return "Data saved";
        }

    }
}
