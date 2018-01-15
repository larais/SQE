using Antlr4.Runtime.Tree;

namespace SQE
{
    public interface IQueryGenerator<T, TResult>
    {
        void Initialize();

        int PaginationOffset { get; set; }

        T VisitMainExp(T left);

        T NestedExp(T content);

        T CombineAndExp(T left, T right);

        T CombineOrExp(T left, T right);

        T ToCompareNumberExp(ITerminalNode property, ITerminalNode op, ITerminalNode number);

        T ToCompareStringExp(ITerminalNode property, ITerminalNode op, ITerminalNode escapedString);

        TResult GetResult();
    }
}
