using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPersonDataLib.Models
{
    public class Address
    {
        public int idGorodRayon  { get; set; }
        public int idStreet  { get; set; }
        public int idNomerDom  { get; set; }
        public int? idNomerKvartira  { get; set; }
        public int? idAddress  { get; set; }
    }
}
