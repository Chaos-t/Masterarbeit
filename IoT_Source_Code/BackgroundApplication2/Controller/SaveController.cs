using BackgroundApplication2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BackgroundApplication2.Controller
{
    public sealed class SaveController
    {
        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>

        public static async void WriteTabsToJsonFile(string filePath, Tabs objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);


        }

        public static async void WriteInfoLeisteToJsonFile(string filePath, Infoleiste objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);



        }

        public static async void WriteProfilToJsonFile(string filePath, [ReadOnlyArray] Profil[] objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);

        }

        public async static void WriteAktuellesToJsonFile(string filePath, Aktuelles objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);

        }

        public async static void WriteVeranstaltungToJsonFile(string filePath, [ReadOnlyArray] Veranstaltung[] objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);

        }

        public async static void WriteBelegungsPlanToJsonFile(string filePath, BelegungsPlan objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);

        }

        public async static void WriteOpenWeatherToJsonFile(string filePath, OpenWeather objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);

        }

        public async static void WriteLoginToJsonFile(string filePath, LoginData objectToWrite)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filePath,
                    CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));

            await FileIO.WriteTextAsync(file, json);

        }
        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static Tabs ReadFromJsonFileTab(string filePath)
        {
            try
            {
                return (Tabs)JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(Tabs));
            }
            catch
            {
                return null;
            }
        }

        public static Infoleiste ReadFromJsonFileInfoLeiste(string filePath)
        {
            try
            {
                return (Infoleiste)JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(Infoleiste));
            }
            catch
            {
                return null;
            }
        }

        public static Profil[] ReadFromJsonFileProfil(string filePath)
        {
            try
            {
                return (Profil[])JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(Profil[]));
            }
            catch
            {
                return null;
            }
        }

        public static Aktuelles ReadFromJsonFileAktuelles(string filePath)
        {
            try
            {
                return (Aktuelles)JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(Aktuelles));
            }
            catch
            {
                return null;
            }
        }

        public static Veranstaltung[] ReadFromJsonFileVeranstaltung(string filePath)
        {
            try
            {
                return (Veranstaltung[])JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(Veranstaltung[]));
            }
            catch
            {
                return null;
            }
        }

        public static BelegungsPlan ReadFromJsonFileBelegungsPlan(string filePath)
        {
            try
            {
                return (BelegungsPlan)JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(BelegungsPlan));
            }
            catch
            {
                return null;
            }
        }

        public static OpenWeather ReadFromJsonFileWeather(string filePath)
        {
            try
            {
                return (OpenWeather)JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(OpenWeather));
            }
            catch
            {
                return null;
            }
        }

        public static LoginData ReadFromJsonFileLogin(string filePath)
        {
            try
            {
                return (LoginData)JsonConvert.DeserializeObject(HelperRead(filePath).Result, typeof(LoginData));
            }
            catch
            {
                return null;
            }
        }

        private static async Task<string> HelperRead(string filePath)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync(filePath);

            string json = await FileIO.ReadTextAsync(file);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));
            return json;
        }


    }
}
