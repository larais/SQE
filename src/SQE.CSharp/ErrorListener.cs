using Antlr4.Runtime;
using System;
using System.IO;

namespace SQE.CSharp
{
    public class ErrorListener : BaseErrorListener
    {
        public event EventHandler<ErrorEventArgs> Error;

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (Error != null)
            {
                Error(this, new ErrorEventArgs { Error = e != null ? e : new Exception("SyntaxError" + e) });
            }
            else
            {
                throw new Exception("SyntaxError" + e);
            }
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public Exception Error { get; set; }
    }
}
