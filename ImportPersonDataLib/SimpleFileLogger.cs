using ImportPersonDataLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImportPersonDataLib
{
    public class SimpleFileLogger : ILogger
    {
        public string PathFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                

        public void WriteLog(string message)
        {
            using (StreamWriter logFile = new StreamWriter(Path.Combine(PathFolder, "logfile.txt"), true))
            {
                logFile.WriteLine(message);
            }
        }


        internal bool FindLogFile(string fileName, IDataAccessObject dataAccessObject) 
        {
            if (dataAccessObject==null)
            {
                throw new ArgumentNullException("dataAccessObject", "Parameter dataAccessObject cannot null.");
            }

           List<string> files = dataAccessObject.GetFiles(PathFolder);

            foreach (var file in files)
            {
                if (file.Contains(fileName))
                {
                    return true;
                }
            }

            return false;
        }




    }
}
