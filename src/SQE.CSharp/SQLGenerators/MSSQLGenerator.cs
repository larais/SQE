using Antlr4.Runtime.Tree;
using System.Data.SqlClient;
using System.Data;
using System;

namespace SQE.CSharp.SQLGenerators
{
    public class MSSQLGenerator : IQueryGenerator<string, SqlCommand>
    {
        public SqlCommand Command { get; private set; } = new SqlCommand();

        private void BuildXMLQueryString(ITerminalNode item)
        {
            // TODO: X.exist('//*[@key="SourceContext" and contains(.,"CSV")]') = 1
            throw new NotImplementedException();
        }

        private int CreateSqlParameter(ITerminalNode item, SqlDbType type = SqlDbType.VarChar)
        {
            var propertyCounter = Command.Parameters.Count;

            var p = new SqlParameter($"@{propertyCounter}", type);
            if (type == SqlDbType.VarChar)
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

            Command.Parameters.Add(p);

            return propertyCounter;
        }

        public string VisitMainExp(string mainExpression)
        {
            Setup();

            var query = $"SELECT * " +
                $"FROM serilog.Logs " +
                $"CROSS APPLY (SELECT CAST(Properties AS XML)) AS X(X) " + 
                $"WHERE {mainExpression};";
            Command.CommandText = query;

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
            var valueParam = CreateSqlParameter(number, SqlDbType.Int);

            return $"({property.ToString()} {op} @{valueParam})";
        }

        public string ToCompareStringExp(ITerminalNode property, ITerminalNode op, ITerminalNode escapedString)
        {
            var valueParam = CreateSqlParameter(escapedString);

            return $"({property.ToString()} {op} @{valueParam})";
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
