using Antlr4.Runtime;
using System;

namespace SQE
{
    public class SQE
    {
        public static TResult GenerateCommand<TReturn, TResult>(IQueryGenerator<TReturn, TResult> qg, string input) where TReturn : class
        {
            SQEParser.ExpressionContext expressionContext = ProcessInput(input);

            var visitor = new AbstractTreeVisitor<TReturn, TResult>(qg);
            visitor.Visit(expressionContext);

            return qg.GetResult();
        }

        public static bool IsValidSyntax(string input)
        {
            try
            {
                ProcessInput(input);
            }
            catch (Exception e)
            {
                return false;
                //TODO: We should probably log this :)
            }

            return true;
        }

        private static SQEParser.ExpressionContext ProcessInput(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            var lexer = new SQELexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            var parser = new SQEParser(commonTokenStream);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(new PrimitiveErrorListener());

            SQEParser.ExpressionContext expressionContext = parser.expression();

            return expressionContext;
        }
    }
}
