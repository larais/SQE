﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQE.SQLGenerators;
using System;

namespace SQE.CSharp.UnitTests
{
    [TestClass]
    public class MSSQLGeneratorTests
    {
        [TestMethod]
        public void MSSQLGen_Empty()
        {
            var sqlCommand = SQE.GenerateCommand(new MSSQLGenerator("slg.lgs"), string.Empty);
            Assert.IsNotNull(sqlCommand);
            Assert.AreEqual("SELECT * FROM slg.lgs ORDER BY Id", sqlCommand.CommandText);
        }

        [TestMethod]
        public void MSSQLGen_Empty_Only_Spaces()
        {
            var sqlCommand = SQE.GenerateCommand(new MSSQLGenerator("slg.lgs"), "  ");
            Assert.IsNotNull(sqlCommand);
            Assert.AreEqual("SELECT * FROM slg.lgs ORDER BY Id", sqlCommand.CommandText);
        }

        [DataTestMethod]
        [DataRow("CustomProp = 3")]
        [DataRow("CustomProp = \"Three\"")]
        [ExpectedException(typeof(NotImplementedException))]
        public void MSSQLGen_CrossApplyProperties(string query)
        {
            var sqlCommand = SQE.GenerateCommand(new MSSQLGenerator("slg.lgs"), query);
            Assert.IsNotNull(sqlCommand);
            Assert.IsTrue(sqlCommand.CommandText.StartsWith("SELECT * FROM slg.lgs CROSS APPLY (SELECT CAST(Properties AS XML)) AS X(X) WHERE"));
        }

        [DataTestMethod]
        [DataRow("Id = 3")]
        [DataRow("Id = \"Three\"")]
        public void MSSQLGen_NoCustomProperties(string query)
        {
            var sqlCommand = SQE.GenerateCommand(new MSSQLGenerator("slg.lgs"), query);
            Assert.IsNotNull(sqlCommand);
            Assert.IsTrue(sqlCommand.CommandText.StartsWith("SELECT * FROM slg.lgs WHERE"));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void MSSQLGen_Paging(int page)
        {
            int pageSize = 25;
            var sqlCommand = SQE.GenerateCommand(new MSSQLGenerator("slg.lgs", paginationWindowSize: pageSize, usePaging: true), string.Empty, paginationOffset: page);
            Assert.IsNotNull(sqlCommand);
            Assert.IsTrue(sqlCommand.CommandText == $"SELECT * FROM slg.lgs ORDER BY Id OFFSET { (page - 1) * pageSize } ROWS FETCH NEXT 25 ROWS ONLY;");
        }
    }
}
