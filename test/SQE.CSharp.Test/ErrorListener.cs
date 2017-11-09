using Antlr4.Runtime;
using System;
using System.IO;

namespace SQE.CSharp.Test
{
    public class ErrorListener : BaseErrorListener
    {
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new Exception("SyntaxError" + e);
        }
    }
}
