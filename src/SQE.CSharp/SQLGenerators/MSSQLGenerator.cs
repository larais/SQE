using Antlr4.Runtime.Tree;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;

namespace SQE.SQLGenerators
{
    public class MSSQLGenerator : IQueryGenerator<string, SqlCommand>
    {
        private bool queryContainsProperty;

        public SqlCommand Command { get; private set; } = new SqlCommand();

        public ICollection<string> SqlSchemaColumns { get; private set; }

        public string PropertyColumn { get; set; }

        public string LogTable { get; set; }

        public bool UsePaging { get; set; }

        public int PaginationWindowSize { get; set; }

        public int PaginationOffset { get; set; } = 1;

        public MSSQLGenerator(string logTable = "Logs", string propertyColumn = "Properties", ICollection<string> sqlSchemaColumns = null, int paginationWindowSize = 25, bool usePaging = false)
        {
            LogTable = logTable;
            PropertyColumn = propertyColumn;
            SqlSchemaColumns = sqlSchemaColumns ?? new[] { "Id", "Message", "MessageTemplate", "Level", "TimeStamp", "Exception", "Properties" };
            PaginationWindowSize = paginationWindowSize;
            UsePaging = usePaging;
        }

        public void Initialize()
        {
            queryContainsProperty = false;
        }

        public string VisitMainExp(string mainExpression)
        {
            var query =
                    $"SELECT * " +
                    $"FROM {LogTable}";

            if (!string.IsNullOrEmpty(mainExpression))
            {
                if (queryContainsProperty)
                {
                    query += $" CROSS APPLY (SELECT CAST({PropertyColumn} AS XML)) AS X(X)";
                }

                query += $" WHERE {mainExpression} ";
            }

            query += $" ORDER BY Id";

            if (UsePaging)
            {
                query += $" OFFSET {(PaginationOffset - 1) * PaginationWindowSize} ROWS FETCH NEXT {PaginationWindowSize} ROWS ONLY;";
            }

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
            var propertyString = property.ToString();

            if (SqlSchemaColumns.Contains(propertyString))
            {
                var valueParam = CreateSqlParameter(number, SqlDbType.Int);
                return $"({property.ToString()} {op} @{valueParam})";
            }
            else
            {
                queryContainsProperty = true;
                throw new NotImplementedException();
            }
        }

        public string ToCompareStringExp(ITerminalNode property, ITerminalNode op, ITerminalNode escapedString)
        {
            var propertyString = property.ToString();
            var valueParam = CreateSqlParameter(escapedString);

            if (SqlSchemaColumns.Contains(propertyString))
            {
                return $"({property.ToString()} {op} @{valueParam})";
            }
            else
            {
                queryContainsProperty = true;
                throw new NotImplementedException();

                // TODO: SqlParameter, Different Operators
                var propParam = CreateSqlParameter(property);
                return $"(X.exist('//*[@key=\"@{propParam}\" and contains(.,\"@{valueParam}\")]') = 1)";
            }
        }

        public SqlCommand GetResult()
        {
            return Command;
        }

        private void BuildXMLQueryString(ITerminalNode item)
        {
            throw new NotImplementedException();

            // TODO: (X.exist('//*[@key="SourceContext" and contains(.,"CSV")]') = 1)
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
    }
}
