using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamenschildDesktop.Model
{
    public class HardwareInfo
    {
        public string Name { get; set; }
        public string HarwareId { get; set; }
        public string Network { get; set; }
        public string Ip { get; set; }
        public string Version { get; set; }
        public bool Ipv6 { get; set; }
        public bool Ipv4 { get; set; }
    }
}
