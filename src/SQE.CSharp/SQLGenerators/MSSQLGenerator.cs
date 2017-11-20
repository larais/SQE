using System;
using Antlr4.Runtime.Tree;

namespace SQE.CSharp.SQLGenerators
{
    public class MSSQLGenerator : IQueryGenerator<System.String>
    {
        public string GetResult()
        {
            throw new NotImplementedException();
        }

        public string ToCompareNumberExp(ITerminalNode prop, ITerminalNode op, ITerminalNode number)
        {
            throw new NotImplementedException();
        }

        public string ToCompareStringExp(ITerminalNode prop, ITerminalNode op, ITerminalNode number)
        {
            throw new NotImplementedException();
        }
    }
}
