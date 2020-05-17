using System;
using System.Collections.Generic;
using System.Media;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NamenschildDesktop.Services;
using System.Windows.Media;
using Ookii.Dialogs.Wpf;
using BackgroundApplication2.Controller;
using NamenschildDesktop.Model;

namespace NamenschildDesktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Dictionary<string, ConnectWindow> AllWindows = new Dictionary<string, ConnectWindow>();

        public MainWindow()
        {
            InitializeComponent();
            RegisterHandler();
            LoadIPList();
        }


        private void RegisterHandler()
        {
            AddIP.Click += new RoutedEventHandler(AddIP_Click);
            DeleteIP.Click += new RoutedEventHandler(DeleteIP_Click);
            Connect.Click += new RoutedEventHandler(Connect_Click);
            Search.Click += new RoutedEventHandler(Search_Click);
            ReCheck.Click += new RoutedEventHandler(ReCheck_Click);
        }

        private async void ReCheck_Click(object sender, RoutedEventArgs e)
        {

            int state = Online.Items.Count;
            Online.Items.Clear();
            ConnectWindow[] windows = AllWindows.Values.Select(x => x).ToArray();

            for (int j = 0; j < state; j++)
            {
                ListViewItem listv = new ListViewItem
                {
                    Content = "Checking",
                    Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Yellow")),
                    Focusable = false
                };
                Online.Items.Add(listv);

                if (j < windows.Length)
                {
                    string[] subtext = windows[j].Title.Split('|');
                    windows[j].Title = subtext[0] + "| " + (string)listv.Content;
                }

            }

            int state2 = IPListe.Items.Count;
            for (int i = 0; i < state2; i++)
            {
                string save = IPListe.Items.GetItemAt(i).ToString();
                bool online = await NetworkinfoService.ValidateIP(save);
                if (IPListe.Items.Contains(save))
                {
                    OnlineAddItem(online, save, IPListe.Items.IndexOf(save));
                }
                else
                {
                    i--;
                }
                state2 = IPListe.Items.Count;
            }

        }

        private async void AddIP_Click(Object sender, EventArgs e)
        {
            string ip = IPBox.Address;

            try
            {


                ListViewItem listv = new ListViewItem
                {
                    Content = "Checking",
                    Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Yellow")),
                    Focusable = false
                };
                Online.Items.Add(listv);
                int state = Online.Items.Count - 1;



                if (!IPListe.Items.Contains(ip))
                {
                    IPListe.Items.Add(ip);
                    SaveIPList();
                    bool online = await NetworkinfoService.ValidateIP(ip);

                    if (IPListe.Items.Contains(ip))
                    {
                        OnlineAddItem(online, ip, IPListe.Items.IndexOf(ip));

                    }

                }
                else
                {
                    Online.Items.RemoveAt(Online.Items.Count - 1);
                }
            }
            catch 
            {
                IPListe.Items.Remove(ip);
                Online.Items.RemoveAt(Online.Items.Count - 1);
            }
            finally
            {

            }
        }

        private void DeleteIP_Click(Object sender, EventArgs e)
        {

            int index = GetSelectedIndex();
            if (index > -1)
            {
                string ip = IPListe.Items.GetItemAt(index) as string;

                // Configure the message box to be displayed
                string messageBoxText = "Wenn Sie die IP \"" + ip + "\" löschen, werden alle gespeicherten Daten zur Verbindung ebenfalls gelöscht. Trotzdem Fortfahren?";
                string caption = "IP \"" + ip + "\" wirklich löschen.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;

                // Display message box
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (AllWindows.ContainsKey(ip))
                        {
                            ConnectWindow window = AllWindows[ip];
                            window.Close();
                        }

                        IPListe.Items.RemoveAt(index);
                        Online.Items.RemoveAt(index);
                        SaveIPList();
                        break;
                    case MessageBoxResult.No:
                        // User pressed No button
                        // ...
                        break;

                }


            }



        }

        private void Connect_Click(Object sender, EventArgs e)
        {


            if (IPListe.SelectedItem != null && !AllWindows.ContainsKey(IPListe.SelectedItem.ToString()))
            {
                var index = GetSelectedIndex();
                var status = (ListViewItem)Online.Items.GetItemAt(index);

                Login(IPListe.SelectedItem.ToString());


                ConnectWindow window = new ConnectWindow(IPListe.SelectedItem.ToString())
                {

                    Title = IPListe.SelectedItem.ToString() + " | " + status.Content,


                };
                window.Closed += new EventHandler(Window_Closed);
                window.Show();
                AllWindows.Add(IPListe.SelectedItem.ToString(), window);
                window.ActivateWindow();

            }

            else if (IPListe.SelectedItem != null && AllWindows.ContainsKey(IPListe.SelectedItem.ToString()))
            {
                ConnectWindow window = AllWindows[IPListe.SelectedItem.ToString()];

                window.Focus();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = sender as ConnectWindow;
            string[] subtext = window.Title.Split('|');


            AllWindows.Remove(subtext[0].Remove(subtext[0].Length - 1));
        }

        private void Search_Click(Object sender, EventArgs e)
        {

        }

        ////////////////////////////////////////////////////////////////

        


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void OnlineAddItem(bool online, string ip, int state)
        {
            ListViewItem listv;

            if (online)
            {
                listv = new ListViewItem
                {
                    Content = "Online",
                    Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Green")),
                    Focusable = false
                };
                Online.Items.RemoveAt(state);
                Online.Items.Insert(state, listv);

            }

            else
            {
                listv = new ListViewItem
                {
                    Content = "Offline",
                    Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Red")),
                    Focusable = false
                };


                Online.Items.RemoveAt(state);
                Online.Items.Insert(state, listv);
            }

            if (AllWindows.ContainsKey(ip))
            {
                ConnectWindow window = AllWindows[ip];
                window.Title = ip + " | " + listv.Content;
            }

        }


        private int GetSelectedIndex()
        {
            var selected = IPListe.SelectedItem;
            var items = IPListe.Items;

            for (int i = 0; i < IPListe.Items.Count; i++)
            {
                if (items[i].Equals(selected))
                {
                    return i;
                }

            }
            return -1;
        }



        private async void Login(string ip)
        {

            string strUsername = "";
            string strPassword = "";


            CredentialDialog crDiag = new CredentialDialog
            {
                Content = "Diese Information stammen vom Windows 10 IoT Gerät und werden für die Kommunikation mit der API benötigt.",
                MainInstruction = "Bitte geben Sie Benutzername und Passwort für " + IPListe.SelectedItem.ToString() + " ein.",
                Target = "AnmeldeFenster",

                ShowSaveCheckBox = true
            };

            string[] saveddata = Save.LoadLogin(ip);

            if (saveddata == null)
            {
                if (crDiag.ShowDialog() == true)
                {

                    strUsername = crDiag.UserName;
                    strPassword = crDiag.Password;

                    NetworkinfoService.ChangeHttp(strUsername, strPassword);
                    bool valid = await NetworkinfoService.Authentification(ip);

                    if (valid && crDiag.IsSaveChecked)
                    {                  
                        Save.SaveLogin(ip, strUsername, strPassword);                   
                    }
                    else if (!valid)
                    {
                        System.Windows.Forms.MessageBox.Show("Benutzername und Passwort sind entweder nicht korrekt oder das Gerät ist offline. \n\nWenn Sie Daten senden oder empfangen möchten, werden Sie wiederholt nach den Zugangsdaten gefragt.", "Benutzername und Passwort inkorrekt", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ohne Benutzername und Passwort ist es Ihnen nicht möglich Daten an das Gerät zu senden oder zu empfangen. \n\nWenn Sie Daten senden oder empfangen möchten werden Sie wiederholt nach den Zugangsdaten gefragt.", "Benutzername und Passwort inkorrekt", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            else
            {
                NetworkinfoService.ChangeHttp(saveddata[1], saveddata[2]);
            }


        }

        public static string GetIPByValue(ConnectWindow window)
        {
            string key = AllWindows.FirstOrDefault(x => x.Value == window).Key;
            return key;
        }

        public void SaveIPList()
        {
            List<string> IPList = new List<string>();
            for (int i = 0; i < IPListe.Items.Count; i++)
            {
                IPList.Add(IPListe.Items.GetItemAt(i).ToString());
            }
            Save.SaveIPList(IPList.ToArray<string>());
        }

        public void LoadIPList()
        {
            string[] IPList = Save.LoadIPList();

            if (IPList != null)
            {
                for (int i = 0; i < IPList.Length; i++)
                {
                    IPListe.Items.Add(IPList[i]);
                    ListViewItem listv = new ListViewItem
                    {
                        Content = "Checking",
                        Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Yellow")),
                        Focusable = false
                    };
                    Online.Items.Add(listv);
                }
                ReCheck_Click(null, null);
            }
        }
    }
}
