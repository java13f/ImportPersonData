using ImportPersonDataLib.Dao;
using ImportPersonDataLib.Interfaces;
using ImportPersonDataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImportPersonDataLib
{
    public class ImportPersonDataFromDb : IImportPersonData
    {              
        private readonly IImportPersonDao dao;
        private readonly IAddDatabaseFieldsDao addFieldsDao;
        private readonly ILogger logger;

        public ImportPersonDataFromDb(string sql, string connectionString)
        {            
            dao = new ImportPersonDao(sql, connectionString);
            addFieldsDao = new AddDatabaseFieldsDao(sql, connectionString);
            logger = new SimpleFileLogger();
        }

        private bool CheckDataBeforeImport()
        {            
            try
            {
                //Получить список данных
                List<Person> persons = dao.GetPersons();

                //Проверить список данных
                ICheckData checkData = new CheckData();
                var result = checkData.CheckList(persons);
                //Очистить поля в которых были сообщения об ошибками
                dao.ClearFieldErrorMessage();

                if (!result)
                {                    
                    var onlyErrors = persons.Where(p => p.ErrorMessage != null).ToList();

                    //пишу ошибки в базу данных
                    dao.WriteErrors(onlyErrors);
                    return false;
                }                

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
                      

        public bool Import()
        {
            try
            {
                logger.WriteLog($"{DateTime.Now} - Method Import start.");
                IDurationExecution duration = new DurationExecution();
                                               
                duration.Start();
                                
                if (!addFieldsDao.AddFields()) { return false; }
                
                if (!CheckDataBeforeImport()) { return false; }

                var result = ExecuteImport();
                duration.Stop();
                logger.WriteLog($"{DateTime.Now} - Method Import stop. Duration: {duration.ElapsedTime}");
                                
                return result;
            }
            catch (Exception ex)
            {
                logger.WriteLog($"{DateTime.Now} - Error in method Import: {ex.Message}");                
                return false;
            }            
        }


        private bool ExecuteImport()
        {            
            //Получить список согласно запросу
            List<Person> persons = dao.GetPersons();

            bool isError = false;

            foreach (var person in persons)
            {
                try
                {
                    int idAddress = dao.GetIdAddress(person);
                    dao.AddPersonToDatabase(person, idAddress);
                }
                catch (Exception ex)
                {
                    isError = true;
                    person.ErrorMessage = ex.Message;
                    dao.WriteErrors(new List<Person> { person });
                    continue;
                }
            }

            return !isError;
        }
             


    }    
}
