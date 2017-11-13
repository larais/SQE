using Antlr4.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SQE.CSharp.UnitTests
{
    [TestClass]
    public class SQEGrammarTests
    {
        private SQEParser Setup(string input, EventHandler<ErrorEventArgs> errorEventHandler = null)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            var lexer = new SQELexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            var parser = new SQEParser(commonTokenStream);
            var errorListener = new ErrorListener();
            if (errorEventHandler != null)
            {
                errorListener.Error += errorEventHandler;
            }
            parser.AddErrorListener(errorListener);
            var expressionContext = parser.expression();
            return parser;
        }

        [DataTestMethod]
        [DataRow("PropertyName = 3")]
        [DataRow("PropertyName=3")]
        [DataRow("PropertyName= 3")]
        [DataRow(" PropertyName = 3")]
        [DataRow("PropertyName = 3 ")]
        [DataRow(" PropertyName = 3 ")]
        public void Test_Isolated_Property_Equals_Number_Expression(string input)
        {
            var cts = (CommonTokenStream)Setup(input).InputStream;
            var size = cts.Size - 1; // Off by one due to EOF token

            Assert.AreEqual(size, 3);
            Assert.AreEqual(SQELexer.PROPERTY, cts.Get(0).Type);
            Assert.AreEqual("=", cts.Get(1).Text);
            Assert.AreEqual(SQELexer.NUMBER, cts.Get(2).Type);
        }

        [DataTestMethod]
        [DataRow("PropertyName = \"Text\"")]
        [DataRow("PropertyName=\"Text\"")]
        [DataRow("PropertyName= \"Text\"")]
        [DataRow(" PropertyName= \"Text\"")]
        [DataRow("PropertyName= \"Text\" ")]
        [DataRow(" PropertyName= \"Text\" ")]
        public void Test_Isolated_Property_Equals_String_Expression(string input)
        {
            var cts = (CommonTokenStream)Setup(input).InputStream;
            var size = cts.Size - 1; // Off by one due to EOF token

            Assert.AreEqual(size, 3);
            Assert.AreEqual(SQELexer.PROPERTY, cts.Get(0).Type);
            Assert.AreEqual("=", cts.Get(1).Text);
            Assert.AreEqual(SQELexer.ESCAPEDSTRING, cts.Get(2).Type);
        }

        [DataTestMethod]
        [DataRow("test PropertyName = \"Text\"")]
        [DataRow("\"test PropertyName = \"Text\"")]
        [DataRow("test\" PropertyName = \"Text\"")]
        [DataRow("test \"PropertyName = \"Text\"")]
        [DataRow("\"test\" PropertyName = \"Text\"")]
        [DataRow("\"test\"PropertyName = \"Text\"")]
        public void Test_Property_Equals_String_Expression_Wrong_Start(string input)
        {
            var throwsError = false;

            var parser = Setup(input, (sender, args) =>
            {
                if (args.Error != null)
                {
                    throwsError = true;
                }
            });

            Assert.IsTrue(throwsError);
        }

        [DataTestMethod]
        [DataRow("PropertyName = \"Text\" test")]
        [DataRow("PropertyName = \"Text\"\"test")]
        [DataRow("PropertyName = \"Text\"test\" ")]
        [DataRow("PropertyName = \"Text\" test \"")]
        [DataRow("PropertyName = \"Text\" \"test\" ")]
        [DataRow("PropertyName = \"Text\"\"test\"")]
        public void Test_Property_Equals_String_Expression_Wrong_End(string input)
        {
            var throwsError = false;

            var parser = Setup(input, (sender, args) =>
            {
                if (args.Error != null)
                {
                    throwsError = true;
                }
            });

            Assert.IsTrue(throwsError);
        }
    }
}
