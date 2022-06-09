using ImportPersonDataLib.Interfaces;
using ImportPersonDataLib.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ImportPersonDataLib.Tests
{
    [TestClass]
    public class SimpleFileLoggerTests
    {

        [TestMethod]
        [DeploymentItem("Resources\\logfile.txt")]
        [DeploymentItem("Resources\\logfileTemplate.txt")]
        public void Log_WriteText2()
        {
            //Arrange
            ILogger logger = new SimpleFileLogger();
            logger.PathFolder = ".";
            string expectedText = File.ReadAllText("logfileTemplate.txt");

            //Act
            logger.WriteLog("Any text.");
            string actualText = File.ReadAllText("logfile.txt").Trim();

            //Assert
            Assert.AreEqual(expectedText, actualText);
        }


        [TestMethod]
        public void FindLogFile_NameFile_ReturnTrue()
        {
            //Arrange
            IDataAccessObject dataAccessObject = new StubFileDataObject();
            SimpleFileLogger logger = new SimpleFileLogger();            
            string fileName = "logfile.txt";

            //Act
            bool result = logger.FindLogFile(fileName, dataAccessObject);

            //Assert
            Assert.IsTrue(result);
        }


    }
}
