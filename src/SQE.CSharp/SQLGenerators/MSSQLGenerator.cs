using System;
using Antlr4.Runtime.Tree;
using System.Data.SqlClient;

namespace SQE.CSharp.SQLGenerators
{
    public class MSSQLGenerator : IQueryGenerator<SqlCommand>
    {
        public SqlCommand Command { get; private set; }

        public String VisitMainExp(string mainExpression)
        {
            Command.CommandText = "SELECT * FROM TABLENAME WHERE (@FilterExpr)";
            //return $"({mainExpression})";
        }

        public String NestedExp(string content)
        {
            return $"({content})";
        }

        public string CombineAndExp(string left, string right)
        {
            return $"({left}) AND ({right})";  
        }

        public String CombineOrExp(string left, string right)
        {
            return $"({left}) OR ({right})";
        }
        
        public String ToCompareNumberExp(ITerminalNode property, ITerminalNode op, ITerminalNode number)
        {
            return $"{property} {op} {number}";
        }

        public String ToCompareStringExp(ITerminalNode property, ITerminalNode op, ITerminalNode number)
        {
            return $"{property} {op} {number}";
        }

        public SqlCommand GetResult()
        
            return Command;
            throw new NotImplementedException();
        }
    }
}
