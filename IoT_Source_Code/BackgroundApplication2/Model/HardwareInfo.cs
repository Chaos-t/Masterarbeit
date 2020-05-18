using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication2.Model
{
    public sealed class HardwareInfo
    {
        public String Name { get; set; }
        public String HarwareId { get; set; }
        public String Network { get; set; }
        public String Ip { get; set; }
        public String Version { get; set; }

        public bool Ipv6 { get; set; }
        public bool Ipv4 { get; set; }
    }
}
