using ImportPersonDataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPersonDataLib.Interfaces
{
    public interface ICheckData
    {        
        bool CheckList(List<Person> list);
    }
}
