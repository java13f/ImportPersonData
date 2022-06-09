using ImportPersonDataLib.Interfaces;
using System.Collections.Generic;

namespace ImportPersonDataLib.Stubs
{
    public class StubFileDataObject : IDataAccessObject
    {
        public List<string> GetFiles(string path)
        {
            List<string> list = new List<string> {
                "file1.txt",
                "file2.txt",
                "logfile.txt",
                "main.log"
            };

            return list;
        }



    }
}
