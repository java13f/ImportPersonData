using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImportPersonDataLib.Tests
{
    [TestClass]
    public class UtilsTests
    {
        private Utils utils;
        //public TestContext TestContext { get; set; }

        /// <summary>
        /// Запускается перед стартом каждого медота
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            utils = new Utils();
        }


        [TestMethod]
        public void GetNameTable_SqlQuery_NameTableReturned()
        {
            //Arrange
            string sql = "select * from    MyTable where id=5";
            var expected = "Mytable";

            //Act            
            var actual = utils.GetNameTable(sql);

            //Assert
            Assert.AreEqual(expected, actual, true, "В запросе должен быть оператор 'from' и через пробел имя таблицы.");
        }


        [ExpectedException(typeof(ArgumentException), "Ошибка определения имени таблицы. Не указан оператор 'from'")]
        [TestMethod]
        public void GetNameTable_ExceptionNotFrom()
        {
            //Arrange
            string sql = "select * MyTable where id=5";
            //Act            
            var actual = utils.GetNameTable(sql);
        }


        [ExpectedException(typeof(ArgumentException), "Ошибка определения имени таблицы.")]
        [TestMethod]
        public void GetNameTable_ExceptionNotTable()
        {
            //Arrange
            string sql = "select * from";
            //Act            
            var actual = utils.GetNameTable(sql);
        }


        [TestMethod]
        public void SeparatNumberAndLetter_325А_325()
        {
            //Arrange
            int expectedPartC = 325;
            string expectedPartB = "А";

            //Act
            utils.SeparatNumberAndLetter("325А", out int partC, out string partB);

            //Assert
            Assert.AreEqual(expectedPartC, partC);
            Assert.AreEqual(expectedPartB, partB);

        }



        [TestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void SeparatNumberAndLetter_xmlData_NumberAndLetter(string num, string expectedPartC, string expectedPartB)
        {            
            //Act
            utils.SeparatNumberAndLetter(num, out int partC, out string partB);

            //Assert
            Assert.AreEqual(int.Parse(expectedPartC), partC);
            Assert.AreEqual(expectedPartB, partB);
        }


        private static string[] SplitCsv(string input)
        {
            var csvSplit = new Regex("(?:^|;)(\"(?:[^\"]+|\"\")*\"|[^;]*)", RegexOptions.Compiled);
            var list = new List<string>();
            foreach (Match match in csvSplit.Matches(input))
            {
                string value = match.Value;
                if (value.Length == 0)
                {
                    list.Add(string.Empty);
                }

                list.Add(value.TrimStart(';'));
            }
            return list.ToArray();
        }

        private static IEnumerable<string[]> GetData()
        {
            IEnumerable<string> rows = System.IO.File.ReadAllLines(@"Resources\testDataCsvUtf.csv").Skip(1);
            foreach (string row in rows)
            {
                yield return SplitCsv(row);
            }
        }



    }
}
