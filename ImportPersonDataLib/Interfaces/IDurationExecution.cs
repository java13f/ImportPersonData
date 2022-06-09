using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPersonDataLib.Interfaces
{
    public interface IDurationExecution
    {
        string ElapsedTime { get; }
        void Start();
        void Stop();
    }
}
