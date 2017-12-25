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
            var errorListener = new PrimitiveErrorListener();
            if (errorEventHandler != null)
            {
                errorListener.Error += errorEventHandler;
            }
            parser.AddErrorListener(errorListener);
            lexer.AddErrorListener(errorListener);
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
        //[DataRow(" 13PropertyName = 3 ")]
        [DataRow(" PropertyName13 = 3 ")]
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
        //[DataRow("13NumberPropertyName = \"Text\"")]
        [DataRow("NumberPropertyName13 = \"Text\"")]
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
        [DataRow("( PropertyName = \"Text\"")]
        [DataRow("(PropertyName = \"Text\"")]
        public void Test_Property_Equals_String_Expression_Wrong_Start(string input)
        {
            Assert.IsTrue(ThrowsError(input));
        }

        [DataTestMethod]
        [DataRow("PropertyName = \"Text\" test")]
        [DataRow("PropertyName = \"Text\"\"test")]
        [DataRow("PropertyName = \"Text\"test\" ")]
        [DataRow("PropertyName = \"Text\" test \"")]
        [DataRow("PropertyName = \"Text\" \"test\" ")]
        [DataRow("PropertyName = \"Text\"\"test\"")]
        [DataRow("PropertyName = \"Text\")")]
        [DataRow("PropertyName = \"Text\" )")]
        public void Test_Property_Equals_String_Expression_Wrong_End(string input)
        {
            Assert.IsTrue(ThrowsError(input));
        }

        [DataTestMethod]
        [DataRow("(PropertyName = \"Text\")")]
        [DataRow("(PropertyName = \"Text\" and AnotherProp = 3)")]
        [DataRow("(PropertyName = \"Text\") and (AnotherProp = 3)")]
        [DataRow("(PropertyName = \"Text\" or AnotherProp = 3)")]
        [DataRow("(PropertyName = \"Text\") or (AnotherProp = 3)")]
        public void Test_Braces(string input)
        {
            Assert.IsFalse(ThrowsError(input));
        }

        [DataTestMethod]
        [DataRow("()")]
        [DataRow("PropertyName = \"Text\")")]
        [DataRow("(PropertyName = \"Text\"")]
        [DataRow("PropertyName = (\"Text\"")]
        [DataRow("PropertyName) = \"Text\"")]
        [DataRow("(PropertyName) = \"Text\"")]
        [DataRow("PropertyName = (\"Text\")")]
        [DataRow("Prope(r)tyName = \"Text\"")]

        [DataRow(")(")]
        [DataRow("PropertyName = \"Text\"(")]
        [DataRow(")PropertyName = \"Text\"")]
        [DataRow("PropertyName = )\"Text\"")]
        [DataRow("PropertyName( = \"Text\"")]
        [DataRow(")PropertyName( = \"Text\"")]
        [DataRow("PropertyName = )\"Text\"(")]
        public void Test_Braces_Single_Expression_Wrong(string input)
        {
            Assert.IsTrue(ThrowsError(input));
        }

        [DataTestMethod]
        [DataRow("(PropertyName = \"Text\" and AnotherProp = 3")]
        [DataRow("PropertyName = \"Text\") and (AnotherProp = 3")]
        [DataRow("(PropertyName = \"Text\" or AnotherProp) = 3")]
        [DataRow("PropertyName = (\"Text\" or AnotherProp = 3)")]
        public void Test_Braces_Multi_Expression_Wrong(string input)
        {
            Assert.IsTrue(ThrowsError(input));
        }

        [DataTestMethod]
        [DataRow("PropertyName = \"(Text)\"", "\"(Text)\"")]
        [DataRow("PropertyName = \"T)ex(t)\"", "\"T)ex(t)\"")]
        public void Test_Braces_As_Content(string input, string expectedPropValue)
        {
            var cts = (CommonTokenStream)Setup(input).InputStream;

            var property = cts.Get(2);

            Assert.AreEqual(SQELexer.ESCAPEDSTRING, property.Type);
            Assert.AreEqual(expectedPropValue, property.Text);
        }

        [DataTestMethod]
        [DataRow("PropertyOne = 1 and PropertyTwo = 2")]
        [DataRow("PropertyOne = 1 anD PropertyTwo = 2")]
        [DataRow("PropertyOne = 1 aND PropertyTwo = 2")]
        [DataRow("PropertyOne = 1 AND PropertyTwo = 2")]
        [DataRow("PropertyOne = 1 AnD PropertyTwo = 2")]
        public void Test_Logical_Operator_And_UpperLowerCase(string input)
        {
            Assert.IsFalse(ThrowsError(input));
        }

        [DataTestMethod]
        [DataRow("PropertyOne = 1 or PropertyTwo = 2")]
        [DataRow("PropertyOne = 1 oR PropertyTwo = 2")]
        [DataRow("PropertyOne = 1 Or PropertyTwo = 2")]
        [DataRow("PropertyOne = 1 OR PropertyTwo = 2")]
        public void Test_Logical_Operator_Or_UpperLowerCase(string input)
        {
            Assert.IsFalse(ThrowsError(input));
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("  ")]
        [DataRow(" ")]
        [DataRow("        ")]
        public void Test_Empty(string input)
        {
            Assert.IsFalse(ThrowsError(input));
        }

        private bool ThrowsError(string input)
        {
            var throwsError = false;

            var parser = Setup(input, (sender, args) =>
            {
                if (args.Error != null)
                {
                    throwsError = true;
                }
            });

            return throwsError;
        }
    }
}
