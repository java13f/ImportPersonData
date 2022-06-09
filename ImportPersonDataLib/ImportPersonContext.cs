using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPersonDataLib
{
    public class ImportPersonContext : DbContext
    {
        public ImportPersonContext(string connectionString) : base(connectionString)
        {
        }
    }
}
