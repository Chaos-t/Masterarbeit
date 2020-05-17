using NamenschildDesktop.Model;
using NamenschildDesktop.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace NamenschildDesktop.Controller
{
    public static class ProfilController
    {
        static HttpClient http = null;

        private static void GetHttp()
        {
            http = NetworkinfoService.GetHttp();
        }

        public async static Task<Infoleiste> GetInfoLeiste(string ip)
        {
            try
            {
                if (http == null)
                    GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8081/api/infoleiste/getLeiste", ip));
                var result = await response.Content.ReadAsStringAsync();
                if (result != "")
                {
                    result = NetworkinfoService.Prettify(result);
                    Infoleiste infoleiste = JsonConvert.DeserializeObject<Infoleiste>(result);
                    Save.SaveInfoLeiste(ip, infoleiste);
                    return infoleiste;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool> SetInfoLeiste(string ip, Infoleiste infoleiste)
        {
            try
            {
                if (http == null)
                    GetHttp();



                var json = JsonConvert.SerializeObject(infoleiste);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(String.Format("http://{0}:8081/api/infoleiste/setLeiste", ip), httpContent);

                return response.IsSuccessStatusCode;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async static Task<Profil[]> GetProfil(string ip)
        {
            try
            {
                if (http == null)
                    GetHttp();

                var response = await http.GetAsync(String.Format("http://{0}:8081/api/allgemein/getProfil", ip));
                var result = await response.Content.ReadAsStringAsync();
                if (result != "")
                {
                    result = NetworkinfoService.Prettify(result);
                    var profil = JsonConvert.DeserializeObject<Profil[]>(result);
                    Save.SaveProfil(ip, profil);
                    return profil;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool> SetProfil(string ip, Profil[] profil)
        {
            try
            {
                if (http == null)
                    GetHttp();



                var json = JsonConvert.SerializeObject(profil);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(String.Format("http://{0}:8081/api/allgemein/setProfil", ip), httpContent);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static BitmapImage GetFile()
        {
            Stream imageFile = null;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Lade PNG Bild-Datei",
                Filter = "PNG files|*.png",
                InitialDirectory = @"C:\"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((imageFile = openFileDialog.OpenFile()) != null)
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = imageFile;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        image.Freeze();

                        return image;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return null;
                }
            }
            return null;
        }
    }
}
