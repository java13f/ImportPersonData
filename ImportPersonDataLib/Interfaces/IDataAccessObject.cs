using System.Collections.Generic;

namespace ImportPersonDataLib.Interfaces
{
    public interface IDataAccessObject
    {
        List<string> GetFiles(string path);
    }
}
