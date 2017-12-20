using Antlr4.Runtime.Tree;

namespace SQE.CSharp
{
    public interface IQueryGenerator<T, TResult>
    {
        T VisitMainExp(T left);

        T NestedExp(T content);

        T CombineAndExp(T left, T right);

        T CombineOrExp(T left, T right);

        T ToCompareNumberExp(ITerminalNode property, ITerminalNode op, ITerminalNode number);

        T ToCompareStringExp(ITerminalNode property, ITerminalNode op, ITerminalNode escapedString);

        TResult GetResult();
    }
}
