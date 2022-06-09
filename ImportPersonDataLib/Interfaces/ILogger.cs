using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPersonDataLib.Interfaces
{
    public interface ILogger
    {
        string PathFolder { get; set; }

        void WriteLog(string message);
    }
}
