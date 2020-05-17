using NamenschildDesktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamenschildDesktop.Services
{
    public static class Save
    {
        static string root = "Default";
        public static void SaveLogin(string ip, string username, string password)
        {
            string directory = ip + "\\Login.txt";

            if (!Directory.Exists(ip))
            {
                DirectoryInfo di = Directory.CreateDirectory(ip);
            }

            string[] text = { ip, username, password };
            File.WriteAllLines(directory, text);
        }

        public static string[] LoadLogin(string ip)
        {
            string directory = ip + "\\Login.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);

                return data;
            }
            return null;
        }

        public static void SaveIPList(string[] ip)
        {
            string directory = "IPList.txt";
            File.WriteAllLines(directory, ip);

        }

        public static string[] LoadIPList()
        {
            string directory = "IPList.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);


                return data;
            }
            return null;
        }

        public static void SaveDeviceInfo(string ip, HardwareInfo hf)
        {
            string directory = ip + "\\Profil.txt";
            string[] content = { hf.Name, hf.HarwareId, hf.Ip, hf.Network, hf.Ipv4.ToString(), hf.Ipv6.ToString(), hf.Version };

            File.WriteAllLines(directory, content);

        }

        public static HardwareInfo LoadDeviceInfo(string ip)
        {
            string directory = ip + "\\Profil.txt";
            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                HardwareInfo hf = new HardwareInfo
                {
                    Name = data[0],
                    HarwareId = data[1],
                    Ip = data[2],
                    Network = data[3],
                    Ipv4 = Convert.ToBoolean(data[4]),
                    Ipv6 = Convert.ToBoolean(data[5]),
                    Version = data[6]
                };

                return hf;
            }
            return null;
        }

        public static void SaveProfil(string ip, Profil[] profil)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Profil\\ProfilList.txt";
            else
                directory = root + "\\Profil\\ProfilList.txt";

            int i = 0;

            if (!Directory.Exists(root + "\\Profil"))
            {
                Directory.CreateDirectory(root + "\\Profil");
            }

            if (profil.Length > i+1)
            {
                if (profil[i].Image == null)
                    profil[i].Image = new byte[0];
                i++;
            }
            if (profil.Length > i+1)
            {
                if (profil[i].Image == null)
                    profil[i].Image = new byte[0];
                i++;
            }
            if (profil.Length > i+1)
            {
                if (profil[i].Image == null)
                    profil[i].Image = new byte[0];
                i++;
            }
            string[] json = { JsonConvert.SerializeObject(profil) };

            File.WriteAllLines(directory, json);
        }

        public static Profil[] LoadProfil(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Profil\\ProfilList.txt";
            else
                directory = root + "\\Profil\\ProfilList.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);

                Profil[] profilList = JsonConvert.DeserializeObject<Profil[]>(data[0]);

                return profilList;
            }
            return null;
        }

        public static void SaveProfilChecked(string ip, bool[] profilChecked)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Profil\\ProfilChecked.txt";
            else
                directory = root + "\\Profil\\ProfilChecked.txt";

            if (root.Equals("Default"))

                if (!Directory.Exists(root + "\\" + ip + "\\Profil"))
                {
                    Directory.CreateDirectory(root + "\\" + ip + "\\Profil");
                }
            else 

                if (!Directory.Exists(root + "\\Profil"))
                {
                    Directory.CreateDirectory(root + "\\Profil");
                }

            string[] json = { JsonConvert.SerializeObject(profilChecked) };

            File.WriteAllLines(directory, json);

        }

        public static bool[] LoadProfilChecked(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Profil\\ProfilChecked.txt";
            else
                directory = root + "\\Profil\\ProfilChecked.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);

                bool[] profilchecked = JsonConvert.DeserializeObject<bool[]>(data[0]);



                if (profilchecked == null)
                    profilchecked = new bool[]
                    {
                        true,
                        true,
                        true
                    };

                return profilchecked;
            }
            return new bool[]{
                true,
                true,
                true
            };
        }

        public static void SaveInfoLeiste(string ip, Infoleiste infoleiste)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Profil\\InfoLeiste.txt";
            else
                directory = root + "\\Profil\\InfoLeiste.txt";

            if (!Directory.Exists(root + "\\Profil"))
            {
                Directory.CreateDirectory(root + "\\Profil");
            }

            if (infoleiste.LeftImage == null)
                infoleiste.LeftImage = new byte[0];

            if (infoleiste.RightImage == null)
                infoleiste.RightImage = new byte[0];

            string[] json = { JsonConvert.SerializeObject(infoleiste) };

            File.WriteAllLines(directory, json);

        }

        public static void SetProfilSystem(string ip, string choosenValue)
        {
            root =  choosenValue;
        }

        public static void SaveProfilSystem(string ip, string[] root)
        {
            string directory = "Profil\\SaveProfil.txt";

            string[] json = root;

            if (!Directory.Exists("Profil"))
            {
                Directory.CreateDirectory("Profil");
            }

            File.WriteAllLines(directory, json);
        }

        public static string[] LoadProfilSystem(string ip)
        {
            string directory = "Profil\\SaveProfil.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);



                return data;
            }
            return null;
        }

        public static Infoleiste LoadInfoLeiste(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Profil\\InfoLeiste.txt";
            else
                directory = root + "\\Profil\\InfoLeiste.txt";

            if (!Directory.Exists("Profil"))
            {
                Directory.CreateDirectory("Profil");
            }
            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                Infoleiste infoleiste = JsonConvert.DeserializeObject<Infoleiste>(data[0]);



                return infoleiste;
            }
            return null;
        }

        public static void SaveAktuelles(string ip, Aktuelles aktuelles)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Aktuelles.txt";
            else
                directory = root + "\\Aktuelles.txt";

            if (!Directory.Exists(root + "\\" + ip))
            {
                Directory.CreateDirectory(root + "\\" + ip);
            }

            string[] json = { JsonConvert.SerializeObject(aktuelles) };

            File.WriteAllLines(directory, json);
        }

        public static Aktuelles LoadAktuelles(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Aktuelles.txt";
            else
                directory = root + "\\Aktuelles.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                Aktuelles aktuelles = JsonConvert.DeserializeObject<Aktuelles>(data[0]);



                return aktuelles;
            }

            return null;
        }

        public static void SaveVeranstaltung(string ip, Veranstaltung[] veranstaltung)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Veranstaltung.txt";
            else
                directory = root + "\\Veranstaltung.txt";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }    

            string[] json = { JsonConvert.SerializeObject(veranstaltung) };

            File.WriteAllLines(directory, json);
        }

        public static Veranstaltung[] LoadVeranstaltung(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Veranstaltung.txt";
            else
                directory = root + "\\Veranstaltung.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                Veranstaltung[] veranstaltung = JsonConvert.DeserializeObject<Veranstaltung[]>(data[0]);


                return veranstaltung;
            }

            return null;
        }

        public static void SaveCountVeranstaltung(string ip, int count)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\VeranstaltungCount.txt";
            else
                directory = root + "\\VeranstaltungCount.txt";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            string[] json = { JsonConvert.SerializeObject(count) };

            File.WriteAllLines(directory, json);
        }

        public static int LoadCountVeranstaltung(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\VeranstaltungCount.txt";
            else
                directory = root + "\\VeranstaltungCount.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                int count = JsonConvert.DeserializeObject<int>(data[0]);


                return count;
            }

            return 1;
        }


        public static void SaveOpenWeather(string ip, OpenWeather openWeather)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\OpenWeather.txt";
            else
                directory = root + "\\OpenWeather.txt";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            string[] json = { JsonConvert.SerializeObject(openWeather) };

            File.WriteAllLines(directory, json);
        }

        public static OpenWeather LoadOpenWeather(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\OpenWeather.txt";
            else
                directory = root + "\\OpenWeather.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                OpenWeather weather = JsonConvert.DeserializeObject<OpenWeather>(data[0]);


                return weather;
            }

            return null;
        }

        public static void SaveTabs(string ip, Tabs tabs)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Tabs.txt";
            else
                directory = root + "\\Tabs.txt";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            string[] json = { JsonConvert.SerializeObject(tabs) };

            File.WriteAllLines(directory, json);
        }

        public static Tabs LoadTabs(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Tabs.txt";
            else
                directory = root + "\\Tabs.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                Tabs tabs = JsonConvert.DeserializeObject<Tabs>(data[0]);


                return tabs;
            }

            return null;
        }

        public static void SaveSchedule(string ip, BelegungsPlan belegungsPlan)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Schedule.txt";
            else
                directory = root + "\\Schedule.txt";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            string[] json = { JsonConvert.SerializeObject(belegungsPlan) };

            File.WriteAllLines(directory, json);
        }

        public static BelegungsPlan LoadSchedule(string ip)
        {
            string directory = "";
            if (root.Equals("Default"))
                directory = root + "\\" + ip + "\\Schedule.txt";
            else
                directory = root + "\\Schedule.txt";

            if (File.Exists(directory))
            {
                string[] data = File.ReadAllLines(directory);
                BelegungsPlan belegungsPlan = JsonConvert.DeserializeObject<BelegungsPlan>(data[0]);


                return belegungsPlan;
            }

            return null;
        }


    }
}
