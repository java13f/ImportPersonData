using System;

namespace ImportPersonDataLib.Models
{
    public class Person
    {
        public int? Num { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Oname { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Rayon { get; set; }
        public string Gorod { get; set; }
        public string Geonim { get; set; }
        public string Street { get; set; }
        public string Dom { get; set; }
        public string Kv { get; set; }

        public string ErrorMessage { get; set; }        

    }
}                     
