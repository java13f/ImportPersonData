using ImportPersonDataLib.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPersonDataLib.Tests
{
    [TestClass]
    public class CheckDataTests
    {
        List<Person> persons;
        CheckData checkData;

        [TestInitialize]
        public void TestInitialize()
        {
            checkData = new CheckData();

            persons = new List<Person> {
                new Person { Surname="", Name="", DateBirth=new DateTime().Date, Rayon="", Gorod="", Street="", Dom=""}                
            };   
        }

       

        [TestMethod]
        public void CheckList_PersonsWithErrors_MessageErrorAndFalse()
        {
            //Arrange
            string expeсtedError = 
                "Поле \"Фамилия\" не заполнено."+"#"
                + "Поле \"Имя\" не заполнено." +"#"
                + "Поле \"Район\" не заполнено." + "#"
                + "Поле \"Город\" не заполнено." + "#"
                + "Поле \"Улица\" не заполнено." + "#"
                + "Поле \"Дом\" не заполнено.";

            //Act
            bool result = checkData.CheckList(persons);

            //Assert
            Assert.AreEqual(expeсtedError, persons[0].ErrorMessage);
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void CheckList_Persons_ResultTrue()
        {
            //Arrange

            persons.Clear();
            persons.Add(new Person { Surname = "Иванов", Name = "Иван", DateBirth = new DateTime().Date, Rayon = "Район", Gorod = "Город", Street = "Улица", Dom = "Дом" });
            int expeсtedCountError = 0;             

            //Act
            bool result = checkData.CheckList(persons);
            int actualCountError = persons.Where(x => x.ErrorMessage != null).Count();

            //Assert
            Assert.AreEqual(expeсtedCountError, actualCountError);
            Assert.IsTrue(result);
        }


    }
}
