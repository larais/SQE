using Antlr4.Runtime;
using SQE.CSharp.SQLGenerators;
using System;
using System.Data.SqlClient;

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

                var mssqlQueryGenerator = new MSSQLGenerator();

                var cmd = GenerateCommand(mssqlQueryGenerator, input);

                Console.WriteLine(cmd);

                //if (IsValid(input))
                //{
                //    using (var connection = new SqlConnection(""))
                //    {
                //        connection.Open();

                //        var mssqlQueryGenerator = new MSSQLGenerator();

                //        var cmd = GenerateCommand(mssqlQueryGenerator, input);

                //        cmd.ExecuteNonQueryAsync();
                //    }
                //}
            }
        }

        private static TReturn GenerateCommand<TReturn>(IQueryGenerator<TReturn> qg, string input) where TReturn : class
        {
            //TODO: Move Lexing & Parsing to earlier stage.

            AntlrInputStream inputStream = new AntlrInputStream(input);
            var lexer = new SQELexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            var parser = new SQEParser(commonTokenStream);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(new PrimitiveErrorListener());
            
            SQEParser.ExpressionContext expressionContext = parser.expression();
            Console.WriteLine("Tree expression context: " + expressionContext.ToStringTree());

            var visitor = new AbstractTreeVisitor<TReturn>(qg);
            return visitor.Visit(expressionContext) as TReturn;

           // return qg.GetResult();
        }

        private static bool IsValid(string intpusd)
        {
            throw new NotImplementedException();
        }
    }
}
