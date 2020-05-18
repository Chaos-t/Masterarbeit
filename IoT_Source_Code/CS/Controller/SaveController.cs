using Foreground.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;


namespace Foreground.Controller
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
        public async static void WriteToJsonFile<T>(string filePath, T objectToWrite) where T : new()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(objectToWrite);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));
            await FileIO.WriteTextAsync(sampleFile, json);
            AppRestartFailureReason result = await CoreApplication.RequestRestartAsync("-fastInit");
        }

        public async static void WriteProfilArrayToJsonFile(string filePath, Profil[] profil)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(profil);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));
            await FileIO.WriteTextAsync(sampleFile, json);

        }

        public async static void WriteEventsArrayToJsonFile(string filePath, Veranstaltung[] events)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(events);
            json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(json));
            await FileIO.WriteTextAsync(sampleFile, json);

        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public async static Task<T> ReadFromJsonFile<T>(string filePath) where T : new()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync(filePath);
            string text = await FileIO.ReadTextAsync(file);

            string json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(text));
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async static Task<Profil[]> ReadArrayFromJsonFile (string filePath)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync(filePath);
            string text = await FileIO.ReadTextAsync(file);

            string json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(text));
            return JsonConvert.DeserializeObject<Profil[]>(json);
        }

        public async static Task<Veranstaltung[]> ReadVArrayFromJsonFile(string filePath)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync(filePath);
            string text = await FileIO.ReadTextAsync(file);

            string json = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(text));
            return JsonConvert.DeserializeObject<Veranstaltung[]>(json);
        }

        public async static Task<StorageFile> LoadFileFromAssets(string fileName)
        {
            string CountriesFile = @"Assets\"+ fileName;
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(CountriesFile);

            return file;
        }
    }
}
