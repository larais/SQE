using Antlr4.Runtime.Misc;

namespace SQE.CSharp
{
    public class AbstractTreeVisitor<TReturn> : SQEBaseVisitor<string> where TReturn : class
    {
        private IQueryGenerator<TReturn> generator;

        public AbstractTreeVisitor(IQueryGenerator<TReturn> generator)
        {
            this.generator = generator;
        }

        public override string VisitExpression([NotNull] SQEParser.ExpressionContext context)
        {
            var left = Visit(context.mainExpr());

            return generator.VisitMainExp(left as TReturn);
        }

        public override string VisitAndExp([NotNull] SQEParser.AndExpContext context)
        {
            var left = Visit(context.mainExpr(0));
            var right = Visit(context.mainExpr(1));

            return generator.CombineAndExp(left as TReturn, right as TReturn);
        }

        public override string VisitOrExp([NotNull] SQEParser.OrExpContext context)
        {
            var left = Visit(context.mainExpr(0));
            var right = Visit(context.mainExpr(1));

            return generator.CombineOrExp(left as TReturn, right as TReturn);
        }

        public override string VisitParenthesisExp([NotNull] SQEParser.ParenthesisExpContext context)
        {
            var content = Visit(context.mainExpr());
            
            return generator.NestedExp(content as TReturn);
        }

        public override string VisitCompareNumberExp([NotNull] SQEParser.CompareNumberExpContext context)
        {
            var property = context.PROPERTY();
            var op = context.OPERATOR();
            var number = context.NUMBER();

            return generator.ToCompareNumberExp(property, op, number);
        }

        public override string VisitCompareStringExp([NotNull] SQEParser.CompareStringExpContext context)
        {
            var property = context.PROPERTY();
            var op = context.OPERATOR();
            var number = context.ESCAPEDSTRING();

            return generator.ToCompareStringExp(property, op, number);
        }
    }
}
