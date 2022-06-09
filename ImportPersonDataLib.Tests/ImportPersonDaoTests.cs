using ImportPersonDataLib.Dao;
using ImportPersonDataLib.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPersonDataLib.Tests
{

    [TestClass]
    public class ImportPersonDaoTests
    {
        static ImportPersonDao dao;        
        static StringBuilder sqlCondition;
        static List<SqlParameter> sqlParams;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            dao = new ImportPersonDao("sql", "conStr");
            
            sqlCondition = new StringBuilder();
            sqlParams = new List<SqlParameter>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            sqlCondition.Clear();
            sqlParams.Clear();
        }


        [TestMethod]
        public void SetParameters_3params_count3()
        {
            //Arrange
            Person p = new Person { Surname = "Фамилия", Name = "Имя", Oname = "Отчество" };
            int expectedCountParams = 4;

            //Act
            dao.SetParameters(p, sqlCondition, sqlParams);
            int actualCountParams = sqlParams.Count;

            //Assert
            Assert.AreEqual(expectedCountParams, actualCountParams);
        }

        [ExpectedException(typeof(ArgumentException), "Входных параметров меньше 3.") ]
        [TestMethod]
        public void SetParameters_1params_Exception()
        {
            //Arrange
            Person p = new Person { Surname = "Фамилия" };
          
            //Act
            dao.SetParameters(p, sqlCondition, sqlParams);
        }

    }
}
