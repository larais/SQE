using Antlr4.Runtime.Tree;

namespace SQE.CSharp
{
    public interface IQueryGenerator<T>
    {
        string ToCompareNumberExp(ITerminalNode prop, ITerminalNode op, ITerminalNode number);

        string ToCompareStringExp(ITerminalNode prop, ITerminalNode op, ITerminalNode number);

        T GetResult();
    }
}
