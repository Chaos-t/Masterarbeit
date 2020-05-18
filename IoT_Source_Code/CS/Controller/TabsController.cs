using Foreground.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreground.Controller
{
    public sealed class TabsController
    {
        public Boolean[] ActiveTabs { get; set; }

        public static Tabs CreateModelFromJson(string json)
        {
            Tabs tabs = JsonConvert.DeserializeObject<Tabs>(json);
            return tabs;
        }
    }
}
