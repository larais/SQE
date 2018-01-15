using SQE.SQLGenerators;
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

                if (SQE.IsValidSyntax(input))
                {
                    var mssqlQueryGenerator = new MSSQLGenerator(logTable: "serilog.Logs");
                    var sqlCommand = SQE.GenerateCommand(mssqlQueryGenerator, input, paginationOffset: 0);

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
    }
}
