

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using Foreground.Controller;
using Foreground.Model;
using Windows.Devices.Geolocation;
using Foreground.Extras;
using Foreground.Service;
using Windows.ApplicationModel.Core;

namespace HelloWorld
{
    /// <summary>
    /// Namensschild Kontaktseite
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public readonly MessageRelayService _connection = MessageRelayService.Instance;
        DispatcherTimer Timer = new DispatcherTimer();
        static readonly object locker = new object();
        static int NavPosition = 0;
        static Border lastMarked;
        static Tabs tabs;

        public MainPage()
        {
            InitializeComponent();
            OpenNavPage(0);
            DataContext = this;

            //Bind Method to Eventhandler
            Timer.Tick += Timer_Tick;

            //Calculate interval
            double IntervalinSec = 10;
            int FinalInterval = (int)(IntervalinSec * 1000) / (int)progressBar1.Maximum;

            //Set interval
            Timer.Interval = new TimeSpan(0, 0, 0, 0, FinalInterval);
            Timer.Start();

            Loaded += OnLoaded;
            _connection.OnMessageReceived += ConnectionOnMessageReceived;

            InitTabs();
            InitInfoLeiste();
            InitHardware();

        }

        private async void InitTabs()
        {
            await CreateTab();

            if (tabs.ActiveTabs[0] == false)
            {
                ProfilBorder.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            }

            if (tabs.ActiveTabs[1] == false)
            {
                AktuellesBorder.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            }

            if (tabs.ActiveTabs[2] == false)
            {
                VeranstaltungBorder.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            }

            if (tabs.ActiveTabs[3] == false)
            {
                BelegungsplanBorder.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            }

            if (tabs.ActiveTabs[4] == false)
            {
                WetterBorder.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            }

        }

        private async void InitInfoLeiste()
        {
            try
            {
                Infoleiste infoleiste = await SaveController.ReadFromJsonFile<Infoleiste>("Infoleiste.txt");
                SetInfoLeiste(infoleiste);
            }
            catch
            {

            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            await MessageController.EnsureConnected(_connection);
        }

        private void ConnectionOnMessageReceived(ValueSet valueSet)
        {
            
                    lock (locker)
                    {
                        string message = valueSet.First().Value.ToString();
                        string key = valueSet.First().Key;



                        switch (key)
                        {
                            case "Aktuelles":
                                Aktuelles aktuelles = AktuellesController.CreateModelFromJson((string)message);
                                SaveController.WriteToJsonFile("Aktuelles.txt", aktuelles);
                                return;
                            case "Profil":
                                Profil[] profil = ProfilController.CreateModelFromJson((string)message);
                                SaveController.WriteProfilArrayToJsonFile("Profil.txt", profil);
                                return;
                            case "BelegungsPlan":
                                BelegungsPlan belegungsPlan = BelegungsPlanController.CreateModelFromJson((string)message);
                                SaveController.WriteToJsonFile("Belegungsplan.txt", belegungsPlan);
                                return;
                            case "InfoLeiste":
                                Infoleiste infoleiste = InfoleistenController.CreateModelFromJson((string)message);
                                SaveController.WriteToJsonFile("Infoleiste.txt", infoleiste);
                                SetInfoLeiste(infoleiste);
                                return;
                            case "Tabs":
                                Tabs tabs = TabsController.CreateModelFromJson((string)message);
                                SaveController.WriteToJsonFile("Tabs.txt", tabs);                                
                                return;
                            case "Veranstaltung":
                                Veranstaltung[] veranstaltung = VeranstaltungController.CreateModelFromJson((string)message);
                                SaveController.WriteEventsArrayToJsonFile("Veranstaltung.txt", veranstaltung);
                                return;
                            case "Weather":
                                OpenWeather weather = WetterController.CreateModelFromJson((string)message);
                                SaveController.WriteToJsonFile("OpenWeather.txt", weather);
                                return;
                            default:
                                return;
                        }
                    }
  
            
        }

        private async void SetInfoLeiste(Infoleiste infoleiste)
        {
            Room.Text = infoleiste.Room;
            Title.Text = infoleiste.Title;
            Chair.Text = infoleiste.Chair;

            if (infoleiste.LeftImage != null)
            {
                LeftImage.Source = await ByteArrayBitmapService.AsBitmapImage(infoleiste.LeftImage);
            }

            if (infoleiste.RightImage != null)
            {
                RightImage.Source = await ByteArrayBitmapService.AsBitmapImage(infoleiste.RightImage);
            }

        }

        /* Initialize GPIO, SPI, and the display */
        private async void InitHardware()
        {
            await PiFaceSpiDriver.InitSPI();
            PiFaceSpiDriver.InitMCP23S17();

            PiFaceSpiDriver.WritePin(PFDII.LED0, PiFaceSpiDriver.On);
            PiFaceSpiDriver.WritePin(PFDII.LED3, PiFaceSpiDriver.On);
        }

        //Timer
        private void Timer_Tick(object sender, object e)
        {
            //24 hour time
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            //If progressbar not full, fill it
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value += 1;
            }

            //If progressbar full, go to next navigationtab
            else if (NavPosition < 4)
            {
                progressBar1.Value = 0;
                NavPosition += 1;
                OpenNavPage(NavPosition);
            }

            //If end of navigation, go to start
            else
            {
                progressBar1.Value = 0;
                NavPosition = 0;
                OpenNavPage(NavPosition);
            }
        }



        //Navigation beginn
        private void Navigation_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Grid nav)
            {

                Point p = e.GetCurrentPoint(sender as Grid).Position;

                int row = 0;
                int col = 0;
                double accumulatedHeight = 0.0;
                double accumulatedWidth = 0.0;

                // Berechne geklickte Reihe
                foreach (var rowDefinition in nav.RowDefinitions)
                {
                    accumulatedHeight += rowDefinition.ActualHeight;
                    if (accumulatedHeight >= p.Y)
                        break;
                    row++;
                }

                // Berechne geklickte Spalte
                foreach (var columnDefinition in nav.ColumnDefinitions)
                {
                    accumulatedWidth += columnDefinition.ActualWidth;
                    if (accumulatedWidth >= p.X)
                        break;
                    col++;
                }



                if (tabs.ActiveTabs[row])
                    OpenNavPage(row);
            }
        }

        private async Task<Tabs> CreateTab()
        {
            try
            {
                tabs = await SaveController.ReadFromJsonFile<Tabs>("Tabs.txt");


            }
            catch
            {
                if (tabs == null)
                {
                    Boolean[] active = { true, true, true, true, true };
                    tabs = new Tabs
                    {
                        ActiveTabs = active
                    };
                }
            }

            return tabs;
        }
        private async void OpenNavPage(int row)
        {
            ContentPage.Margin = new Thickness(0, 0, 0, 0);
            ContentPage.Children.Clear();

            await CreateTab();

            switch (row)
            {
                case 0:
                    // Fuege Tabelle in das Content Layout hinzu
                    if (tabs.ActiveTabs[row] != false)
                    {
                        ContentPage.Children.Add(await ProfilController.AddAllgemein());
                        NavPosition = row;
                        MarkNavBar(0, row);
                    }
                    else
                        OpenNavPage(row + 1);
                    break;

                case 1:
                    if (tabs.ActiveTabs[row] != false)
                    {
                        ContentPage.Children.Add(await AktuellesController.AddAktuellesLayout());
                        NavPosition = row;
                        MarkNavBar(0, row);
                    }
                    else
                        OpenNavPage(row + 1);
                    break;

                case 2:
                    if (tabs.ActiveTabs[row] != false)
                    {
                        ContentPage.Children.Add(await VeranstaltungController.AddVeranstaltung());
                        NavPosition = row;
                        MarkNavBar(0, row);
                    }
                    else
                        OpenNavPage(row + 1);
                    break;

                case 3:
                    if (tabs.ActiveTabs[row] != false)
                    {
                        ContentPage.Children.Add(await BelegungsPlanController.MainCreateMethode());
                        NavPosition = row;
                        MarkNavBar(0, row);
                    }
                    else
                        OpenNavPage(row + 1);
                    break;

                case 4:
                    if (tabs.ActiveTabs[row] != false)
                    {
                        AddWetter();
                        NavPosition = row;
                        MarkNavBar(0, row);
                    }
                    else
                        OpenNavPage(0);
                    break;
            }

        }
        //Navigation Ende

        private void MarkNavBar(int col, int row)
        {
            for (int i = 0; i < 2; i++)
            {
                var list = Navigation.Children
                .Where(elem => Grid.GetColumn((FrameworkElement)elem) == col && Grid.GetRow((FrameworkElement)elem) == row);



                if (list.ElementAt(i).GetType() == typeof(Border))
                {
                    Border element = (Border)list.ElementAt(i);
                    element.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                    if (lastMarked != null && !lastMarked.Equals(element))
                    {
                        lastMarked.Background = null;
                    }
                    lastMarked = element;
                }
            }
        }

        private async void AddWetter()
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool isWLANConnection = (InternetConnectionProfile == null) ? false : InternetConnectionProfile.IsWlanConnectionProfile;

            if (isInternetConnected)
            {
                // Tabelle erstellen
                Grid wetterTabelle = new Grid();

                //Datum
                DateTime thisDay = DateTime.Now;
                CultureInfo ci = new CultureInfo("de-DE");

                // Definiere die Zeilen
                ColumnDefinition[] colDefcollect = new ColumnDefinition[5];


                for (int i = 0; i < colDefcollect.Length; i++)
                {
                    colDefcollect[i] = new ColumnDefinition
                    {
                        Width = new GridLength(99)
                    };
                    wetterTabelle.ColumnDefinitions.Add(colDefcollect[i]);
                }


                // Definiere die Reihen
                RowDefinition[] rowDefcollect = new RowDefinition[5];

                for (int i = 0; i < rowDefcollect.Length; i++)
                {
                    rowDefcollect[i] = new RowDefinition
                    {
                        Height = new GridLength(100)
                    };
                    wetterTabelle.RowDefinitions.Add(rowDefcollect[i]);
                }

                // Position ermitteln
                Geoposition position = await LocationManager.GetPosition();

                RootObject weather = await OpenWeatherMap.GetWeather(
                    position.Coordinate.Point.Position.Latitude,
                    position.Coordinate.Point.Position.Longitude);

                TextBlock[,] wetterBeschreibung = new TextBlock[colDefcollect.Length, rowDefcollect.Length];
                Image[] wetterIcons = new Image[4];

                // Entnahme der Icons
                int helper = 0;
                for (int i = 0; i < weather.list.Count; i++)
                {
                    String temp = weather.list[i].dt_txt;
                    if ((helper < wetterIcons.Length && temp.Substring(temp.Length - 8, 8) == "12:00:00") || helper == 0)
                    {
                        String iconSource = String.Format("http://openweathermap.org/img/w/{0}.png", weather.list[i].weather[0].icon);
                        wetterIcons[helper] = new Image
                        {
                            Source = new BitmapImage(new Uri(iconSource, UriKind.Absolute)),
                            Width = 100.0,
                            Height = 200.0,
                            VerticalAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(0, 20, 0, 0)
                        };


                        Grid.SetColumn(wetterIcons[helper], 4);
                        Grid.SetRow(wetterIcons[helper], helper);
                        wetterTabelle.Children.Add(wetterIcons[helper]);
                        helper++;
                    }
                }

                // Erstellen der Textfelder
                for (int i = 0; i < wetterBeschreibung.GetLength(0); i++)
                {
                    for (int j = 0; j < wetterBeschreibung.GetLength(1); j++)
                    {
                        wetterBeschreibung[i, j] = new TextBlock
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Margin = new Thickness(0, 0, 0, 20)
                        };
                    }
                }

                helper = 0;
                for (int i = 0; i < weather.list.Count && helper < wetterBeschreibung.GetLength(1); i++)
                {
                    String temp = weather.list[i].dt_txt;
                    if (temp.Substring(temp.Length - 8, 8) == "12:00:00" || helper == 0)
                    {
                        wetterBeschreibung[helper, 0].Text = ci.DateTimeFormat.GetDayName(thisDay.DayOfWeek);
                        if (i + 2 < weather.list.Count && weather.list[i + 2].main.temp_max > weather.list[i].main.temp_max)
                        {
                            wetterBeschreibung[helper, 3].Text = weather.list[i + 2].main.temp_max.ToString() + " °C";
                            wetterBeschreibung[helper, 1].Text = weather.list[i].main.temp_max.ToString() + " °C";
                        }
                        else if (i + 2 < weather.list.Count)
                        {
                            wetterBeschreibung[helper, 3].Text = weather.list[i].main.temp_max.ToString() + " °C";
                            wetterBeschreibung[helper, 1].Text = weather.list[i + 2].main.temp_max.ToString() + " °C";
                        }
                        else
                        {
                            wetterBeschreibung[helper, 3].Text = weather.list[i].main.temp_max.ToString() + " °C";
                            wetterBeschreibung[helper, 1].Text = weather.list[i].main.temp_max.ToString() + " °C";
                        }

                        wetterBeschreibung[helper, 2].Text = " bis ";

                        Grid.SetColumn(wetterBeschreibung[helper, 0], 0);
                        Grid.SetRow(wetterBeschreibung[helper, 0], helper);
                        wetterTabelle.Children.Add(wetterBeschreibung[helper, 0]);

                        Grid.SetColumn(wetterBeschreibung[helper, 1], 1);
                        Grid.SetRow(wetterBeschreibung[helper, 1], helper);
                        wetterTabelle.Children.Add(wetterBeschreibung[helper, 1]);

                        Grid.SetColumn(wetterBeschreibung[helper, 2], 2);
                        Grid.SetRow(wetterBeschreibung[helper, 2], helper);
                        wetterTabelle.Children.Add(wetterBeschreibung[helper, 2]);

                        Grid.SetColumn(wetterBeschreibung[helper, 3], 3);
                        Grid.SetRow(wetterBeschreibung[helper, 3], helper);
                        wetterTabelle.Children.Add(wetterBeschreibung[helper, 3]);

                        helper++;
                        thisDay = thisDay.AddDays(1);
                    }
                }
                wetterTabelle.Margin = new Thickness(0, 40, 0, 0);
                ContentPage.Children.Add(wetterTabelle);
            }
            else
            {
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx://HelloWorld/Assets/No_Internet.png")),
                    Width = 300,
                    Height = 300
                };
                ContentPage.Children.Add(image);
            }
        }




    }
}
