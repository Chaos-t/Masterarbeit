using Foreground.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Foreground.Controller
{
    public sealed class VeranstaltungController
    {

        public async static Task<Veranstaltung[]> LoadVeranstaltung()
        {
  
            try
            {
                Veranstaltung[] events = await SaveController.ReadVArrayFromJsonFile("Veranstaltung.txt");

                return events;
            }
            catch
            {
                 
                Veranstaltung veranstaltung1 = new Veranstaltung
                {
                    Day = "Montag",
                    From = 16,
                    To = 17,
                    Title = "Logische Methoden der Informatik"
                                     
                };

                Veranstaltung veranstaltung2 = new Veranstaltung
                {
                    Day = "Dienstag",
                    From = 15,
                    To = 16,
                    Title = "Methodische Grundlagen der Informatik"
                };

                Veranstaltung veranstaltung3 = new Veranstaltung
                {
                    Day = "Mittwoch",
                    From = 14,
                    To = 15,
                    Title = "Mathematik für Informatiker"
                };

                Veranstaltung veranstaltung4 = new Veranstaltung
                {
                    Day = "Freitag",
                    From = 16,
                    To = 18,
                    Title = "Mathematik für Informatiker"
                };

                Veranstaltung[] events = { veranstaltung1,veranstaltung2, veranstaltung3, veranstaltung4};
                return events;
            }

            

           
        }


        public static Veranstaltung[] CreateModelFromJson(string json)
        {
            Veranstaltung[] veranstaltung = JsonConvert.DeserializeObject<Veranstaltung[]>(json);
            return veranstaltung;
        }

        public async static Task<Grid> AddVeranstaltung()
        {
            Veranstaltung[] events = await LoadVeranstaltung();
            int anzahl = events.Length;

            TextBlock[] felder = new TextBlock[anzahl * 2];

            int j = 0;
            for(int i = 0; i < anzahl;i++)
            {               

                felder[j] = new TextBlock
                {
                    Text = events[i].Title,
                    FontSize = 30,
                    Margin = new Thickness(0,0,0,0),
                    Padding = new Thickness(0,0,0,0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                j++;
                felder[j] = new TextBlock
                {
                    Text = events[i].Day + " " + events[i].From + ":00 Uhr bis " + events[i].To + ":00 Uhr",
                    FontSize = 20,
                    Margin = new Thickness(10, 0, 0, 15),
                    Padding = new Thickness(0, 0, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                j++;
            }



            Grid veranstaltungsContent = new Grid
            {
                Margin = new Thickness(0, 0, 0, 30)
            };

            // Definiere die Zeilen
            ColumnDefinition[] colDefcollect = new ColumnDefinition[1];


            for (int i = 0; i < colDefcollect.Length; i++)
            {
                colDefcollect[i] = new ColumnDefinition();
                veranstaltungsContent.ColumnDefinitions.Add(colDefcollect[i]);
            }


            // Definiere die Reihen
            RowDefinition[] rowDefcollect = new RowDefinition[anzahl * 2];

            for (int i = 0; i < rowDefcollect.Length; i++)
            {
                if (i % 2 == 1)
                    rowDefcollect[i] = new RowDefinition()
                    {
                    };
                else
                    rowDefcollect[i] = new RowDefinition()
                    {
                        Height = new GridLength(40)
                    };


                veranstaltungsContent.RowDefinitions.Add(rowDefcollect[i]);
            }

            for(int i = 0; i< felder.Length; i++)
            {
                Grid.SetColumn(felder[i], 0);
                Grid.SetRow(felder[i], i);
                veranstaltungsContent.Children.Add(felder[i]);
            }
            

            return veranstaltungsContent;
        }
    }
}
