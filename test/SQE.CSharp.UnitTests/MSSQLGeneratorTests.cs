using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQE.SQLGenerators;

namespace SQE.CSharp.UnitTests
{
    [TestClass]
    public class MSSQLGeneratorTests
    {
        [TestMethod]
        public void Test_MSSQLGen_Empty()
        {
            var sqlCommand = SQE.GenerateCommand(new MSSQLGenerator("slg.lgs"), string.Empty);
            Assert.IsNotNull(sqlCommand);
            Assert.AreEqual("SELECT * FROM slg.lgs", sqlCommand.CommandText);
        }

        [TestMethod]
        public void Test_MSSQLGen_Empty_Only_Spaces()
        {
            var sqlCommand = SQE.GenerateCommand(new MSSQLGenerator("slg.lgs"), "  ");
            Assert.IsNotNull(sqlCommand);
            Assert.AreEqual("SELECT * FROM slg.lgs", sqlCommand.CommandText);
        }
    }
}
