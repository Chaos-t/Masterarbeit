using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication2.Model
{
    public sealed class BelegungsPlan
    {
        public String[] Montag { get; set; }
        public String[] Dienstag { get; set; }
        public String[] Mittwoch { get; set; }
        public String[] Donnerstag { get; set; }
        public String[] Freitag { get; set; }
    }
}
