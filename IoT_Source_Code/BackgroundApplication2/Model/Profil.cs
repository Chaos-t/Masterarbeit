using System;


namespace BackgroundApplication2.Model
{
    public sealed class Profil
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
