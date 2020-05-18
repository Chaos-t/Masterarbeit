using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreground.Model
{
    public sealed class Veranstaltung
    {
        public int From { get; set; }
        public int To { get; set; }
        public String Day { get; set; }
        public String Title { get; set; }
    }
}
