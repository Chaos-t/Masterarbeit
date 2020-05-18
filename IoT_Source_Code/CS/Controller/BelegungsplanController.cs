using Foreground.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Foreground.Controller
{
    public sealed class BelegungsPlanController
    {

        public static BelegungsPlan CreateModelFromJson(string json)
        {
            BelegungsPlan belegungsPlan = JsonConvert.DeserializeObject<BelegungsPlan>(json);
            return belegungsPlan;
        }

        public async static Task<Grid> MainCreateMethode()
        {
            Grid belegungsplan = new Grid();

            belegungsplan = PrepareGrid(belegungsplan);


            BelegungsPlan belegungsPlan = null;
            try
            {
                var task = await LoadFromFile();
                belegungsPlan = task;
            }
            catch
            {
                string[] platzhalter = { "", "Veranstaltung", "Veranstaltung", "", "", "Veranstaltung", "Veranstaltung", "Veranstaltung", "", "", "", "" };
                belegungsPlan = new BelegungsPlan
                {

                    Montag = platzhalter,
                    Dienstag = platzhalter,
                    Mittwoch = platzhalter,
                    Donnerstag = platzhalter,
                    Freitag = platzhalter
                };
            }

            TextBlock[] MontagBox = CreateTextBox(belegungsPlan.Montag);
            TextBlock[] DienstagBox = CreateTextBox(belegungsPlan.Dienstag);
            TextBlock[] MittwochBox = CreateTextBox(belegungsPlan.Mittwoch);
            TextBlock[] DonnerstagBox = CreateTextBox(belegungsPlan.Donnerstag);
            TextBlock[] FreitagBox = CreateTextBox(belegungsPlan.Freitag);


            // Einzeichnen der äußeren Tabellenlinie
            Grid veranstaltungen = new Grid
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
            };

            ColumnDefinition[] colDefcollect = new ColumnDefinition[5];


            for (int i = 0; i < colDefcollect.Length; i++)
            {
                colDefcollect[i] = new ColumnDefinition
                {
                    Width = new GridLength(98.8)
                };
                veranstaltungen.ColumnDefinitions.Add(colDefcollect[i]);
            }


            // Definiere die Reihen
            RowDefinition[] rowDefcollect = new RowDefinition[12];

            for (int i = 0; i < rowDefcollect.Length; i++)
            {
                rowDefcollect[i] = new RowDefinition
                {
                    Height = new GridLength(30.8)
                };
                veranstaltungen.RowDefinitions.Add(rowDefcollect[i]);
            }



            int[] monday = CreateSample(MontagBox);
            int[] tuesday = CreateSample(DienstagBox);
            int[] wednesday = CreateSample(MittwochBox);
            int[] thursday = CreateSample(DonnerstagBox);
            int[] friday = CreateSample(FreitagBox);

            int[][] week = { monday, tuesday, wednesday, thursday, friday };
            TextBlock[][] weekText = { MontagBox, DienstagBox, MittwochBox, DonnerstagBox, FreitagBox };

            //Heutiger Tag
            int today = (int)DateTime.Now.DayOfWeek - 1 ;
            int todayTimeHour = DateTime.Now.Hour;

            int hour = -10;

            for (int i = 0; i < week.Length; i++)
            {
                for (int j = 0; j < week[i].Length; j++)
                {
                    if (week[i][j] != 0)
                    {
                        if (today == i)
                        {
                            if (todayTimeHour > 7 && todayTimeHour < 20)
                            {
                                hour = todayTimeHour - 8;
                                
                            }
                        }
                        else
                            hour = -10;

                        Border border;
                        if (weekText[i][j - (week[i][j] - 1)].Text != "")
                        {
                            border = new Border
                            {
                                BorderThickness = new Thickness(1),
                                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White),
                                Background = new SolidColorBrush(Windows.UI.Colors.LightBlue)
                            };
                        }
                        else
                        {
                            border = new Border
                            {
                                BorderThickness = new Thickness(1),
                                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White),
                            };
                        }
                        if (hour > j - (week[i][j]) && hour < j + 1)
                        {

                            border.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                            hour = -10;
                        }


                        Grid.SetColumn(weekText[i][j - (week[i][j] - 1)], i);
                        Grid.SetRow(weekText[i][j - (week[i][j] - 1)], j - (week[i][j] - 1));

                        Grid.SetColumn(border, i);
                        Grid.SetRow(border, j - (week[i][j] - 1));

                        Grid.SetRowSpan(border, week[i][j]);
                        Grid.SetRowSpan(weekText[i][j - (week[i][j] - 1)], week[i][j]);

                        veranstaltungen.Children.Add(border);
                        veranstaltungen.Children.Add(weekText[i][j - (week[i][j] - 1)]);
                    }
                }
            }

            Grid.SetColumn(veranstaltungen, 1);
            Grid.SetRow(veranstaltungen, 1);
            Grid.SetColumnSpan(veranstaltungen, 5);
            Grid.SetRowSpan(veranstaltungen, 12);

            belegungsplan.Children.Add(veranstaltungen);

            Border outBorder = new Border()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
            };

            belegungsplan.Children.Add(outBorder);

            return belegungsplan;
        }

        private static Grid PrepareGrid(Grid belegungsplan)
        {
            TextBlock[] zeiten = new TextBlock[6];
            TextBlock[] tage = new TextBlock[5];

            ColumnDefinition[] colDefcollect = new ColumnDefinition[6];
            for (int i = 0; i < colDefcollect.Length; i++)
            {
                colDefcollect[i] = new ColumnDefinition
                {
                    Width = new GridLength(99)
                };
                belegungsplan.ColumnDefinitions.Add(colDefcollect[i]);
            }


            // Definiere die Reihen
            RowDefinition[] rowDefcollect = new RowDefinition[13];

            for (int i = 0; i < rowDefcollect.Length; i++)
            {
                rowDefcollect[i] = new RowDefinition
                {
                    Height = new GridLength(31)
                };
                belegungsplan.RowDefinitions.Add(rowDefcollect[i]);
            }


            int zeitbeginn = 8;
            int zeitende = zeitbeginn + 2;
            for (int i = 0; i < zeiten.Length; i++)
            {


                zeiten[i] = new TextBlock();
                Border border = new Border()
                {
                    BorderThickness = new Thickness(1,1,0,1),
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
                };
                if (zeitbeginn < 10)
                {
                    zeiten[i].Text = "0" + zeitbeginn + ":00 - " + zeitende + ":00";
                }
                else
                {
                    zeiten[i].Text = zeitbeginn + ":00 - " + zeitende + ":00";
                }
                zeiten[i].FontSize = 15;
                zeiten[i].Padding = new Thickness(9, 12, 0, 0);
                zeiten[i].Width = 300;
                Grid.SetColumn(zeiten[i], 0);
                Grid.SetRow(zeiten[i], 1 + (2 * i));
                Grid.SetColumnSpan(zeiten[i], 1);
                Grid.SetRowSpan(zeiten[i], 2);

                Grid.SetColumn(border, 0);
                Grid.SetRow(border, 1 + (2 * i));
                Grid.SetColumnSpan(border, 1);
                Grid.SetRowSpan(border, 2);

                belegungsplan.Children.Add(zeiten[i]);
                belegungsplan.Children.Add(border);

                zeitbeginn += 2;
                zeitende += 2;


            }

            for (int i = 0; i < tage.Length; i++)
            {
                tage[i] = new TextBlock
                {
                    FontSize = 15,
                    Padding = new Thickness(12, 7, 0, 0)
                };
                Border border = new Border()
                {
                    BorderThickness = new Thickness(1,1,1,0),
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
                };
                Grid.SetColumn(tage[i], 1 + i);
                Grid.SetRow(tage[i], 0);
                //Grid.SetRowSpan(tage[i], 2);
                belegungsplan.Children.Add(tage[i]);

                Grid.SetColumn(border, 1 + i);
                Grid.SetRow(border, 0);
                //Grid.SetRowSpan(border, 2);
                belegungsplan.Children.Add(border);
            }

            tage[0].Text = "Montag";
            tage[1].Text = "Dienstag";
            tage[2].Text = "Mittwoch";
            tage[3].Text = "Donnerstag";
            tage[4].Text = "Freitag";

            return belegungsplan;
        }

        public static int[] CreateSample(TextBlock[] day)
        {
            int[] sameRow = new int[day.Length];
            int k = 1;

            for (int j = 0; j < day.Length; j++)
            {
                if (j + 1 != day.Length && day[j].Text.Equals(day[j + 1].Text))
                {
                    sameRow[j] = 0;
                    k++;
                }
                else
                {
                    sameRow[j] = k;
                    k = 1;
                }
            }
            return sameRow;

        }
        public async static Task<BelegungsPlan> LoadFromFile()
        {
            return await SaveController.ReadFromJsonFile<BelegungsPlan>("BelegungsPlan.txt");
        }

        public static TextBlock[] CreateTextBox(string[] day)
        {
            TextBlock[] textboxField = new TextBlock[day.Length];
            for (int i = 0; i < textboxField.Length; i++)
            {
                textboxField[i] = new TextBlock
                {
                    Text = day[i],
                    FontSize = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center


                };
            }
            return textboxField;
        }
    }
}
