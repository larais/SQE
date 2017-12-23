using Antlr4.Runtime;
using System;
using System.IO;

namespace SQE
{
    public class PrimitiveErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        public event EventHandler<ErrorEventArgs> Error;

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (Error != null)
            {
                Error(this, new ErrorEventArgs { Error = e != null ? e : new Exception("ParserError" + e) });
            }
            else
            {
                throw new Exception("ParserError" + e);
            }
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (Error != null)
            {
                Error(this, new ErrorEventArgs { Error = e != null ? e : new Exception("LexerError" + e) });
            }
            else
            {
                throw new Exception("LexerError" + e);
            }
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public Exception Error { get; set; }
    }
}
