using ImportPersonDataLib;
using ImportPersonDataLib.Interfaces;
using System;

namespace _06_WorkWithImportPerson
{
    class Program
    {
        static void Main(string[] args)
        {
                      

            string sql = "select num, surname ,name , oname " +
                        ", dateBirth, rayon, gorod, geonim " +
                        ", street, dom, kv " +
                        "from Happy3 " +
                        "where isImport is null ";

            

            string connectionString = @"Data Source=server;Initial Catalog=nameDatabase;Integrated Security=True;"; 

            IImportPersonData importPerson = new ImportPersonDataFromDb(sql, connectionString);
            
            var result = importPerson.Import();
            if (!result)
            {
                Console.WriteLine("При импорте данных произошла ошибка.");
                Console.WriteLine("Информация в базе данных в поле \"ErrorMessage\" и в каталоге \"Документы\\logfile.txt\".");
            }
            else
            {
                Console.WriteLine("Импорт прошел успешно.");
            }

            
            Console.ReadKey();
        }


       




    }
}
