using Foreground.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Foreground.Controller
{
    public sealed class AktuellesController
    {


        public static Aktuelles CreateModelFromJson(string json)
        {
            Aktuelles aktuelles = JsonConvert.DeserializeObject<Aktuelles>(json);
            return aktuelles;
        }

        public async static Task<Grid> AddAktuellesLayout()
        {
            Aktuelles aktuelles = null;
            try
            {
                var task = await LoadFromFile();
                aktuelles = task;
            }
            catch
            {
                aktuelles = new Aktuelles
                {
                    Title = "Lorem ipsum dolor sit amet",
                    Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                };
            }



            Grid grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 900
            };

            var titlebox = new TextBlock
            {
                Text = aktuelles.Title,
                FontSize = 35,       
                Height = 90,               
                Margin = new Thickness(15, 40, 470, 330),
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var textblock = new TextBlock
            {
                Text = aktuelles.Text,
                FontSize = 20,
                Width = 600,
                Margin = new Thickness(25, 100, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            textblock.Text = aktuelles.Text.Replace("\n", "\r\n");
            textblock.Text = aktuelles.Text.Replace(". ", ".\r\n");

            grid.Children.Add(titlebox);
            grid.Children.Add(textblock);

            return grid;

        }

        public async static Task<Aktuelles> LoadFromFile()
        {
            return await SaveController.ReadFromJsonFile<Aktuelles>("Aktuelles.txt");
        }
    }
}
