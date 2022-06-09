using ImportPersonDataLib.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace ImportPersonDataLib
{
    public class FileDataObject : IDataAccessObject
    {
        public List<string> GetFiles(string path)
        {            
            List<string> list = new List<string>();
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] files = d.GetFiles();

            foreach (var file in files)
            {
                list.Add(file.Name);
            }

            return list;
        }


    }
}
