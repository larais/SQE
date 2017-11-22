using Antlr4.Runtime.Tree;

namespace SQE.CSharp
{
    public interface IQueryGenerator<T>
    {
        string VisitMainExp(T left);

        string NestedExp(T content);

        string CombineAndExp(T left, T right);

        string CombineOrExp(T left, T right);

        string ToCompareNumberExp(ITerminalNode property, ITerminalNode op, ITerminalNode number);

        string ToCompareStringExp(ITerminalNode property, ITerminalNode op, ITerminalNode number);

        T GetResult();
    }
}
