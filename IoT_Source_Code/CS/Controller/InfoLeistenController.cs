using Foreground.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreground.Controller
{
    public sealed class InfoleistenController
    {
        public static Infoleiste CreateModelFromJson(string json)
        {
            Infoleiste infoleiste = JsonConvert.DeserializeObject<Infoleiste>(json);
            return infoleiste;
        }
    }
}
