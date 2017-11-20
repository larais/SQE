using Antlr4.Runtime;
using System;

namespace SQE.CSharp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter expression:" + Environment.NewLine);

                string input = input = Console.ReadLine();
                if (input == "exit")
                    break;

                if (IsValid(input))
                {
                    using (var conn = new SqlConnection())
                    {
                        connection.Open();

                        var mysqlQueryGen = new MSSQLGenerator();

                        var cmd = GenerateCommand(mysqlQueryGen);

                        cmd.ExecuteQueryAsync();
                    }
                }


                DoIt(input);
            }
        }

        private static TReturn GenerateCommand<TReturn>(IQueryGenerator<TReturn> qg) where TReturn : class
        {

            AntlrInputStream inputStream = new AntlrInputStream(input);
            var lexer = new SQELexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            var parser = new SQEParser(commonTokenStream);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(new PrimitiveErrorListener());
            
            SQEParser.ExpressionContext expressionContext = parser.expression();
            Console.WriteLine("Tree expression context: " + expressionContext.ToStringTree());

            var visitor = new AbstractTreeVisitor<TReturn>(qg);
            visitor.Visit(expressionContext);

            return qg.GetResult();
        }

        private static bool IsValid(string intpusd)
        {
            return false;
        }

        private static void DoIt(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            var lexer = new SQELexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            var parser = new SQEParser(commonTokenStream);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(new PrimitiveErrorListener());

            try
            {
                SQEParser.ExpressionContext expressionContext = parser.expression();
                Console.WriteLine("Tree expression context: " + expressionContext.ToStringTree());

                var visitor = new AbstractTreeVisitor();
                Console.WriteLine("Visiting tree: " + visitor.Visit(expressionContext) + Environment.NewLine);





            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
