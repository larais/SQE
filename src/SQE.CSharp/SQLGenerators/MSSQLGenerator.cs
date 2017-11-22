using Antlr4.Runtime.Tree;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace SQE.CSharp.SQLGenerators
{
    public class MSSQLGenerator : IQueryGenerator<string, SqlCommand>
    {
        public SqlCommand Command { get; private set; } = new SqlCommand();

        private List<SqlParameter> PropertiesContainer = new List<SqlParameter>();
        private int PropertyCounter { get; set; } = 0;

        private int CreateSqlParameter(ITerminalNode item, SqlDbType type = SqlDbType.VarChar)
        {
            PropertyCounter++;
            var p = new SqlParameter($"@{PropertyCounter}", type);
            if(type == SqlDbType.VarChar)
            {
                var trimmedString = item.ToString().Trim('"');
                p.Value = trimmedString;
                p.Size = trimmedString.Length;
            }
            else
            {
                //Assert Int
                p.Value = int.Parse(item.ToString());
            }

            PropertiesContainer.Add(p);

            return PropertyCounter;
        }

        public string VisitMainExp(string mainExpression)
        {
            Setup();

            var query = $"SELECT * FROM serilog.Logs WHERE {mainExpression};";
            Command.CommandText = query;
            Command.Parameters.AddRange(PropertiesContainer.ToArray());

            return query;
        }

        public string NestedExp(string content)
        {
            return $"({content})";
        }

        public string CombineAndExp(string left, string right)
        {
            return $"({left}) AND ({right})";  
        }

        public string CombineOrExp(string left, string right)
        {
            return $"({left}) OR ({right})";
        }
        
        public string ToCompareNumberExp(ITerminalNode property, ITerminalNode op, ITerminalNode number)
        {
            var propParam = CreateSqlParameter(property);
            var valueParam = CreateSqlParameter(number, SqlDbType.Int);

            return $"@{propParam} {op} @{valueParam}";
        }

        public string ToCompareStringExp(ITerminalNode property, ITerminalNode op, ITerminalNode escapedString)
        {
            var propParam = CreateSqlParameter(property);
            var valueParam = CreateSqlParameter(escapedString);

            return $"@{propParam} {op} @{valueParam}";
        }

        public SqlCommand GetResult()
        {
            return Command;
        }

        public void Setup()
        {
        }
    }
}
