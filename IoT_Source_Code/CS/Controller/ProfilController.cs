using System;
using System.Threading.Tasks;
using Foreground.Model;
using Foreground.Service;
using HelloWorld;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Foreground.Controller
{
    public sealed class ProfilController
    {
        public async static Task<Profil[]> Platzhalter()
        {

            // Fuege Bild hinzu
            BitmapImage bitmapImage = new BitmapImage(new Uri("ms-appx://HelloWorld/Assets/leeres_Profil.png"));
            Image image = new Image
            {
                Source = bitmapImage
            };

            Profil[] profils = new Profil[1];
            Profil profil = new Profil
            {
                Country = "Deutschland",
                Email = "lars.hildebrand@cs.tu-dortmund.de",
                Fax = "(+49) 231 755-7936",
                Image = await ByteArrayBitmapService.StandardBild(),
                Name = "Lars",
                Phone = "(+49) 231 755-7953",
                Place = "Dortmund",
                Placenumber = 44227,
                Role = "Ausländerbeauftragter",
                Street = "Otto-Hahn-Straße 12",
                Surname = "Hildebrand",
                Title = "Dr."
            };
            profils[0] = profil;
            //profils[1] = profil;
            //profils[2] = profil;
            return profils;
        }

        public static string[][] PrepareForUse(Profil[] profil)
        {
            string[][] preparedArray = new String[profil.Length][];
            for (int i = 0; i < profil.Length; i++)
            {

                preparedArray[i] = new String[11];
                preparedArray[i][0] = profil[i].Title + " " + profil[i].Name + " " + profil[i].Surname;
                preparedArray[i][1] = profil[i].Role;
                preparedArray[i][2] = profil[i].Street;
                preparedArray[i][3] = profil[i].Placenumber.ToString() + " "+ profil[i].Place;
                preparedArray[i][4] = profil[i].Country;
                preparedArray[i][5] = "Telefon:";
                preparedArray[i][6] = "Fax:";
                preparedArray[i][7] = "Email";
                preparedArray[i][8] = profil[i].Phone;
                preparedArray[i][9] = profil[i].Fax;
                preparedArray[i][10] = profil[i].Email;


            }


            return preparedArray;
        }

        public async static Task<Grid> AddAllgemein()
        {

            const int nameSize = 35;
            const int titleSize = 27;
            const int normalSize = 21;



            Grid contact = new Grid();
            Profil[] profil = null;
            try
            {
                profil = await SaveController.ReadArrayFromJsonFile("Profil.txt");
            }
            catch
            {
                profil = await Platzhalter();
            }

            string[][] preparedArray = PrepareForUse(profil);

            if (profil.Length == 1)
            {
                CreateOneProfil(contact, profil, preparedArray, nameSize, titleSize, normalSize);
            }
            else if (profil.Length == 2)
            {
                CreateTwoProfil(contact, profil, preparedArray, nameSize, titleSize, normalSize);
            }
            else
            {
                CreateThreeProfil(contact, profil, preparedArray, nameSize, titleSize, normalSize);
            }
            return contact;

        }

        public static Profil[] CreateModelFromJson(string json)
        {
            Profil[] profil = JsonConvert.DeserializeObject<Profil[]>(json);
            return profil;
        }

        private async static void CreateOneProfil(Grid contact, Profil[] profil, string[][] preparedArray, int nameSize, int titleSize, int normalSize)
        {

            Image image = new Image
            {
                Source = await ByteArrayBitmapService.AsBitmapImage(profil[0].Image),
                Height = 175,
                Width = 175,
                Margin = new Thickness(0, 0, 0, 40)
            };

            // Definiere die Zeilen
            ColumnDefinition[] colDefcollect = new ColumnDefinition[2];


            for (int i = 0; i < colDefcollect.Length; i++)
            {
                colDefcollect[i] = new ColumnDefinition();
                contact.ColumnDefinitions.Add(colDefcollect[i]);
            }

            colDefcollect[0].Width = new GridLength(320);
            colDefcollect[1].Width = new GridLength(340);

            // Definiere die Reihen
            RowDefinition[] rowDefcollect = new RowDefinition[8];

            for (int i = 0; i < rowDefcollect.Length; i++)
            {
                rowDefcollect[i] = new RowDefinition();
                contact.RowDefinitions.Add(rowDefcollect[i]);
            }

            // Schaffe Textfelder
            TextBlock[] contactDaten = new TextBlock[5];

            for (int i = 0; i < contactDaten.Length; i++)
            {
                contactDaten[i] = new TextBlock
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    Text = preparedArray[0][i]
                };
                if (i == 0)
                {
                    contactDaten[i].FontSize = nameSize;
                }
                else if (i == 1)
                {
                    contactDaten[i].FontSize = titleSize;
                }
                else
                {
                    contactDaten[i].FontSize = normalSize;
                }
                if (i == contactDaten.Length - 1)
                {
                    contactDaten[i].Margin = new Thickness(10, 0, 0, 70);
                }
                Grid.SetColumn(contactDaten[i], 0);
                Grid.SetRow(contactDaten[i], i);
            }


            Grid.SetColumn(image, 1);
            Grid.SetRow(image, 0);
            Grid.SetRowSpan(image, 5);




            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Grid subContact = new Grid();

            // Definiere die Zeilen
            ColumnDefinition[] subColDefcollect = new ColumnDefinition[2];


            for (int i = 0; i < subColDefcollect.Length; i++)
            {
                subColDefcollect[i] = new ColumnDefinition();
                subContact.ColumnDefinitions.Add(subColDefcollect[i]);
            }

            subColDefcollect[0].Width = new GridLength(150);
            subColDefcollect[1].Width = new GridLength(360);

            // Definiere die Reihen
            RowDefinition[] subRowDefcollect = new RowDefinition[4];

            for (int i = 0; i < subRowDefcollect.Length; i++)
            {
                subRowDefcollect[i] = new RowDefinition();
                subContact.RowDefinitions.Add(subRowDefcollect[i]);
            }


            // Schaffe Textfelder
            TextBlock[] subContactDaten = new TextBlock[6];

            int j = 0;
            for (int i = 0; i < subContactDaten.Length; i++)
            {
                subContactDaten[i] = new TextBlock
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    FlowDirection = FlowDirection.LeftToRight,
                    Text = preparedArray[0][i + 5],
                    FontSize = normalSize
                };
                if (i == 0 || i == 3)
                    subContactDaten[i].Margin = new Thickness(10, 40, 0, 0);
                if (i < 3)
                    Grid.SetColumn(subContactDaten[i], 0);
                else
                {
                    Grid.SetColumn(subContactDaten[i], 1);
                    if (j > 2)
                        j = 0;
                }
                Grid.SetRow(subContactDaten[i], j);
                j++;

            }


            Grid.SetColumn(subContact, 0);
            Grid.SetColumnSpan(subContact, 2);
            Grid.SetRow(subContact, 5);
            Grid.SetRowSpan(subContact, 4);
            foreach (TextBlock element in subContactDaten)
            {
                subContact.Children.Add(element);
            }


            // Hinzufuegen in erstellte Tabelle
            contact.Children.Add(image);
            contact.Children.Add(subContact);
            foreach (TextBlock element in contactDaten)
            {
                contact.Children.Add(element);
            }
        }

        private async static void CreateTwoProfil(Grid contact, Profil[] profil, string[][] preparedArray, int nameSize, int titleSize, int normalSize)
        {
            const int difference = 5;


            Image image = new Image
            {
                Source = await ByteArrayBitmapService.AsBitmapImage(profil[0].Image),
                Height = 150,
                Width = 150,
                Margin = new Thickness(0, 0, 0, 40)
            };

            Image image2 = new Image
            {
                Source = await ByteArrayBitmapService.AsBitmapImage(profil[1].Image),
                Height = 150,
                Width = 150,
                Margin = new Thickness(0, 0, 0, 40)
            };

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Grid together = new Grid();

            // Definiere die Zeilen
            ColumnDefinition[] colDefTogether = new ColumnDefinition[2];


            for (int i = 0; i < colDefTogether.Length; i++)
            {
                colDefTogether[i] = new ColumnDefinition();
                together.ColumnDefinitions.Add(colDefTogether[i]);
            }

            colDefTogether[0].Width = new GridLength(310);
            colDefTogether[1].Width = new GridLength(330);


            // Definiere die Reihen
            RowDefinition[] rowDefTogether = new RowDefinition[1];

            for (int i = 0; i < rowDefTogether.Length; i++)
            {
                rowDefTogether[i] = new RowDefinition();
                together.RowDefinitions.Add(rowDefTogether[i]);
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Grid grid1 = new Grid()
            {
                BorderThickness = new Thickness(0, 0, 1, 0),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
            };

            Grid grid2 = new Grid()
            {
                BorderThickness = new Thickness(1, 0, 0, 0),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
            };

            // Definiere die Zeilen
            for (int k = 0; k < profil.Length; k++)
            {
                ColumnDefinition[] colDefcollect = new ColumnDefinition[1];


                for (int i = 0; i < colDefcollect.Length; i++)
                {
                    colDefcollect[i] = new ColumnDefinition();
                    if (k == 1)
                        grid1.ColumnDefinitions.Add(colDefcollect[i]);
                    else
                        grid2.ColumnDefinitions.Add(colDefcollect[i]);
                }


                // Definiere die Reihen
                RowDefinition[] rowDefcollect = new RowDefinition[6];

                for (int i = 0; i < rowDefcollect.Length; i++)
                {
                    rowDefcollect[i] = new RowDefinition();
                    if (k == 1)
                        grid1.RowDefinitions.Add(rowDefcollect[i]);
                    else
                        grid2.RowDefinitions.Add(rowDefcollect[i]);
                }
            }
            // Schaffe Textfelder
            TextBlock[] contactDaten = new TextBlock[5];

            for (int k = 0; k < preparedArray.GetLength(0); k++)
            {
                int j = 0;
                for (int i = 0; i < preparedArray[0].GetLength(0); i++)
                {
                    if (i < 2 || i > 7)
                    {
                        contactDaten[j] = new TextBlock
                        {
                            Margin = new Thickness(10, 0, 0, 0),
                            Text = preparedArray[k][i]
                        };
                        if (i == 0)
                        {
                            contactDaten[j].FontSize = nameSize - difference;
                        }
                        else if (i == 1)
                        {
                            contactDaten[j].FontSize = titleSize - difference;
                        }
                        else
                        {
                            contactDaten[j].FontSize = normalSize - difference;
                        }
                        if (i == contactDaten.Length - 3)
                        {
                            contactDaten[j].Margin = new Thickness(10, 20, 0, 0);
                        }

                        Grid.SetColumn(contactDaten[j], 0);
                        Grid.SetRow(contactDaten[j], j + 1);

                        if (k == 0)
                            grid1.Children.Add(contactDaten[j]);
                        else
                            grid2.Children.Add(contactDaten[j]);
                        j++;
                    }
                }

            }


            Grid.SetColumn(image, 0);
            Grid.SetRow(image, 0);
            grid1.Children.Add(image);

            Grid.SetColumn(grid1, 0);
            Grid.SetRow(grid1, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Grid.SetColumn(image2, 1);
            Grid.SetRow(image2, 0);
            grid2.Children.Add(image2);

            Grid.SetColumn(grid2, 1);
            Grid.SetRow(grid2, 0);

            together.Children.Add(grid1);
            together.Children.Add(grid2);
            contact.Children.Add(together);
        }

        private async static void CreateThreeProfil(Grid contact, Profil[] profil, string[][] preparedArray, int nameSize, int titleSize, int normalSize)
        {
            const int difference = 10;
            const int differenceName = 5;

            Image image = new Image
            {
                Source = await ByteArrayBitmapService.AsBitmapImage(profil[0].Image),
                Height = 150,
                Width = 150,
                Margin = new Thickness(0, 0, 0, 40)
            };

            Image image2 = new Image
            {
                Source = await ByteArrayBitmapService.AsBitmapImage(profil[1].Image),
                Height = 150,
                Width = 150,
                Margin = new Thickness(0, 0, 0, 40)
            };

            Image image3 = new Image
            {
                Source = await ByteArrayBitmapService.AsBitmapImage(profil[2].Image),
                Height = 150,
                Width = 150,
                Margin = new Thickness(0, 0, 40, 40)
            };

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Grid together = new Grid();

            // Definiere die Zeilen
            ColumnDefinition[] colDefTogether = new ColumnDefinition[3];

            for (int i = 0; i < colDefTogether.Length; i++)
            {
                colDefTogether[i] = new ColumnDefinition();
                together.ColumnDefinitions.Add(colDefTogether[i]);
            }

            colDefTogether[0].Width = new GridLength(200);
            colDefTogether[1].Width = new GridLength(200);
            colDefTogether[2].Width = new GridLength(242);


            // Definiere die Reihen
            RowDefinition[] rowDefTogether = new RowDefinition[1];

            for (int i = 0; i < rowDefTogether.Length; i++)
            {
                rowDefTogether[i] = new RowDefinition();
                together.RowDefinitions.Add(rowDefTogether[i]);
            }

            Grid grid1 = new Grid()
            {
                BorderThickness = new Thickness(0, 0, 1, 0),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
            };

            Grid grid2 = new Grid()
            {
                BorderThickness = new Thickness(1, 0, 1, 0),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
            };

            Grid grid3 = new Grid()
            {
                BorderThickness = new Thickness(1, 0, 0, 0),
                BorderBrush = new SolidColorBrush(Windows.UI.Colors.White)
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Definiere die Zeilen
            for (int k = 0; k < profil.Length; k++)
            {
                ColumnDefinition[] colDefcollect = new ColumnDefinition[1];


                for (int i = 0; i < colDefcollect.Length; i++)
                {
                    colDefcollect[i] = new ColumnDefinition();
                    if (k == 1)
                        grid1.ColumnDefinitions.Add(colDefcollect[i]);
                    else if (k == 2)
                        grid2.ColumnDefinitions.Add(colDefcollect[i]);
                    else
                        grid3.ColumnDefinitions.Add(colDefcollect[i]);
                }


                // Definiere die Reihen
                RowDefinition[] rowDefcollect = new RowDefinition[6];

                for (int i = 0; i < rowDefcollect.Length; i++)
                {
                    rowDefcollect[i] = new RowDefinition();
                    if (k == 1)
                        grid1.RowDefinitions.Add(rowDefcollect[i]);
                    else if (k == 2)
                        grid2.RowDefinitions.Add(rowDefcollect[i]);
                    else
                        grid3.RowDefinitions.Add(rowDefcollect[i]);
                }
            }

            // Schaffe Textfelder
            TextBlock[] contactDaten = new TextBlock[5];

            for (int k = 0; k < preparedArray.GetLength(0); k++)
            {
                int j = 0;
                for (int i = 0; i < preparedArray[0].GetLength(0); i++)
                {
                    if (i < 2 || i > 7)
                    {
                        contactDaten[j] = new TextBlock
                        {
                            Margin = new Thickness(10, 0, 0, 0),
                            Text = preparedArray[k][i]
                        };
                        if (i == 0)
                        {
                            contactDaten[j].FontSize = nameSize - difference - differenceName;
                        }
                        else if (i == 1)
                        {
                            contactDaten[j].FontSize = titleSize - difference;
                        }
                        else
                        {
                            contactDaten[j].FontSize = normalSize - difference;
                        }
                        if (i == contactDaten.Length - 3)
                        {
                            contactDaten[j].Margin = new Thickness(10, 20, 0, 0);
                        }

                        Grid.SetColumn(contactDaten[j], 0);
                        Grid.SetRow(contactDaten[j], j + 1);

                        if (k == 0)
                            grid1.Children.Add(contactDaten[j]);
                        else if (k == 1)
                            grid2.Children.Add(contactDaten[j]);
                        else
                            grid3.Children.Add(contactDaten[j]);
                        j++;
                    }
                }

            }

            Grid.SetColumn(image, 0);
            Grid.SetRow(image, 0);
            grid1.Children.Add(image);

            Grid.SetColumn(grid1, 0);
            Grid.SetRow(grid1, 0);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Grid.SetColumn(image2, 1);
            Grid.SetRow(image2, 0);
            grid2.Children.Add(image2);

            Grid.SetColumn(grid2, 1);
            Grid.SetRow(grid2, 0);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Grid.SetColumn(image3, 2);
            Grid.SetRow(image3, 0);
            grid3.Children.Add(image3);

            Grid.SetColumn(grid3, 2);
            Grid.SetRow(grid3, 0);

            together.Children.Add(grid1);
            together.Children.Add(grid2);
            together.Children.Add(grid3);
            contact.Children.Add(together);
        }
    }
}
