using BackgroundApplication2.Controller;
using NamenschildDesktop.Controller;
using NamenschildDesktop.Model;
using NamenschildDesktop.Services;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;

namespace NamenschildDesktop
{
    /// <summary>
    /// Interaktionslogik für ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        string ip = "";
        bool pruefung = false;
        bool once = true;
        bool profilchanged = false;

        static string[] helper = { "", "", "", "", "", "", "", "", "", "", "", "" };
        List<Border> helpBorders = new List<Border>();
        List<TextBlock> helpTextBlocks = new List<TextBlock>();
        List<Border> allBorders = new List<Border>();
        List<TextBlock> allTextBlocks = new List<TextBlock>();
        static List<int> valuedItems = new List<int>();
        static int[] helpEventDifference = { 0, 0, 0, 0 };

        List<int>[] ValuedItemsArray = {
            Clone(valuedItems),
            Clone(valuedItems),
            Clone(valuedItems),
            Clone(valuedItems),
            Clone(valuedItems),
        };

        private static BelegungsPlan MainBelegungsPlan = new BelegungsPlan
        {
            Montag = (String[])helper.Clone(),
            Dienstag = (String[])helper.Clone(),
            Mittwoch = (String[])helper.Clone(),
            Donnerstag = (String[])helper.Clone(),
            Freitag = (String[])helper.Clone()
        };

        public ConnectWindow(string ip)
        {
            InitializeComponent();


            Loaded += new RoutedEventHandler(Window_Loaded);

            this.ip = ip;

            Save.SetProfilSystem(ip, "Default");
            Load_DeviceInfo();
            Load_ProfilSystem();
            LoadProfil_Changed(null, null);


        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            Refresh.Click += new RoutedEventHandler(Refresh_Clicked);
            Install.Click += new RoutedEventHandler(Install_Clicked);
            Uninstall.Click += new RoutedEventHandler(Uninstall_Clicked);
            Restart.Click += new RoutedEventHandler(Restart_Clicked);
            SaveButton.Click += new RoutedEventHandler(Save_Clicked);
            Screen.Click += new RoutedEventHandler(Screen_Clicked);
            Login.Click += new RoutedEventHandler(Login_Clicked);
            Pruefung.Click += new RoutedEventHandler(Prufung_Clicked);
            LeftLogoButton.Click += new RoutedEventHandler(LeftLogoButton_Clicked);
            RightLogoButton.Click += new RoutedEventHandler(RightLogoButton_Clicked);

            ProfilImage1Button.Click += new RoutedEventHandler(ProfilImage1_Clicked);
            ProfilImage2Button.Click += new RoutedEventHandler(ProfilImage2_Clicked);
            ProfilImage3Button.Click += new RoutedEventHandler(ProfilImage3_Clicked);

            CheckProfil1.Checked += new RoutedEventHandler(CheckProfil1_Activate);
            CheckProfil1.Unchecked += new RoutedEventHandler(CheckProfil1_Deactivate);
            CheckProfil2.Checked += new RoutedEventHandler(CheckProfil2_Activate);
            CheckProfil2.Unchecked += new RoutedEventHandler(CheckProfil2_Deactivate);
            CheckProfil3.Checked += new RoutedEventHandler(CheckProfil3_Activate);
            CheckProfil3.Unchecked += new RoutedEventHandler(CheckProfil3_Deactivate);

            EventFrom1.SelectionChanged += new SelectionChangedEventHandler(EventFrom1_Changed);
            EventFrom2.SelectionChanged += new SelectionChangedEventHandler(EventFrom2_Changed);
            EventFrom3.SelectionChanged += new SelectionChangedEventHandler(EventFrom3_Changed);
            EventFrom4.SelectionChanged += new SelectionChangedEventHandler(EventFrom4_Changed);

            NumberOfEvents.SelectionChanged += new SelectionChangedEventHandler(NumberOfEvents_Changed);

            ProfilTab.Checked += new RoutedEventHandler(ProfilTab_Activate);
            ProfilTab.Unchecked += new RoutedEventHandler(ProfilTab_Deactivate);
            ActualTab.Checked += new RoutedEventHandler(ActualTab_Activate);
            ActualTab.Unchecked += new RoutedEventHandler(ActualTab_Deactivate);
            EventTab.Checked += new RoutedEventHandler(EventTab_Activate);
            EventTab.Unchecked += new RoutedEventHandler(EventTab_Deactivate);
            ScheduleTab.Checked += new RoutedEventHandler(ScheduleTab_Activate);
            ScheduleTab.Unchecked += new RoutedEventHandler(ScheduleTab_Deactivate);
            WeatherTab.Checked += new RoutedEventHandler(WeatherTab_Activate);
            WeatherTab.Unchecked += new RoutedEventHandler(WeatherTab_Deactivate);

            ScheduleAddButton.Click += new RoutedEventHandler(ScheduleAddButton_Clicked);
            ScheduleDeleteButton.Click += new RoutedEventHandler(ScheduleDeleteButton_Clicked);

            ScheduleFrom.SelectionChanged += new SelectionChangedEventHandler(ScheduleFrom_Changed);
            ScheduleDay.SelectionChanged += new SelectionChangedEventHandler(ScheduleDay_Changed);

            ProfilHeadTab.LayoutUpdated += new EventHandler(Profil_Loaded);

            LoadProfil.SelectionChanged += new SelectionChangedEventHandler(LoadProfil_Changed);
        }

        private void LoadProfil_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem choosenValueC = (ComboBoxItem)LoadProfil.SelectedValue;
            string choosenValue = choosenValueC.Content.ToString();

            if (!choosenValue.Equals("Neues Profil"))
            {
                Save.SetProfilSystem(ip, choosenValue);

                SetProfilTab();
                Load_Aktuelles();
                Load_Veranstaltung();
                Load_OpenWeather();
                Load_Tabs();
                Load_Schedule();

                
            }
            else
            {
                string profil = "Neues Profil";
                InputDialog.ShowInputDialog(ref profil);

                if (!profil.Equals("Default") && !profil.Equals("Neues Profil"))
                {
                    NewProfil_Adder(profil);
                    Save_ProfilSystem();
                }
            }

            profilchanged = true;
        }



        private void Profil_Loaded(object sender, EventArgs e)
        {

            if (once)
            {
                once = false;
                bool check = ProfilTab.IsChecked ?? false;
                if (check)
                    ProfilTab_Activate(null, null);
                else
                    ProfilTab_Deactivate(null, null);
            }
        }

        private async Task<bool> IsAppinstalled()
        {
            return await NetworkinfoService.IsAppInstalled(ip);
        }

        public async void ActivateWindow()
        {
            appInstalled.Text = "Checking";

            if (await IsAppinstalled())
            {
                appInstalled.Text = "App ist installiert";
            }
            else
            {
                appInstalled.Text = "App ist nicht installiert";
            }
        }

        private void SetGeratTab(HardwareInfo hi)
        {
            HardwareName.Text = hi.Name;
            Network.Text = hi.Network;
            IP.Text = hi.Ip;
            OS.Text = hi.Version;
            Ipv4.Text = hi.Ipv4.ToString();
            Ipv6.Text = hi.Ipv6.ToString();

            GetLogin();
        }

        public async void Refresh_Clicked(object sender, EventArgs e)
        {
            HardwareInfo hi = await HardwareController.GetData(ip);

            if (hi != null)
            {
                ActivateWindow();
                SetGeratTab(hi);

                Save_DeviceInfo(hi);
            }

            Infoleiste il = await ProfilController.GetInfoLeiste(ip);

            if (il != null)
            {
                SetInfoLeiste(il);
                Save_InfoLeiste(il);
            }

            Profil[] profils = await ProfilController.GetProfil(ip);

            if (profils != null)
            {
                SetProfil(profils);
                Save_Profil(profils);
            }

            Aktuelles aktuelles = await AktuellesController.GetAktuelles(ip);

            if (aktuelles != null)
            {
                SetAktuelles(aktuelles);
                Save_Aktuelles(aktuelles);
            }

            Veranstaltung[] veranstaltung = await VeranstaltungsController.GetVeranstaltung(ip);


            if (veranstaltung != null)
            {
                SetVeranstaltung(veranstaltung, veranstaltung.Length);
                Save_Veranstaltung(veranstaltung);
            }

            OpenWeather openWeather = await WetterController.GetOpenWeather(ip);

            if (openWeather != null)
            {
                SetOpenWeather(openWeather);
                Save_OpenWeather(openWeather);
            }

            Tabs tabs = await TabController.GetTabs(ip);

            if (tabs != null)
            {
                SetTabs(tabs);
                Save_Tabs(tabs);
            }

            BelegungsPlan belegungsPlan = await BelegungsPlanController.GetSchedule(ip);

            if (belegungsPlan != null)
            {
                DeleteAllEvents();
                SetSchedule(belegungsPlan);
                Save_Schedule(belegungsPlan);
            }
        }

        public async void Install_Clicked(object sender, EventArgs e)
        {
            SwitchButtonEnability(false);
            await HardwareController.InstallApp(ip);

            string[] save = Save.LoadLogin(ip);
            await HardwareController.SetLoginData(ip, new LoginData { Username = save[1], Password = save[2] });

            SwitchButtonEnability(true);
        }

        public async void Uninstall_Clicked(object sender, EventArgs e)
        {
            SwitchButtonEnability(false);
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Alle Komponenten der IoT-Applikation werden vom Gerät entfernt.\n", "Wirklich fortfahren?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                await HardwareController.UninstallAPP(ip);
                Refresh_Clicked(null, null);
                Refresh_Clicked(null, null);
            }
            SwitchButtonEnability(true);
        }

        public async void Restart_Clicked(object sender, EventArgs e)
        {
            await NetworkinfoService.RestartApp(ip);
        }

        public async void Screen_Clicked(object sender, EventArgs e)
        {
            BitmapImage img = await HardwareController.GetScreenShot(ip);
            if (img != null)
            {
                Screenshot.Source = img;
                Screenshot.Stretch = Stretch.Fill;
            }

        }

        public async void Login_Clicked(object sender, EventArgs e)
        {
            string strUsername = "";
            string strPassword = "";


            CredentialDialog crDiag = new CredentialDialog
            {
                Content = "Diese Information stammen vom Windows 10 IoT Gerät und werden für die Kommunikation mit der API benötigt. " +
                "Die Daten werden nur lokal gespeichert. Es werden keine Änderungen an das Gerät gesendet.",
                MainInstruction = "Bitte geben Sie Benutzername und Passwort für " + ip + " ein.",
                Target = "AnmeldeFenster",

                ShowSaveCheckBox = true
            };

            if (crDiag.ShowDialog() == true)
            {

                strUsername = crDiag.UserName;
                strPassword = crDiag.Password;

                NetworkinfoService.ChangeHttp(strUsername, strPassword);
                bool valid = await NetworkinfoService.Authentification(ip);

                if (valid && crDiag.IsSaveChecked)
                {
                    Save.SaveLogin(ip, strUsername, strPassword);
                    HardwareInfo hi = await HardwareController.GetData(ip);

                    if (hi != null)
                    {
                        SetGeratTab(hi);
                        Save_DeviceInfo(hi);
                    }
                }

                else if (!valid)
                {
                    System.Windows.Forms.MessageBox.Show("Benutzername und Passwort sind entweder nicht korrekt oder das Gerät ist offline. \n\nWenn Sie Daten senden oder empfangen möchten, werden Sie wiederholt nach den Zugangsdaten gefragt.", "Benutzername und Passwort inkorrekt", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

            }
        }

        public async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                int waittimer = 20000;
                bool devicenameChanged = false;
                bool successDevice = false;

                SwitchButtonEnability(false);

                if (DeviceNameIsChanged())
                {
                    successDevice = await SetDeviceName();
                    devicenameChanged = true;
                    await Task.Delay(waittimer);
                }

                if (PasswordIsChanged())
                {
                    SetPassword();
                    await Task.Delay(waittimer);
                }

                if (devicenameChanged)
                {
                    if (successDevice)
                        ShowRestartWarning();
                    else
                        ShowSendErrorMessage();
                    await Task.Delay(waittimer);
                }

                if (ProfilIsChanged() || profilchanged)
                {
                    await Send_Profil();
                    await Task.Delay(waittimer);
                }

                if (AktuellesIsChanged() || profilchanged)
                {
                    await Send_Aktuelles();
                    await Task.Delay(waittimer);
                }

                if (VeranstaltungIsChanged() || profilchanged)
                {
                    await Send_Veranstaltung();
                    await Task.Delay(waittimer);
                }

                if (OpenWeatherIsChanged() || profilchanged)
                {
                    await Send_OpenWeather();
                    await Task.Delay(waittimer);
                }

                if (InfoLeisteIsChanged() || profilchanged)
                {
                    await Send_InfoLeiste();
                    await Task.Delay(waittimer);
                }

                if (ScheduleIsChanged() || profilchanged)
                {
                    await Send_Schedule();
                    await Task.Delay(waittimer);
                }

                if (TabsIsChanged() || profilchanged)
                {
                    bool success = await Send_Tabs();

                    if (!success)
                    {
                        ShowTabErrorMessage();
                    }
                    await Task.Delay(waittimer);
                }

            }
            finally
            {
                profilchanged = false;
                FinishedDialog();
                SwitchButtonEnability(true);
            }
        }

        public async void Prufung_Clicked(object sender, EventArgs e)
        {
            pruefung = !pruefung;
            await HardwareController.StartPruefung(ip, pruefung);
        }

        public void LeftLogoButton_Clicked(object sender, EventArgs e)
        {
            BitmapImage image = ProfilController.GetFile();
            LeftLogo.Source = image;
        }

        public void RightLogoButton_Clicked(object sender, EventArgs e)
        {
            BitmapImage image = ProfilController.GetFile();
            RightLogo.Source = image;
        }

        public void ProfilImage1_Clicked(object sender, EventArgs e)
        {
            BitmapImage image = ProfilController.GetFile();
            ProfilImage1.Source = image;
        }

        public void ProfilImage2_Clicked(object sender, EventArgs e)
        {
            BitmapImage image = ProfilController.GetFile();
            ProfilImage2.Source = image;
        }

        public void ProfilImage3_Clicked(object sender, EventArgs e)
        {
            BitmapImage image = ProfilController.GetFile();
            ProfilImage3.Source = image;
        }

        public void CheckProfil1_Activate(object sender, EventArgs e)
        {
            //Aktiviere Eingabefelder
            if (ProfilTab.IsChecked == true)
            {
                DocChecked1.IsEnabled = true;
                ProfChecked1.IsEnabled = true;
                OtherChecked1.IsEnabled = true;
                OtherText1.IsEnabled = true;

                Forename1.IsEnabled = true;
                Surname1.IsEnabled = true;
                Job1.IsEnabled = true;

                Street1.IsEnabled = true;
                Zip1.IsEnabled = true;
                Place1.IsEnabled = true;

                Phone1.IsEnabled = true;
                Fax1.IsEnabled = true;
                Email1.IsEnabled = true;

                ProfilImage1Button.IsEnabled = true;

                //Aktiviere Label
                ProfText1.IsEnabled = true;
                DocText1.IsEnabled = true;
                ProfilTitle1.IsEnabled = true;
                ProfVorname1.IsEnabled = true;
                ProfName1.IsEnabled = true;
                ProfBeruf1.IsEnabled = true;
                ProfStreet1.IsEnabled = true;
                ProfPLZ1.IsEnabled = true;
                ProfPlace1.IsEnabled = true;
                ProfPhone1.IsEnabled = true;
                ProfFax1.IsEnabled = true;
                ProfEmail1.IsEnabled = true;

                ProfilImage1.IsEnabled = true;
            }
        }

        public void CheckProfil1_Deactivate(object sender, EventArgs e)
        {

            DocChecked1.IsEnabled = false;
            ProfChecked1.IsEnabled = false;
            OtherChecked1.IsEnabled = false;
            OtherText1.IsEnabled = false;

            Forename1.IsEnabled = false;
            Surname1.IsEnabled = false;
            Job1.IsEnabled = false;

            Street1.IsEnabled = false;
            Zip1.IsEnabled = false;
            Place1.IsEnabled = false;

            Phone1.IsEnabled = false;
            Fax1.IsEnabled = false;
            Email1.IsEnabled = false;

            ProfilImage1Button.IsEnabled = false;

            //Aktiviere Label
            ProfText1.IsEnabled = false;
            DocText1.IsEnabled = false;
            ProfilTitle1.IsEnabled = false;
            ProfVorname1.IsEnabled = false;
            ProfName1.IsEnabled = false;
            ProfBeruf1.IsEnabled = false;
            ProfStreet1.IsEnabled = false;
            ProfPLZ1.IsEnabled = false;
            ProfPlace1.IsEnabled = false;
            ProfPhone1.IsEnabled = false;
            ProfFax1.IsEnabled = false;
            ProfEmail1.IsEnabled = false;

            ProfilImage1.IsEnabled = false;
        }

        public void CheckProfil2_Activate(object sender, EventArgs e)
        {
            if (ProfilTab.IsChecked == true)
            {
                //Aktiviere Eingabefelder
                DocChecked2.IsEnabled = true;
                ProfChecked2.IsEnabled = true;
                OtherChecked2.IsEnabled = true;
                OtherText2.IsEnabled = true;

                Forename2.IsEnabled = true;
                Surname2.IsEnabled = true;
                Job2.IsEnabled = true;

                Street2.IsEnabled = true;
                Zip2.IsEnabled = true;
                Place2.IsEnabled = true;

                Phone2.IsEnabled = true;
                Fax2.IsEnabled = true;
                Email2.IsEnabled = true;

                ProfilImage2Button.IsEnabled = true;

                //Aktiviere Label
                ProfText2.IsEnabled = true;
                DocText2.IsEnabled = true;
                ProfilTitle2.IsEnabled = true;
                ProfVorname2.IsEnabled = true;
                ProfName2.IsEnabled = true;
                ProfBeruf2.IsEnabled = true;
                ProfStreet2.IsEnabled = true;
                ProfPLZ2.IsEnabled = true;
                ProfPlace2.IsEnabled = true;
                ProfPhone2.IsEnabled = true;
                ProfFax2.IsEnabled = true;
                ProfEmail2.IsEnabled = true;

                ProfilImage2.IsEnabled = true;
            }
        }

        public void CheckProfil2_Deactivate(object sender, EventArgs e)
        {

            DocChecked2.IsEnabled = false;
            ProfChecked2.IsEnabled = false;
            OtherChecked2.IsEnabled = false;
            OtherText2.IsEnabled = false;

            Forename2.IsEnabled = false;
            Surname2.IsEnabled = false;
            Job2.IsEnabled = false;

            Street2.IsEnabled = false;
            Zip2.IsEnabled = false;
            Place2.IsEnabled = false;

            Phone2.IsEnabled = false;
            Fax2.IsEnabled = false;
            Email2.IsEnabled = false;

            ProfilImage2Button.IsEnabled = false;

            //Aktiviere Label
            ProfText2.IsEnabled = false;
            DocText2.IsEnabled = false;
            ProfilTitle2.IsEnabled = false;
            ProfVorname2.IsEnabled = false;
            ProfName2.IsEnabled = false;
            ProfBeruf2.IsEnabled = false;
            ProfStreet2.IsEnabled = false;
            ProfPLZ2.IsEnabled = false;
            ProfPlace2.IsEnabled = false;
            ProfPhone2.IsEnabled = false;
            ProfFax2.IsEnabled = false;
            ProfEmail2.IsEnabled = false;

            ProfilImage2.IsEnabled = false;
        }

        public void CheckProfil3_Activate(object sender, EventArgs e)
        {
            if (ProfilTab.IsChecked == true)
            {
                //Aktiviere Eingabefelder
                DocChecked3.IsEnabled = true;
                ProfChecked3.IsEnabled = true;
                OtherChecked3.IsEnabled = true;
                OtherText3.IsEnabled = true;

                Forename3.IsEnabled = true;
                Surname3.IsEnabled = true;
                Job3.IsEnabled = true;

                Street3.IsEnabled = true;
                Zip3.IsEnabled = true;
                Place3.IsEnabled = true;

                Phone3.IsEnabled = true;
                Fax3.IsEnabled = true;
                Email3.IsEnabled = true;

                ProfilImage3Button.IsEnabled = true;

                //Aktiviere Label
                ProfText3.IsEnabled = true;
                DocText3.IsEnabled = true;
                ProfilTitle3.IsEnabled = true;
                ProfVorname3.IsEnabled = true;
                ProfName3.IsEnabled = true;
                ProfBeruf3.IsEnabled = true;
                ProfStreet3.IsEnabled = true;
                ProfPLZ3.IsEnabled = true;
                ProfPlace3.IsEnabled = true;
                ProfPhone3.IsEnabled = true;
                ProfFax3.IsEnabled = true;
                ProfEmail3.IsEnabled = true;

                ProfilImage3.IsEnabled = true;
            }
        }

        public void CheckProfil3_Deactivate(object sender, EventArgs e)
        {

            DocChecked3.IsEnabled = false;
            ProfChecked3.IsEnabled = false;
            OtherChecked3.IsEnabled = false;
            OtherText3.IsEnabled = false;

            Forename3.IsEnabled = false;
            Surname3.IsEnabled = false;
            Job3.IsEnabled = false;

            Street3.IsEnabled = false;
            Zip3.IsEnabled = false;
            Place3.IsEnabled = false;

            Phone3.IsEnabled = false;
            Fax3.IsEnabled = false;
            Email3.IsEnabled = false;

            ProfilImage3Button.IsEnabled = false;

            //Aktiviere Label
            ProfText3.IsEnabled = false;
            DocText3.IsEnabled = false;
            ProfilTitle3.IsEnabled = false;
            ProfVorname3.IsEnabled = false;
            ProfName3.IsEnabled = false;
            ProfBeruf3.IsEnabled = false;
            ProfStreet3.IsEnabled = false;
            ProfPLZ3.IsEnabled = false;
            ProfPlace3.IsEnabled = false;
            ProfPhone3.IsEnabled = false;
            ProfFax3.IsEnabled = false;
            ProfEmail3.IsEnabled = false;

            ProfilImage3.IsEnabled = false;
        }

        private void EventFrom1_Changed(object Sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem EventFromC = (ComboBoxItem)EventFrom1.SelectedValue;
            ComboBoxItem EventToC = (ComboBoxItem)EventTo1.Items[0];
            int EventFromInt = Int32.Parse(EventFromC.Content.ToString());
            int EventToInt = Int32.Parse(EventToC.Content.ToString());

            if (EventFromInt >= EventToInt)
            {
                int difference = EventFromInt - EventToInt + 1 - helpEventDifference[0];
                helpEventDifference[0] += difference;

                for (int i = 0; i < difference; i++)
                {
                    EventTo1.Items.RemoveAt(0);
                    EventTo1.SelectedIndex = 0;
                }
            }
            else
            {
                for (int i = EventToInt - 1; i > EventFromInt; i--)
                {
                    var comboContent = new ComboBoxItem
                    {
                        Content = i
                    };
                    EventTo1.Items.Insert(0, comboContent);
                    helpEventDifference[0] -= 1;
                }
            }
        }

        private void EventFrom2_Changed(object Sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem EventFromC = (ComboBoxItem)EventFrom2.SelectedValue;
            ComboBoxItem EventToC = (ComboBoxItem)EventTo2.Items[0];
            int EventFromInt = Int32.Parse(EventFromC.Content.ToString());
            int EventToInt = Int32.Parse(EventToC.Content.ToString());

            if (EventFromInt >= EventToInt)
            {
                int difference = EventFromInt - EventToInt + 1 - helpEventDifference[1];
                helpEventDifference[1] += difference;

                for (int i = 0; i < difference; i++)
                {
                    EventTo2.Items.RemoveAt(0);
                    EventTo2.SelectedIndex = 0;
                }
            }
            else
            {
                for (int i = EventToInt - 1; i > EventFromInt; i--)
                {
                    var comboContent = new ComboBoxItem
                    {
                        Content = i
                    };
                    EventTo2.Items.Insert(0, comboContent);
                    helpEventDifference[1] -= 1;
                }
            }
        }

        private void EventFrom3_Changed(object Sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem EventFromC = (ComboBoxItem)EventFrom3.SelectedValue;
            ComboBoxItem EventToC = (ComboBoxItem)EventTo3.Items[0];
            int EventFromInt = Int32.Parse(EventFromC.Content.ToString());
            int EventToInt = Int32.Parse(EventToC.Content.ToString());

            if (EventFromInt >= EventToInt)
            {
                int difference = EventFromInt - EventToInt + 1 - helpEventDifference[2];
                helpEventDifference[2] += difference;

                for (int i = 0; i < difference; i++)
                {
                    EventTo3.Items.RemoveAt(0);
                    EventTo3.SelectedIndex = 0;
                }
            }
            else
            {
                for (int i = EventToInt - 1; i > EventFromInt; i--)
                {
                    var comboContent = new ComboBoxItem
                    {
                        Content = i
                    };
                    EventTo3.Items.Insert(0, comboContent);
                    helpEventDifference[2] -= 1;
                }
            }
        }

        private void EventFrom4_Changed(object Sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem EventFromC = (ComboBoxItem)EventFrom4.SelectedValue;
            ComboBoxItem EventToC = (ComboBoxItem)EventTo4.Items[0];
            int EventFromInt = Int32.Parse(EventFromC.Content.ToString());
            int EventToInt = Int32.Parse(EventToC.Content.ToString());

            if (EventFromInt >= EventToInt)
            {
                int difference = EventFromInt - EventToInt + 1 - helpEventDifference[3];
                helpEventDifference[3] += difference;

                for (int i = 0; i < difference; i++)
                {
                    EventTo4.Items.RemoveAt(0);
                    EventTo4.SelectedIndex = 0;
                }
            }
            else
            {
                for (int i = EventToInt - 1; i > EventFromInt; i--)
                {
                    var comboContent = new ComboBoxItem
                    {
                        Content = i
                    };
                    EventTo4.Items.Insert(0, comboContent);
                    helpEventDifference[3] -= 1;
                }
            }
        }

        private void NumberOfEvents_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem countC = (ComboBoxItem)NumberOfEvents.SelectedItem;
            int count = Int32.Parse(countC.Content.ToString());

            if (count > 1 && EventTab.IsChecked == true)
            {
                EventTheme2.IsEnabled = true;
                EventTitle2.IsEnabled = true;
                EventTo2.IsEnabled = true;
                EventFrom2.IsEnabled = true;
                EventDay2.IsEnabled = true;
                EventLabelFrom2.IsEnabled = true;
                EventLabelTitle2.IsEnabled = true;
                EventLabelTo2.IsEnabled = true;

            }
            else
            {
                EventTheme2.IsEnabled = false;
                EventTitle2.IsEnabled = false;
                EventTo2.IsEnabled = false;
                EventFrom2.IsEnabled = false;
                EventDay2.IsEnabled = false;
                EventLabelFrom2.IsEnabled = false;
                EventLabelTitle2.IsEnabled = false;
                EventLabelTo2.IsEnabled = false;
            }

            if (count > 2 && EventTab.IsChecked == true)
            {
                EventTheme3.IsEnabled = true;
                EventTitle3.IsEnabled = true;
                EventTo3.IsEnabled = true;
                EventFrom3.IsEnabled = true;
                EventDay3.IsEnabled = true;
                EventLabelFrom3.IsEnabled = true;
                EventLabelTitle3.IsEnabled = true;
                EventLabelTo3.IsEnabled = true;
            }
            else
            {
                EventTheme3.IsEnabled = false;
                EventTitle3.IsEnabled = false;
                EventTo3.IsEnabled = false;
                EventFrom3.IsEnabled = false;
                EventDay3.IsEnabled = false;
                EventLabelFrom3.IsEnabled = false;
                EventLabelTitle3.IsEnabled = false;
                EventLabelTo3.IsEnabled = false;
            }
            if (count > 3 && EventTab.IsChecked == true)
            {
                EventTheme4.IsEnabled = true;
                EventTitle4.IsEnabled = true;
                EventTo4.IsEnabled = true;
                EventFrom4.IsEnabled = true;
                EventDay4.IsEnabled = true;
                EventLabelFrom4.IsEnabled = true;
                EventLabelTitle4.IsEnabled = true;
                EventLabelTo4.IsEnabled = true;
            }
            else
            {
                EventTheme4.IsEnabled = false;
                EventTitle4.IsEnabled = false;
                EventTo4.IsEnabled = false;
                EventFrom4.IsEnabled = false;
                EventDay4.IsEnabled = false;
                EventLabelFrom4.IsEnabled = false;
                EventLabelTitle4.IsEnabled = false;
                EventLabelTo4.IsEnabled = false;
            }
        }

        private void ProfilTab_Activate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(ProfilMainTab, true))
            {
                form.SetCurrentValue(IsEnabledProperty, true);
            }

            if (CheckProfil1.IsChecked == true)
            {
                CheckProfil1_Activate(null, null);
            }
            else
            {
                CheckProfil1_Deactivate(null, null);
            }

            if (CheckProfil2.IsChecked == true)
            {
                CheckProfil2_Activate(null, null);
            }
            else
            {
                CheckProfil2_Deactivate(null, null);
            }

            if (CheckProfil3.IsChecked == true)
            {
                CheckProfil3_Activate(null, null);
            }
            else
            {
                CheckProfil3_Deactivate(null, null);
            }

        }

        private void ProfilTab_Deactivate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(ProfilMainTab, false))
            {
                form.SetCurrentValue(IsEnabledProperty, false);
            }


        }

        private void ActualTab_Activate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(ActualMainTab, true))
            {
                form.SetCurrentValue(IsEnabledProperty, true);
            }
        }

        private void ActualTab_Deactivate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(ActualMainTab, false))
            {
                form.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void EventTab_Activate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(EventMainTab, true))
            {
                form.SetCurrentValue(IsEnabledProperty, true);
            }
            NumberOfEvents_Changed(null, null);
        }

        private void EventTab_Deactivate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(EventMainTab, false))
            {
                form.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void ScheduleTab_Activate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(ScheduleMainTab, true))
            {
                form.SetCurrentValue(IsEnabledProperty, true);
            }
        }

        private void ScheduleTab_Deactivate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(ScheduleMainTab, false))
            {
                form.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void WeatherTab_Activate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(WeatherMainTab, true))
            {
                form.SetCurrentValue(IsEnabledProperty, true);
            }
        }

        private void WeatherTab_Deactivate(object sender, EventArgs e)
        {
            foreach (var form in SetEnability(WeatherMainTab, false))
            {
                form.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void ScheduleFrom_Changed(object sender, SelectionChangedEventArgs e)
        {


            ComboBoxItem scheduleFromC = (ComboBoxItem)ScheduleFrom.SelectedValue;
            ComboBoxItem scheduleToC = (ComboBoxItem)ScheduleTo.Items[0];
            int day = ScheduleDay.SelectedIndex;

            int ScheduleFromInt = Int32.Parse(scheduleFromC.Content.ToString());
            int ScheduleToInt = Int32.Parse(scheduleToC.Content.ToString());


            int difference = ScheduleFromInt - ScheduleToInt + 1;

            if (ScheduleFromInt >= ScheduleToInt)
            {

                for (int i = 0; i < difference; i++)
                {
                    ScheduleTo.Items.RemoveAt(0);
                    ScheduleTo.SelectedIndex = 0;
                }
            }
            else
            {
                for (int i = ScheduleToInt - 1; i > ScheduleFromInt; i--)
                {
                    var comboContent = new ComboBoxItem
                    {
                        Content = i
                    };
                    ScheduleTo.Items.Insert(0, comboContent);
                }
            }

            bool begin = false;
            foreach (ComboBoxItem elem in ScheduleTo.Items)
            {
                if (ValuedItemsArray[day].Contains(Int32.Parse(elem.Content.ToString())) && !begin)
                {
                    begin = true;
                }

                else if (begin)
                {
                    DependencyObject item = elem;
                    item.SetCurrentValue(IsEnabledProperty, false);

                }

            }

            if (!begin)
                foreach (ComboBoxItem elem in ScheduleTo.Items)
                {

                    DependencyObject item = elem;
                    item.SetCurrentValue(IsEnabledProperty, true);

                }


        }

        private void ScheduleDay_Changed(object sender, SelectionChangedEventArgs e)
        {
            Schedule_ReValidation();
        }

        private void ScheduleDeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            Schedule_ReValidation();
            DeleteEvent();
        }



        private void ScheduleAddButton_Clicked(object sender, RoutedEventArgs e)
        {

            BelegungsPlan belegungsPlan = AddEvent();
        }


        /////////////////////////////////////End Formhandler /////////////////////////

        private void SwitchButtonEnability(bool activate)
        {
            if (activate)
            {
                Install.IsEnabled = true;
                Uninstall.IsEnabled = true;
                Refresh.IsEnabled = true;
                Pruefung.IsEnabled = true;
                SaveButton.IsEnabled = true;
            }
            else
            {
                Install.IsEnabled = false;
                Uninstall.IsEnabled = false;
                Refresh.IsEnabled = false;
                Pruefung.IsEnabled = false;
                SaveButton.IsEnabled = false;
            }
        }

        private void FinishedDialog()
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(String.Format("Senden und Speichern für {0} beendet", ip), "Sendevorgang", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool PasswordIsChanged()
        {
            string[] data = Save.LoadLogin(ip);

            if (data == null)
            {
                return false;
            }

            if (!data[2].Equals(Password.Password))
                return true;
            return false;
        }

        private bool DeviceNameIsChanged()
        {
            HardwareInfo hf = Save.LoadDeviceInfo(ip);

            if (hf != null && !hf.Name.Equals(HardwareName.Text))
                return true;
            return false;
        }

        private void Save_DeviceInfo(HardwareInfo hf)
        {
            Save.SaveDeviceInfo(ip, hf);
        }

        private void Load_DeviceInfo()
        {
            HardwareInfo hi = Save.LoadDeviceInfo(ip);

            if (hi != null)
                SetGeratTab(hi);
        }

        private void GetLogin()
        {
            string[] loginData = Save.LoadLogin(ip);
            if (loginData != null)
            {
                Username.Text = loginData[1];
                Password.Password = loginData[2];
            }
        }

        private async Task<bool> SetDeviceName()
        {
            return await HardwareController.SetDeviceName(ip, HardwareName.Text);
        }

        private async void SetPassword()
        {
            string[] data = Save.LoadLogin(ip);
            bool success = await HardwareController.SetPassword(ip, data[2], Password.Password);

            if (success)
                Save.SaveLogin(ip, data[1], Password.Password);

        }
        private void ShowRestartWarning()
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Es wurden Information geändert, die erst nach einem Neustart des IoT-Gerätes realisiert werden können.\nSoll das Gerät jetzt neustarten?\nDies wird einige Minuten in Anspruch nehmen.", "Gerät neustarten?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Restart_Clicked(null, null);
            }
        }



        private void ShowSendErrorMessage()
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Daten können nicht gesendet werden.\nBitte stellen Sie sicher, dass eine Verbindung zwischen den PC und dem IoT-Gerät besteht.", "Sendefehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowTabErrorMessage()
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Es muss mindestens ein Tab aktiviert sein oder die Verbindung mit dem IoT-Gerät ist unterbrochen. Die Tab-Einstellungen konnten nicht gesendet werden.", "Sendefehler!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        /////////////////////////////Profil Controller ////////////////////////////////////////////////////////////////


        public void SetProfilTab()
        {
            Get_InfoLeiste();
            Get_Profils();
        }

        public void Get_InfoLeiste()
        {
            Infoleiste infoleiste = Save.LoadInfoLeiste(ip);

            if (infoleiste != null)
                SetInfoLeiste(infoleiste);

        }


        private void SetInfoLeiste(Infoleiste infoleiste)
        {
            if (infoleiste != null)
            {
                InfoTitle.Text = infoleiste.Title;
                Room.Text = infoleiste.Room;
                Chair.Text = infoleiste.Chair;
                if (infoleiste.LeftImage != null && infoleiste.LeftImage.Length != 0)
                {
                    LeftLogo.Source = ByteArrayBitmapService.ToImage(infoleiste.LeftImage);

                }
                if (infoleiste.RightImage != null && infoleiste.RightImage.Length != 0)
                {
                    RightLogo.Source = ByteArrayBitmapService.ToImage(infoleiste.RightImage);

                }
            }
        }

        private void Load_InfoLeiste()
        {
            Infoleiste infoleiste = Save.LoadInfoLeiste(ip);

            if (infoleiste != null)
                SetInfoLeiste(infoleiste);
        }

        private void Save_InfoLeiste(Infoleiste infoL)
        {
            Save.SaveInfoLeiste(ip, infoL);
        }

        private async Task Send_InfoLeiste()
        {

            Infoleiste infoleiste = new Infoleiste
            {
                Title = InfoTitle.Text,
                Room = Room.Text,
                Chair = Chair.Text,
                LeftImage = ByteArrayBitmapService.ToByteArray(LeftLogo.Source as BitmapImage),
                RightImage = ByteArrayBitmapService.ToByteArray(RightLogo.Source as BitmapImage)
            };
            bool send = await ProfilController.SetInfoLeiste(ip, infoleiste);

            if (send)
            {
                SetInfoLeiste(infoleiste);
                Save_InfoLeiste(infoleiste);
            }
        }

        private bool InfoLeisteIsChanged()
        {
            Infoleiste infoleiste = Save.LoadInfoLeiste(ip);

            if (infoleiste != null)
            {
                if (!infoleiste.Title.Equals(InfoTitle.Text) || !infoleiste.Room.Equals(Room.Text) || !infoleiste.Chair.Equals(Chair.Text)
                    || !infoleiste.LeftImage.SequenceEqual(ByteArrayBitmapService.ToByteArray(LeftLogo.Source as BitmapImage))
                    || !infoleiste.RightImage.SequenceEqual(ByteArrayBitmapService.ToByteArray(RightLogo.Source as BitmapImage)))
                    return true;
                else
                    return false;
            }
            else
            {
                if (InfoTitle.Text != "" || Room.Text != "" || Chair.Text != "" || LeftLogo.Source != null || RightLogo.Source != null)
                    return true;
                else
                    return false;
            }
        }

        public void Get_Profils()
        {
            Profil[] profils = Save.LoadProfil(ip);

            if (profils != null && profils.Length > 0)
                SetProfil(profils);

        }

        private void SetProfil(Profil[] profil)
        {
            string other = "";

            bool[] profilchecked = Save.LoadProfilChecked(ip);
            int i = 0;
            //Profil 1

            if (profil.Length > i)
            {
                if (profilchecked[0] == true)
                    CheckProfil1.IsChecked = true;

                if (profil[0].Title.Contains("Prof. "))
                    ProfChecked1.IsChecked = true;
                else
                    ProfChecked1.IsChecked = false;

                if (profil[0].Title.Contains("Dr. "))
                    DocChecked1.IsChecked = true;
                else
                    DocChecked1.IsChecked = false;

                other = profil[0].Title;
                other = other.Replace("Prof. ", "");
                other = other.Replace("Dr. ", "");

                if (other != "")
                {
                    OtherChecked1.IsChecked = true;
                    OtherText1.Text = other;
                }
                else
                    OtherChecked1.IsChecked = false;

                Forename1.Text = profil[0].Name;
                Surname1.Text = profil[0].Surname;
                Job1.Text = profil[0].Role;

                Street1.Text = profil[0].Street;
                Zip1.Text = profil[0].Placenumber.ToString();
                Place1.Text = profil[0].Place;

                Phone1.Text = profil[0].Phone;
                Fax1.Text = profil[0].Fax;
                Email1.Text = profil[0].Email;

                if (Zip1.Text == "0")
                    Zip1.Text = "";

                if (profil[0].Image != null && profil[0].Image.Length != 0)
                {
                    ProfilImage1.Source = ByteArrayBitmapService.ToImage(profil[0].Image);
                }
                i++;
            }

            //Profil 2

            if (profil.Length > i)
            {
                if (profilchecked[1] == true)
                    CheckProfil2.IsChecked = true;

                if (profil[1].Title.Contains("Prof. "))
                    ProfChecked2.IsChecked = true;
                else
                    ProfChecked2.IsChecked = false;

                if (profil[1].Title.Contains("Dr. "))
                    DocChecked2.IsChecked = true;
                else
                    DocChecked2.IsChecked = false;

                other = profil[1].Title;
                other = other.Replace("Prof. ", "");
                other = other.Replace("Dr. ", "");

                if (other != "")
                {
                    OtherChecked2.IsChecked = true;
                    OtherText2.Text = other;
                }
                else
                    OtherChecked2.IsChecked = false;


                Forename2.Text = profil[1].Name;
                Surname2.Text = profil[1].Surname;
                Job2.Text = profil[1].Role;

                Street2.Text = profil[1].Street;
                Zip2.Text = profil[1].Placenumber.ToString();
                Place2.Text = profil[1].Place;

                Phone2.Text = profil[1].Phone;
                Fax2.Text = profil[1].Fax;
                Email2.Text = profil[1].Email;

                if (Zip2.Text == "0")
                    Zip2.Text = "";

                if (profil[1].Image != null && profil[1].Image.Length != 0)
                {
                    ProfilImage2.Source = ByteArrayBitmapService.ToImage(profil[1].Image);
                }

                i++;
            }

            //Profil 3

            if (profil.Length > i)
            {
                if (profilchecked[2] == true)
                    CheckProfil3.IsChecked = true;

                if (profil[2].Title.Contains("Prof. "))
                    ProfChecked3.IsChecked = true;
                else
                    ProfChecked3.IsChecked = false;

                if (profil[2].Title.Contains("Dr. "))
                    DocChecked3.IsChecked = true;
                else
                    DocChecked3.IsChecked = false;

                other = profil[2].Title;
                other = other.Replace("Prof. ", "");
                other = other.Replace("Dr. ", "");

                if (other != "")
                {
                    OtherChecked3.IsChecked = true;
                    OtherText3.Text = other;
                }
                else
                    OtherChecked3.IsChecked = false;

                Forename3.Text = profil[2].Name;
                Surname3.Text = profil[2].Surname;
                Job3.Text = profil[2].Role;

                Street3.Text = profil[2].Street;
                Zip3.Text = profil[2].Placenumber.ToString();
                Place3.Text = profil[2].Place;

                Phone3.Text = profil[2].Phone;
                Fax3.Text = profil[2].Fax;
                Email3.Text = profil[2].Email;

                if (Zip3.Text == "0")
                    Zip3.Text = "";

                if (profil[2].Image != null && profil[2].Image.Length != 0)
                {
                    ProfilImage3.Source = ByteArrayBitmapService.ToImage(profil[2].Image);
                }
                i++;
            }


        }

        private bool ProfilIsChanged()
        {
            Profil[] profils = Save.LoadProfil(ip);

            if (profils != null)
            {
                bool used1 = false;
                bool used2 = false;
                bool used3 = false;

                for (int i = 0; i < profils.Length; i++)
                {
                    if (CheckProfil1.IsChecked == true && !used1)
                    {
                        if (!profils[i].Name.Equals(Forename1.Text) || !profils[i].Surname.Equals(Surname1.Text) || !profils[i].Role.Equals(Job1.Text)
                            || !profils[i].Street.Equals(Street1.Text) || !profils[i].Placenumber.ToString().Equals(Zip1.Text) || !profils[i].Place.Equals(Place1.Text)
                            || !profils[i].Phone.Equals(Phone1.Text) || !profils[i].Fax.Equals(Fax1.Text) || !profils[i].Email.Equals(Email1.Text)
                            || !profils[i].Image.SequenceEqual(ByteArrayBitmapService.ToByteArray(ProfilImage1.Source as BitmapImage)))

                            return true;

                    }

                    if (CheckProfil2.IsChecked == true && !used2)
                    {
                        if (!profils[i].Name.Equals(Forename2.Text) || !profils[i].Surname.Equals(Surname2.Text) || !profils[i].Role.Equals(Job2.Text)
                        || !profils[i].Street.Equals(Street2.Text) || !profils[i].Placenumber.ToString().Equals(Zip2.Text) || !profils[i].Place.Equals(Place2.Text)
                        || !profils[i].Phone.Equals(Phone2.Text) || !profils[i].Fax.Equals(Fax2.Text) || !profils[i].Email.Equals(Email2.Text)
                        || !profils[i].Image.SequenceEqual(ByteArrayBitmapService.ToByteArray(ProfilImage2.Source as BitmapImage)))

                            return true;
                    }

                    if (CheckProfil3.IsChecked == true && !used3)
                    {
                        if (!profils[i].Name.Equals(Forename3.Text) || !profils[i].Surname.Equals(Surname3.Text) || !profils[i].Role.Equals(Job3.Text)
                        || !profils[i].Street.Equals(Street3.Text) || !profils[i].Placenumber.ToString().Equals(Zip3.Text) || !profils[i].Place.Equals(Place3.Text)
                        || !profils[i].Phone.Equals(Phone3.Text) || !profils[i].Fax.Equals(Fax3.Text) || !profils[i].Email.Equals(Email3.Text)
                        || !profils[i].Image.SequenceEqual(ByteArrayBitmapService.ToByteArray(ProfilImage2.Source as BitmapImage)))

                            return true;
                    }
                }
                return false;

            }
            else
            {
                if (CheckProfil1.IsChecked == true)
                {
                    if (Forename1.Text != "" || Surname1.Text != "" || Job1.Text != ""
                        || Street1.Text != "" || Zip1.Text != "" || Place1.Text != ""
                        || Phone1.Text != "" || Fax1.Text != "" || Email1.Text != ""
                        || ProfilImage1.Source != null)

                        return true;
                }

                if (CheckProfil2.IsChecked == true)
                {

                    if (Forename2.Text != "" || Surname2.Text != "" || Job2.Text != ""
                    || Street2.Text != "" || Zip2.Text != "" || Place2.Text != ""
                    || Phone2.Text != "" || Fax2.Text != "" || Email2.Text != ""
                    || ProfilImage2.Source != null)

                        return true;
                }
                if (CheckProfil3.IsChecked == true)
                {
                    if (Forename3.Text != "" || Surname3.Text != "" || Job3.Text != ""
                    || Street3.Text != "" || Zip3.Text != "" || Place3.Text != ""
                    || Phone3.Text != "" || Fax3.Text != "" || Email3.Text != ""
                    || ProfilImage3.Source != null)

                        return true;
                }
                return false;
            }
        }

        private void Load_Profil()
        {
            Profil[] profil = Save.LoadProfil(ip);

            if (profil != null)
                SetProfil(profil);
        }

        private void Save_Profil(Profil[] profil)
        {
            Save.SaveProfil(ip, profil);
        }

        private async Task Send_Profil()
        {
            int count = 0;
            int i = 0;

            if (CheckProfil1.IsChecked == true)
                count++;
            if (CheckProfil2.IsChecked == true)
                count++;
            if (CheckProfil3.IsChecked == true)
                count++;

            Profil[] profils = new Profil[count];
            Profil[] save_profils = new Profil[3];

            if (Zip1.Text == "")
            {
                Zip1.Text = "0";
            }

            Profil profil1 = new Profil
            {
                Title = "",

                Name = Forename1.Text,
                Surname = Surname1.Text,
                Role = Job1.Text,

                Street = Street1.Text,
                Place = Place1.Text,
                Placenumber = Int32.Parse(Zip1.Text),
                Country = "",

                Phone = Phone1.Text,
                Fax = Fax1.Text,
                Email = Email1.Text,

                Image = ByteArrayBitmapService.ToByteArray(ProfilImage1.Source as BitmapImage)
            };

            if (Zip1.Text == "0")
                Zip1.Text = "";

            if (ProfChecked1.IsChecked == true)
            {
                profil1.Title += "Prof. ";
            }

            if (DocChecked1.IsChecked == true)
            {
                profil1.Title += "Dr. ";
            }

            if (OtherChecked1.IsChecked == true)
            {
                profil1.Title += OtherText1.Text + " ";
            }
            save_profils[0] = profil1;
            if (CheckProfil1.IsChecked == true)
            {

                profils[i] = profil1;
                i++;
            }

            if (Zip2.Text == "")
            {
                Zip2.Text = "0";
            }
            Profil profil2 = new Profil
            {
                Title = "",

                Name = Forename2.Text,
                Surname = Surname2.Text,
                Role = Job2.Text,

                Street = Street2.Text,
                Place = Place2.Text,
                Placenumber = Int32.Parse(Zip2.Text),
                Country = "",

                Phone = Phone2.Text,
                Fax = Fax2.Text,
                Email = Email2.Text,

                Image = ByteArrayBitmapService.ToByteArray(ProfilImage2.Source as BitmapImage)
            };

            if (Zip2.Text == "0")
                Zip2.Text = "";

            if (ProfChecked2.IsChecked == true)
            {
                profil2.Title += "Prof. ";
            }

            if (DocChecked2.IsChecked == true)
            {
                profil2.Title += "Dr. ";
            }

            if (OtherChecked2.IsChecked == true)
            {
                profil2.Title += OtherText2.Text + " ";
            }

            save_profils[1] = profil2;
            if (CheckProfil2.IsChecked == true)
            {

                profils[i] = profil2;
                i++;
            }


            if (Zip3.Text == "")
            {
                Zip3.Text = "0";
            }
            Profil profil3 = new Profil
            {
                Title = "",

                Name = Forename3.Text,
                Surname = Surname3.Text,
                Role = Job3.Text,

                Street = Street3.Text,
                Place = Place3.Text,
                Placenumber = Int32.Parse(Zip3.Text),
                Country = "",

                Phone = Phone3.Text,
                Fax = Fax3.Text,
                Email = Email3.Text,

                Image = ByteArrayBitmapService.ToByteArray(ProfilImage3.Source as BitmapImage)
            };

            if (Zip3.Text == "0")
                Zip3.Text = "";

            if (ProfChecked3.IsChecked == true)
            {
                profil3.Title += "Prof. ";
            }

            if (DocChecked3.IsChecked == true)
            {
                profil3.Title += "Dr. ";
            }

            if (OtherChecked3.IsChecked == true)
            {
                profil3.Title += OtherText3.Text + " ";
            }

            save_profils[2] = profil3;
            if (CheckProfil3.IsChecked == true)
            {

                profils[i] = profil3;
                i++;
            }

            if (profils.Length > 0)
            {
                bool send = await ProfilController.SetProfil(ip, profils);

                if (send)
                {
                    bool check1 = CheckProfil1.IsChecked ?? false;
                    bool check2 = CheckProfil2.IsChecked ?? false;
                    bool check3 = CheckProfil3.IsChecked ?? false;
                    bool[] profilchecked = { check1, check2, check3 };
                    Save.SaveProfilChecked(ip, profilchecked);

                    SetProfil(save_profils);
                    Save_Profil(save_profils);
                }
            }


        }

        ////////////////////////////////Aktuelles /////////////////////////////////

        public void Get_Aktuelles()
        {
            Aktuelles aktuelles = Save.LoadAktuelles(ip);

            if (aktuelles != null)
                SetAktuelles(aktuelles);

        }

        private void SetAktuelles(Aktuelles aktuelles)
        {
            ActualTitle.Text = aktuelles.Title;
            ActualText.Text = aktuelles.Text;
        }

        private bool AktuellesIsChanged()
        {
            Aktuelles aktuelles = Save.LoadAktuelles(ip);

            if (aktuelles != null)
            {
                if (!aktuelles.Title.Equals(ActualTitle.Text) || !aktuelles.Text.Equals(ActualText.Text))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (!(ActualTitle.Text == "") || !(ActualText.Text == ""))
                {
                    return true;
                }
                return false;
            }
        }

        private void Load_Aktuelles()
        {
            Aktuelles aktuelles = Save.LoadAktuelles(ip);

            if (aktuelles != null)
                SetAktuelles(aktuelles);
        }

        private void Save_Aktuelles(Aktuelles aktuelles)
        {
            Save.SaveAktuelles(ip, aktuelles);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async Task Send_Aktuelles()
        {
            Aktuelles aktuelles = new Aktuelles
            {
                Title = ActualTitle.Text,
                Text = ActualText.Text
            };

            bool send = await AktuellesController.SetAktuelles(ip, aktuelles);

            if (send)
            {
                SetAktuelles(aktuelles);
                Save_Aktuelles(aktuelles);
            }
        }

        /////////////Veranstaltung////////////////////////////////////////////////

        public void Get_Veranstaltung()
        {
            Veranstaltung[] veranstaltung = Save.LoadVeranstaltung(ip);
            int count = Save.LoadCountVeranstaltung(ip);

            if (veranstaltung != null)
                SetVeranstaltung(veranstaltung, count);

        }

        private void SetVeranstaltung(Veranstaltung[] veranstaltung, int count)
        {
            int anzahl = veranstaltung.Length;


            if (anzahl > 0)
            {
                NumberOfEvents.SelectedIndex = count - 1;

                EventTitle1.Text = veranstaltung[0].Title;

                foreach (ComboBoxItem items in EventFrom1.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[0].From && item != -1)
                    {
                        int thisIndex = EventFrom1.Items.IndexOf(items);
                        EventFrom1.SelectedIndex = thisIndex;
                        break;
                    }

                }

                foreach (ComboBoxItem items in ScheduleTo.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[0].To && item != -1)
                    {
                        int thisIndex = ScheduleTo.Items.IndexOf(items);
                        ScheduleTo.SelectedIndex = thisIndex;
                        break;
                    }
                }

                foreach (ComboBoxItem items in EventDay1.Items)
                {
                    string item = items.Content as string;
                    if (item.Equals(veranstaltung[0].Day) && item != "")
                    {
                        int thisIndex = EventDay1.Items.IndexOf(items);
                        EventDay1.SelectedIndex = thisIndex;
                        break;
                    }
                }
            }

            if (anzahl > 1)
            {
                EventTitle2.Text = veranstaltung[1].Title;

                foreach (ComboBoxItem items in EventFrom2.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[1].From && item != -1)
                    {
                        int thisIndex = EventFrom2.Items.IndexOf(items);
                        EventFrom2.SelectedIndex = thisIndex;
                        break;
                    }
                }

                foreach (ComboBoxItem items in EventTo2.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[1].To && item != -1)
                    {
                        int thisIndex = EventTo2.Items.IndexOf(items);
                        EventTo2.SelectedIndex = thisIndex;
                        break;
                    }
                }

                foreach (ComboBoxItem items in EventDay2.Items)
                {
                    string item = items.Content as string;
                    if (item.Equals(veranstaltung[1].Day) && item != "")
                    {
                        int thisIndex = EventDay2.Items.IndexOf(items);
                        EventDay2.SelectedIndex = thisIndex;
                        break;
                    }
                }
            }

            if (anzahl > 2)
            {
                EventTitle3.Text = veranstaltung[2].Title;

                foreach (ComboBoxItem items in EventFrom3.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[2].From && item != -1)
                    {
                        int thisIndex = EventFrom3.Items.IndexOf(items);
                        EventFrom3.SelectedIndex = thisIndex;
                        break;
                    }
                }

                foreach (ComboBoxItem items in EventTo3.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[2].To && item != -1)
                    {
                        int thisIndex = EventTo3.Items.IndexOf(items);
                        EventTo3.SelectedIndex = thisIndex;
                        break;
                    }
                }

                foreach (ComboBoxItem items in EventDay3.Items)
                {
                    string item = items.Content as string;
                    if (item.Equals(veranstaltung[2].Day) && item != "")
                    {
                        int thisIndex = EventDay3.Items.IndexOf(items);
                        EventDay3.SelectedIndex = thisIndex;
                        break;
                    }
                }
            }

            if (anzahl > 3)
            {
                EventTitle4.Text = veranstaltung[3].Title;

                foreach (ComboBoxItem items in EventFrom4.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[3].From && item != -1)
                    {
                        int thisIndex = EventFrom4.Items.IndexOf(items);
                        EventFrom4.SelectedIndex = thisIndex;
                        break;
                    }
                }

                foreach (ComboBoxItem items in EventTo4.Items)
                {
                    int? item = Int32.Parse(items.Content.ToString());
                    if (item == veranstaltung[3].To && item != -1)
                    {
                        int thisIndex = EventTo4.Items.IndexOf(items);
                        EventTo4.SelectedIndex = thisIndex;
                        break;
                    }
                }

                foreach (ComboBoxItem items in EventDay4.Items)
                {
                    string item = items.Content as string;
                    if (item.Equals(veranstaltung[3].Day) && item != "")
                    {
                        int thisIndex = EventDay4.Items.IndexOf(items);
                        EventDay4.SelectedIndex = thisIndex;
                        break;
                    }
                }
            }

        }

        private bool VeranstaltungIsChanged()
        {
            String[] EventTitles = { EventTitle1.Text, EventTitle2.Text, EventTitle3.Text, EventTitle4.Text };

            ComboBoxItem countC = (ComboBoxItem)NumberOfEvents.SelectedItem;
            int count = Int32.Parse((string)countC.Content);

            ComboBoxItem[] EventFromC = { (ComboBoxItem)EventFrom1.SelectedItem, (ComboBoxItem)EventFrom2.SelectedItem, (ComboBoxItem)EventFrom3.SelectedItem, (ComboBoxItem)EventFrom4.SelectedItem };
            ComboBoxItem[] EventToC = { (ComboBoxItem)ScheduleTo.SelectedItem, (ComboBoxItem)EventTo2.SelectedItem, (ComboBoxItem)EventTo3.SelectedItem, (ComboBoxItem)EventTo4.SelectedItem };
            ComboBoxItem[] EventDayC = { (ComboBoxItem)EventDay1.SelectedItem, (ComboBoxItem)EventDay1.SelectedItem, (ComboBoxItem)EventDay1.SelectedItem, (ComboBoxItem)EventDay1.SelectedItem };

            int?[] EventFrom = new int?[4];

            for (int i = 0; i < EventFromC.Length; i++)
            {
                if (EventFromC[i] != null)
                {
                    EventFrom[i] = Int32.Parse(EventFromC[i].Content.ToString());
                }
                else
                    EventFrom[i] = null;
            }

            int?[] EventTo = new int?[4];

            for (int i = 0; i < EventToC.Length; i++)
            {
                if (EventToC[i] != null)
                {
                    EventTo[i] = Int32.Parse(EventToC[i].Content.ToString());
                }
                else
                    EventTo[i] = null;
            }

            string[] EventDay = new string[4];

            for (int i = 0; i < EventDayC.Length; i++)
            {
                if (EventDayC[i] != null)
                {
                    EventDay[i] = EventDayC[i].Content.ToString();
                }
                else
                    EventDay[i] = "";
            }



            Veranstaltung[] veranstaltung = Save.LoadVeranstaltung(ip);


            if (veranstaltung == null)
            {
                veranstaltung = new Veranstaltung[0];
            }


            if (count != veranstaltung.Length)
                return true;

            if (veranstaltung != null)
            {
                for (int i = 0; i < veranstaltung.Length; i++)
                {
                    if ((!veranstaltung[i].Title.Equals(EventTitles[i]) && !veranstaltung[i].Title.Equals("")) || veranstaltung[i].From != EventFrom[i] || veranstaltung[i].To != EventTo[i] || !veranstaltung[i].Day.Equals(EventDay[i]))
                    {
                        return true;
                    }
                }
                return false;

            }
            else
            {
                for (int i = 0; i < EventTitles.Length; i++)
                {
                    if (EventTitles[i] != "" || EventFrom[i] != null || EventTo[i] != null || EventDay[i] != "")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void Load_Veranstaltung()
        {
            Veranstaltung[] veranstaltung = Save.LoadVeranstaltung(ip);
            int count = Save.LoadCountVeranstaltung(ip);

            if (veranstaltung != null)
                SetVeranstaltung(veranstaltung, count);
        }

        private void Save_Veranstaltung(Veranstaltung[] veranstaltung)
        {
            Save.SaveVeranstaltung(ip, veranstaltung);
        }

        private void Save_CountVeranstaltung(int count)
        {
            Save.SaveCountVeranstaltung(ip, count);
        }

        private async Task Send_Veranstaltung()
        {
            ComboBoxItem countC = (ComboBoxItem)NumberOfEvents.SelectedItem;
            int count = Int32.Parse((string)countC.Content);

            ComboBoxItem[] EventFromC = { (ComboBoxItem)EventFrom1.SelectedItem, (ComboBoxItem)EventFrom2.SelectedItem, (ComboBoxItem)EventFrom3.SelectedItem, (ComboBoxItem)EventFrom4.SelectedItem };
            ComboBoxItem[] EventToC = { (ComboBoxItem)ScheduleTo.SelectedItem, (ComboBoxItem)EventTo2.SelectedItem, (ComboBoxItem)EventTo3.SelectedItem, (ComboBoxItem)EventTo4.SelectedItem };
            ComboBoxItem[] EventDayC = { (ComboBoxItem)EventDay1.SelectedItem, (ComboBoxItem)EventDay1.SelectedItem, (ComboBoxItem)EventDay1.SelectedItem, (ComboBoxItem)EventDay1.SelectedItem };



            Veranstaltung veranstaltung1 = new Veranstaltung
            {
                Title = EventTitle1.Text,
                Day = EventDayC[0].Content.ToString()
            };
            if (EventFrom1.SelectedItem != null)
                veranstaltung1.From = Int32.Parse(EventFromC[0].Content.ToString());
            else
                veranstaltung1.From = -1;

            if (ScheduleTo.SelectedItem != null)
                veranstaltung1.To = Int32.Parse(EventToC[0].Content.ToString());
            else
                veranstaltung1.To = -1;



            Veranstaltung veranstaltung2 = new Veranstaltung
            {
                Title = EventTitle2.Text,
                Day = (string)EventDayC[1].Content.ToString()
            };
            if (EventFrom2.SelectedItem != null)
                veranstaltung2.From = Int32.Parse(EventFromC[1].Content.ToString());
            else
                veranstaltung2.From = -1;

            if (EventTo2.SelectedItem != null)
                veranstaltung2.To = Int32.Parse(EventToC[1].Content.ToString());
            else
                veranstaltung2.To = -1;

            Veranstaltung veranstaltung3 = new Veranstaltung
            {
                Title = EventTitle3.Text,
                Day = EventDayC[2].Content.ToString()
            };
            if (EventFrom3.SelectedItem != null)
                veranstaltung3.From = Int32.Parse(EventFromC[2].Content.ToString());
            else
                veranstaltung3.From = -1;

            if (EventTo3.SelectedItem != null)
                veranstaltung3.To = Int32.Parse(EventToC[2].Content.ToString());
            else
                veranstaltung3.To = -1;

            Veranstaltung veranstaltung4 = new Veranstaltung
            {
                Title = EventTitle4.Text,
                Day = EventDayC[3].Content.ToString()
            };
            if (EventFrom4.SelectedItem != null)
                veranstaltung4.From = Int32.Parse(EventFromC[3].Content.ToString());
            else
                veranstaltung4.From = -1;

            if (EventTo4.SelectedItem != null)
                veranstaltung4.To = Int32.Parse(EventToC[3].Content.ToString());
            else
                veranstaltung4.To = -1;

            Veranstaltung[] save_veranstaltung = { veranstaltung1, veranstaltung2, veranstaltung3, veranstaltung4 };
            Veranstaltung[] send_veranstaltung = new Veranstaltung[count];

            for (int i = 0; i < count; i++)
            {
                send_veranstaltung[i] = save_veranstaltung[i];
            }

            bool send = await VeranstaltungsController.SetVeranstaltung(ip, send_veranstaltung);

            if (send)
            {
                SetVeranstaltung(save_veranstaltung, count);
                Save_CountVeranstaltung(count);
                Save_Veranstaltung(save_veranstaltung);
            }
        }

        /////////////Belegungsplan////////////////////////////////////////////////

        public void Get_BelegungsPlan()
        {
            BelegungsPlan belegungsPlan = Save.LoadSchedule(ip);

            if (belegungsPlan != null)
                SetSchedule(belegungsPlan);
        }

        private void SetSchedule(BelegungsPlan belegungsPlan)
        {
            ModifyRowSchedule(0, belegungsPlan.Montag, 0, false);
            ModifyRowSchedule(1, belegungsPlan.Dienstag, 0, false);
            ModifyRowSchedule(2, belegungsPlan.Mittwoch, 0, false);
            ModifyRowSchedule(3, belegungsPlan.Donnerstag, 0, false);
            ModifyRowSchedule(4, belegungsPlan.Freitag, 0, false);
        }

        private bool ScheduleIsChanged()
        {
            bool changed = false;
            BelegungsPlan belegungsPlan = Save.LoadSchedule(ip);


            if (belegungsPlan != null)
            {
                return belegungsPlan.Montag.SequenceEqual(MainBelegungsPlan.Montag)
                 || belegungsPlan.Dienstag.SequenceEqual(MainBelegungsPlan.Dienstag)
                 || belegungsPlan.Mittwoch.SequenceEqual(MainBelegungsPlan.Mittwoch)
                 || belegungsPlan.Donnerstag.SequenceEqual(MainBelegungsPlan.Donnerstag)
                 || belegungsPlan.Freitag.SequenceEqual(MainBelegungsPlan.Freitag);
            }
            else
            {
                string[][] strings = { MainBelegungsPlan.Montag, MainBelegungsPlan.Dienstag, MainBelegungsPlan.Mittwoch,
                                       MainBelegungsPlan.Donnerstag, MainBelegungsPlan.Freitag };

                for (int i = 0; i < strings.GetLength(0); i++)
                {
                    changed = Check_StringArray(strings[i]);
                    if (changed)
                        return true;
                }
                return false;
            }
        }

        private bool Check_StringArray(string[] array)
        {
            foreach (string elem in array)
            {
                if (elem != "")
                    return true;
            }
            return false;
        }

        private void Load_Schedule()
        {
            DeleteAllEvents();
            BelegungsPlan belegungsPlan = Save.LoadSchedule(ip);

            if (belegungsPlan != null)
            {
                MainBelegungsPlan = Save.LoadSchedule(ip);
                SetSchedule(belegungsPlan);
            }
        }

        private void Save_Schedule(BelegungsPlan belegungsPlan)
        {
            Save.SaveSchedule(ip, belegungsPlan);
        }

        private async Task Send_Schedule()
        {
            BelegungsPlan belegungsPlan = MainBelegungsPlan;


            bool send = await BelegungsPlanController.SetSchedule(ip, belegungsPlan);

            if (send)
            {
                SetSchedule(belegungsPlan);
                Save_Schedule(belegungsPlan);
            }
        }

        private BelegungsPlan AddEvent()
        {
            ComboBoxItem ScheduleFromC = (ComboBoxItem)ScheduleFrom.SelectedValue;
            ComboBoxItem ScheduleToC = (ComboBoxItem)ScheduleTo.SelectedValue;
            int ScheduleToInt = Int32.Parse(ScheduleToC.Content.ToString());
            BelegungsPlan belegungsPlan = MainBelegungsPlan;

            int ScheduleFromIndex = ScheduleFrom.SelectedIndex;
            int ScheduleFromInt = Int32.Parse(ScheduleFromC.Content.ToString());
            int ScheduleDayC = ScheduleDay.SelectedIndex;


            int difference = ScheduleToInt - ScheduleFromInt - 1;


            switch (ScheduleDayC)
            {
                case 0:
                    for (int i = 0; i <= difference; i++)
                    {
                        belegungsPlan.Montag[ScheduleFromIndex + i] = ScheduleName.Text;
                    }
                    ModifyRowSchedule(ScheduleDayC, belegungsPlan.Montag, ScheduleFromIndex);
                    break;
                case 1:
                    for (int i = 0; i <= difference; i++)
                    {
                        belegungsPlan.Dienstag[ScheduleFromIndex + i] = ScheduleName.Text;
                    }
                    ModifyRowSchedule(ScheduleDayC, belegungsPlan.Dienstag, ScheduleFromIndex);
                    break;
                case 2:
                    for (int i = 0; i <= difference; i++)
                    {
                        belegungsPlan.Mittwoch[ScheduleFromIndex + i] = ScheduleName.Text;
                    }
                    ModifyRowSchedule(ScheduleDayC, belegungsPlan.Mittwoch, ScheduleFromIndex);
                    break;
                case 3:
                    for (int i = 0; i <= difference; i++)
                    {
                        belegungsPlan.Donnerstag[ScheduleFromIndex + i] = ScheduleName.Text;
                    }
                    ModifyRowSchedule(ScheduleDayC, belegungsPlan.Donnerstag, ScheduleFromIndex);
                    break;
                case 4:
                    for (int i = 0; i <= difference; i++)
                    {
                        belegungsPlan.Freitag[ScheduleFromIndex + i] = ScheduleName.Text;
                    }
                    ModifyRowSchedule(ScheduleDayC, belegungsPlan.Freitag, ScheduleFromIndex);
                    break;


            }
            MainBelegungsPlan = belegungsPlan;
            Validation(ScheduleFromInt, ScheduleToInt);

            return belegungsPlan;
        }

        private void Validation(int ScheduleFromInt, int ScheduleToInt)
        {
            int day = ScheduleDay.SelectedIndex;
            ComboBoxItem valueSelectedC = (ComboBoxItem)ScheduleFrom.SelectedValue;
            int valueSelected = Int32.Parse(valueSelectedC.Content.ToString());

            int indexScheduleFrom = ScheduleFrom.SelectedIndex;
            int difference = ScheduleToInt - ScheduleFromInt - 1;

            for (int i = 0; i <= difference; i++)
            {
                ComboBoxItem scheduleItem = (ComboBoxItem)ScheduleFrom.Items[indexScheduleFrom + i];
                ValuedItemsArray[day].Add(Int32.Parse(scheduleItem.Content.ToString()));
                DependencyObject schedule = scheduleItem;
                schedule.SetCurrentValue(IsEnabledProperty, false);
            }

            foreach (ComboBoxItem item in ScheduleFrom.Items)
            {
                if ((bool)item.GetValue(IsEnabledProperty) == true)
                {
                    ScheduleFrom.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in ScheduleTo.Items)
            {
                if ((bool)item.GetValue(IsEnabledProperty) == true)
                {
                    ScheduleTo.SelectedItem = item;
                    break;
                }
            }


        }

        private void ModifyRowSchedule(int day, String[] array, int indexbegin = 0, bool onlyone = true)
        {
            RowDefinitionCollection rows = MainSchedule.RowDefinitions;
            ColumnDefinitionCollection cols = MainSchedule.ColumnDefinitions;

            bool begin = false;
            int beginEdit = -1;
            int endEdit = -1;

            string sameString = "";
            for (int i = indexbegin; i < rows.Count; i++)
            {
                if (array.Length > i && (array[i] != sameString && !begin))
                {

                    sameString = array[i];
                    endEdit = beginEdit = i;
                    begin = true;

                }
                else if ((array.Length > i && (array[i].Equals(sameString) && i != rows.Count - 1)))
                {
                    endEdit++;
                }

                else if (begin)
                {

                    sameString = "";
                    begin = !begin;


                    i--;
                    MainSchedule = ChangeRows(day, beginEdit, endEdit, MainSchedule, array[i]);
                    if (onlyone)
                        break;
                }
            }
        }

        private Grid ChangeRows(int day, int beginEdit, int endEdit, Grid mainSchedule, String text)
        {
            for (int i = beginEdit; i < endEdit; i++)
            {
                mainSchedule.RowDefinitions.RemoveAt(i);
                RowDefinition row = new RowDefinition();
                mainSchedule.RowDefinitions.Insert(i, row);

            }



            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.LightGray),
                BorderThickness = new Thickness(2),
                Background = new SolidColorBrush(Colors.LightBlue),
                IsEnabled = true,

                Name = "Border" + "_" + day + "_" + beginEdit + "_" + endEdit
            };


            TextBlock textblock = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };

            border.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => BelegungsPlanCell_OnFocus(sender, e, textblock));


            Grid.SetColumn(border, day + 1);
            Grid.SetRow(border, beginEdit + 1);
            Grid.SetRowSpan(border, (endEdit - beginEdit) + 1);

            Grid.SetColumn(textblock, day + 1);
            Grid.SetRow(textblock, beginEdit + 1);
            Grid.SetRowSpan(textblock, (endEdit - beginEdit) + 1);

            allBorders.Add(border);
            allTextBlocks.Add(textblock);

            mainSchedule.Children.Add(border);
            mainSchedule.Children.Add(textblock);

            return mainSchedule;
        }

        private void DeleteEvent()
        {
            while (helpBorders.Count != 0)
            {
                Border border = helpBorders[0];
                String[] data = border.Name.Split(new char[] { '_' });

                int day = Int32.Parse(data[1]);
                int beginEdit = Int32.Parse(data[2]);
                int endEdit = Int32.Parse(data[3]);

                switch (day)
                {
                    case 0:
                        for (int i = beginEdit; i <= endEdit; i++)
                        {

                            MainBelegungsPlan.Montag[i] = "";
                        }

                        break;
                    case 1:
                        for (int i = beginEdit; i <= endEdit; i++)
                        {
                            MainBelegungsPlan.Dienstag[i] = "";
                        }

                        break;
                    case 2:
                        for (int i = beginEdit; i <= endEdit; i++)
                        {
                            MainBelegungsPlan.Mittwoch[i] = "";
                        }

                        break;
                    case 3:
                        for (int i = beginEdit; i <= endEdit; i++)
                        {
                            MainBelegungsPlan.Donnerstag[i] = "";
                        }

                        break;
                    case 4:
                        for (int i = beginEdit; i <= endEdit; i++)
                        {
                            MainBelegungsPlan.Freitag[i] = "";
                        }
                        break;
                }
                helpBorders.RemoveAt(0);

                allBorders.Remove(border);

                MainSchedule.Children.Remove(border);
            }

            while (helpTextBlocks.Count != 0)
            {
                MainSchedule.Children.Remove(helpTextBlocks[0]);

                allTextBlocks.Remove(helpTextBlocks[0]);
                helpTextBlocks.RemoveAt(0);
            }



        }

        private void DeleteAllEvents()
        {
            MainBelegungsPlan = new BelegungsPlan
            {
                Montag = (String[])helper.Clone(),
                Dienstag = (String[])helper.Clone(),
                Mittwoch = (String[])helper.Clone(),
                Donnerstag = (String[])helper.Clone(),
                Freitag = (String[])helper.Clone()
            };

            foreach (Border b in allBorders)
            {
                MainSchedule.Children.Remove(b);
            }
            allBorders = new List<Border>();

            foreach (TextBlock t in allTextBlocks)
            {
                MainSchedule.Children.Remove(t);
            }
            allTextBlocks = new List<TextBlock>();
        }

        private void BelegungsPlanCell_FocusLost(object sender, MouseButtonEventArgs e, TextBlock textBlock)
        {
            Border border = sender as Border;
            border.Background = new SolidColorBrush(Colors.LightBlue);

            helpBorders.Remove(border);
            helpTextBlocks.Remove(textBlock);

            border.MouseLeftButtonDown -= new MouseButtonEventHandler((sender2, e2) => BelegungsPlanCell_FocusLost(sender2, e2, textBlock));
            border.MouseLeftButtonDown += new MouseButtonEventHandler((sender2, e2) => BelegungsPlanCell_OnFocus(sender2, e2, textBlock));
        }

        private void BelegungsPlanCell_OnFocus(object sender, MouseButtonEventArgs e, TextBlock textBlock)
        {
            Border border = sender as Border;
            border.Background = new SolidColorBrush(Colors.Yellow);

            if(!helpBorders.Contains(border))
                helpBorders.Add(border);

            if(!helpTextBlocks.Contains(textBlock))
                helpTextBlocks.Add(textBlock);

            border.MouseLeftButtonDown -= new MouseButtonEventHandler((sender2, e2) => BelegungsPlanCell_OnFocus(sender2, e2, textBlock));
            border.MouseLeftButtonDown += new MouseButtonEventHandler((sender2, e2) => BelegungsPlanCell_FocusLost(sender2, e2, textBlock));
        }



        private void Schedule_ReValidation()
        {
            int day = ScheduleDay.SelectedIndex;
            foreach (Border border in helpBorders)
            {
                String[] data = border.Name.Split(new char[] { '_' });

                int beginEdit = Int32.Parse(data[2]);
                int endEdit = Int32.Parse(data[3]);
                bool change = true;
                int difference = endEdit - beginEdit + 1;

                
                ComboBoxItem indexFromC = (ComboBoxItem)ScheduleFrom.Items.GetItemAt(endEdit);
                ComboBoxItem beginIntC = (ComboBoxItem)ScheduleFrom.Items.GetItemAt(beginEdit);

                int beginInt = Int32.Parse(beginIntC.Content.ToString());
                int indexValue = Int32.Parse(indexFromC.Content.ToString()) + 1;

                int i = 0;
                foreach (ComboBoxItem item in ScheduleFrom.Items)
                {

                    int value = Int32.Parse(item.Content.ToString());

                    if (indexValue == value - 1)
                    {
                        change = false;

                    }

                    if (change && difference > 0  && (beginInt <= value && value <= indexValue))
                    {
                        DependencyObject scheduleFrom = (ComboBoxItem)ScheduleFrom.Items.GetItemAt(beginEdit + i);
                        DependencyObject scheduleTo = item;

                        scheduleFrom.SetCurrentValue(IsEnabledProperty, true);
                        scheduleTo.SetCurrentValue(IsEnabledProperty, true);
                        ValuedItemsArray[day].Remove(value);

                        i++;
                        difference--;
                    }

                }



            }

            foreach (ComboBoxItem item in ScheduleFrom.Items)
            {
                int value = Int32.Parse(item.Content.ToString());
                if (!ValuedItemsArray[day].Contains(value))
                {
                    DependencyObject scheduleFrom = item;
                    scheduleFrom.SetCurrentValue(IsEnabledProperty, true);
                }
                else
                {
                    DependencyObject scheduleFrom = item;
                    scheduleFrom.SetCurrentValue(IsEnabledProperty, false);
                }

            }

            bool changed = false;
            foreach (ComboBoxItem item in ScheduleTo.Items)
            {
                int value = Int32.Parse(item.Content.ToString());
                if (!ValuedItemsArray[day].Contains(value) && !changed)
                {
                    DependencyObject scheduleTo = item;
                    scheduleTo.SetCurrentValue(IsEnabledProperty, true);
                }
                else
                {
                    changed = !changed;
                    DependencyObject scheduleTo = item;
                    scheduleTo.SetCurrentValue(IsEnabledProperty, false);
                }

            }
        }


        /////////////Wetter///////////////////////////////////////////////////////

        public void Get_OpenWeather()
        {
            OpenWeather openWeather = Save.LoadOpenWeather(ip);

            if (openWeather != null)
                SetOpenWeather(openWeather);

        }

        private void SetOpenWeather(OpenWeather openWeather)
        {
            OpenWeatherAPI.Text = openWeather.API;
        }

        private bool OpenWeatherIsChanged()
        {
            OpenWeather openWeather = Save.LoadOpenWeather(ip);

            if (openWeather != null)
            {
                if (!openWeather.API.Equals(OpenWeatherAPI.Text))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (!(OpenWeatherAPI.Text == ""))
                {
                    return true;
                }
                return false;
            }
        }

        private void Load_OpenWeather()
        {
            OpenWeather openWeather = Save.LoadOpenWeather(ip);

            if (openWeather != null)
                SetOpenWeather(openWeather);
        }

        private void Save_OpenWeather(OpenWeather openWeather)
        {
            Save.SaveOpenWeather(ip, openWeather);
        }

        private async Task Send_OpenWeather()
        {
            OpenWeather openWeather = new OpenWeather
            {
                API = OpenWeatherAPI.Text
            };

            bool send = await WetterController.SetOpenWeather(ip, openWeather);

            if (send)
            {
                SetOpenWeather(openWeather);
                Save_OpenWeather(openWeather);
            }
        }


        /////////////////////////Tabs///////////////////////////

        public void Get_Tabs()
        {
            Tabs tabs = Save.LoadTabs(ip);

            if (tabs != null)
                SetTabs(tabs);

        }

        private void SetTabs(Tabs tabs)
        {
            

            ActualTab.IsChecked = tabs.ActiveTabs[1];
            if (tabs.ActiveTabs[1])
                ActualTab_Activate(null, null);
            else
                ActualTab_Deactivate(null, null);

            EventTab.IsChecked = tabs.ActiveTabs[2];
            if (tabs.ActiveTabs[2])
                EventTab_Activate(null, null);
            else
                EventTab_Deactivate(null, null);

            ScheduleTab.IsChecked = tabs.ActiveTabs[3];
            if (tabs.ActiveTabs[3])
                ScheduleTab_Activate(null, null);
            else
                ScheduleTab_Deactivate(null, null);

            WeatherTab.IsChecked = tabs.ActiveTabs[4];
            if (tabs.ActiveTabs[4])
                WeatherTab_Activate(null, null);
            else
                WeatherTab_Deactivate(null, null);

            ProfilTab.IsChecked = tabs.ActiveTabs[0];
            if (tabs.ActiveTabs[0])
                ProfilTab_Activate(null, null);
            else
                ProfilTab_Deactivate(null, null);
        }

        private bool TabsIsChanged()
        {
            Tabs tabs = Save.LoadTabs(ip);

            if (tabs != null)
            {
                if (ProfilTab.IsChecked == tabs.ActiveTabs[0] || ActualTab.IsChecked == tabs.ActiveTabs[1] || EventTab.IsChecked == tabs.ActiveTabs[2]
                    || ScheduleTab.IsChecked == tabs.ActiveTabs[3] || WeatherTab.IsChecked == tabs.ActiveTabs[4])
                {
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Load_Tabs()
        {
            Tabs tabs = Save.LoadTabs(ip);

            if (tabs != null)
                SetTabs(tabs);
        }

        private void Save_Tabs(Tabs tabs)
        {
            Save.SaveTabs(ip, tabs);
        }

        private async Task<bool> Send_Tabs()
        {
            bool check1 = ProfilTab.IsChecked ?? false;
            bool check2 = ActualTab.IsChecked ?? false;
            bool check3 = EventTab.IsChecked ?? false;
            bool check4 = ScheduleTab.IsChecked ?? false;
            bool check5 = WeatherTab.IsChecked ?? false;

            if (check1 == false && check2 == false && check3 == false && check4 == false && check5 == false)
            {
                return false;
            }

            bool[] checkedTabs = { check1, check2, check3, check4, check5 };

            Tabs tabs = new Tabs
            {
                ActiveTabs = checkedTabs
            };

            bool send = await TabController.SetTabs(ip, tabs);

            if (send)
            {
                SetTabs(tabs);
                Save_Tabs(tabs);

            }
            return send;
        }

        /////Profil System/////

        private void NewProfil_Adder(string profil)
        {
            ComboBoxItem newProfil = new ComboBoxItem
            {
                Content = profil
            };
            System.Windows.Controls.ComboBox box = LoadProfil;
            box.Items.Insert(1, newProfil);
            box.SelectedItem = newProfil;
        }

        private void Load_ProfilSystem()
        {
            string[] profils = Save.LoadProfilSystem(ip);

            if (profils != null)
            {
                foreach (string profil in profils)
                {
                    if (!profil.Equals("Default") && !profil.Equals("Neues Profil"))
                        NewProfil_Adder(profil);
                }
                LoadProfil.SelectedIndex = 0;
            }
        }

        private void Save_ProfilSystem()
        {
            List<string> data = new List<string>();
            System.Windows.Controls.ComboBox box = LoadProfil;
            foreach (ComboBoxItem item in box.Items)
            {
                data.Add(item.Content.ToString());
            }
            string[] arrayData = new string[data.Count];
            arrayData = data.ToArray<string>();

            Save.SaveProfilSystem(ip, arrayData);
        }


        /////Helper/////

        public static IEnumerable<DependencyObject> SetEnability(DependencyObject parent, bool activate, int index = 0, bool recurse = true)
        {
            if (parent != null)
            {
                parent.SetCurrentValue(IsEnabledProperty, activate);
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = index; i < count; i++)
                {

                    if (VisualTreeHelper.GetChild(parent, i) is DependencyObject child2)
                    {
                        yield return child2;
                        if (recurse)
                        {
                            foreach (var grandChild in SetEnability(child2, activate))
                            {
                                yield return grandChild;
                            }
                        }
                    }

                }
            }
        }

        public static List<int> Clone(List<int> source)
        {
            return new List<int>(source);
        }

    }

}
