using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamenschildDesktop.Model
{
    public class Profil
    {
        public String Title { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Role { get; set; }
        public String Street { get; set; }
        public String Place { get; set; }
        public String Country { get; set; }
        public int Placenumber { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Email { get; set; }
        public byte[] Image { get; set; }
    }
}
