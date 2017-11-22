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

                var expressionContext = ProcessInput(input);
                Console.WriteLine("Tree expression context: " + expressionContext.ToStringTree());

                var mssqlQueryGenerator = new MSSQLGenerator();

                var sqlCommand = GenerateCommand(mssqlQueryGenerator, expressionContext);

                if (IsValid(input))
                {
                    using (var connection = new SqlConnection(@""))
                    {
                        connection.Open();

                        sqlCommand.Connection = connection;
                        sqlCommand.Prepare();
                        var reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            Console.WriteLine($"RowID: {reader[0]}");
                        }
                    }
                }
            }
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

        private static TResult GenerateCommand<TReturn, TResult>(IQueryGenerator<TReturn, TResult> qg, SQEParser.ExpressionContext expressionContext) where TReturn : class
        {
            var visitor = new AbstractTreeVisitor<TReturn, TResult>(qg);
            visitor.Visit(expressionContext);

            return qg.GetResult();
        }

        private static bool IsValid(string intpusd)
        {
            return true; //HACK:

            // TODO: Call ProcessInput, verify no Exception is Thrown

            throw new NotImplementedException();
        }
    }
}
