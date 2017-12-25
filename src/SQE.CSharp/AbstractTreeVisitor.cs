using Antlr4.Runtime.Misc;

namespace SQE
{
    public class AbstractTreeVisitor<TReturn, TResult> : SQEBaseVisitor<TReturn> where TReturn : class
    {
        private IQueryGenerator<TReturn, TResult> generator;

        public AbstractTreeVisitor(IQueryGenerator<TReturn, TResult> generator)
        {
            this.generator = generator;
        }

        public override TReturn VisitExpression([NotNull] SQEParser.ExpressionContext context)
        {
            var mex = context.mainExpr();
            TReturn mainExpression = mex != null ? Visit(mex) : null;
            
            return generator.VisitMainExp(mainExpression);
        }

        public override TReturn VisitAndExp([NotNull] SQEParser.AndExpContext context)
        {
            var left = Visit(context.mainExpr(0));
            var right = Visit(context.mainExpr(1));

            return generator.CombineAndExp(left, right);
        }

        public override TReturn VisitOrExp([NotNull] SQEParser.OrExpContext context)
        {
            var left = Visit(context.mainExpr(0));
            var right = Visit(context.mainExpr(1));

            return generator.CombineOrExp(left, right);
        }

        public override TReturn VisitParenthesisExp([NotNull] SQEParser.ParenthesisExpContext context)
        {
            var content = Visit(context.mainExpr());
            
            return generator.NestedExp(content);
        }

        public override TReturn VisitCompareNumberExp([NotNull] SQEParser.CompareNumberExpContext context)
        {
            var property = context.PROPERTY();
            var op = context.OPERATOR();
            var number = context.NUMBER();

            return generator.ToCompareNumberExp(property, op, number);
        }

        public override TReturn VisitCompareStringExp([NotNull] SQEParser.CompareStringExpContext context)
        {
            var property = context.PROPERTY();
            var op = context.OPERATOR();
            var escapedString = context.ESCAPEDSTRING();

            return generator.ToCompareStringExp(property, op, escapedString);
        }
    }
}
