using ImportPersonDataLib.Models;
using System.Collections.Generic;

namespace ImportPersonDataLib.Interfaces
{
    public interface IImportPersonDao
    {
        List<Person> GetPersons();
                
        void WriteErrors(List<Person> persons);

        void ClearFieldErrorMessage();

        void AddPersonToDatabase(Person person, int idAddress);

        int GetIdAddress(Person person);
    }
}
