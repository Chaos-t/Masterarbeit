using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreground.Model
{
    public sealed class Infoleiste
    {
        public String Title { get; set; }
        public String Room { get; set; }
        public String Chair { get; set; }
        public byte[] LeftImage { get; set; }
        public byte[] RightImage { get; set; }
    }
}
