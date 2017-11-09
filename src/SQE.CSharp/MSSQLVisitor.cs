using Antlr4.Runtime.Misc;

namespace SQE.CSharp
{
    public class MSSQLVisitor : SQEBaseVisitor<string>
    {
        public override string VisitExpression([NotNull] SQEParser.ExpressionContext context)
        {
            var left = Visit(context.mainExpr());
            return $"({left})";
        }

        public override string VisitAndExp([NotNull] SQEParser.AndExpContext context)
        {
            var left = Visit(context.mainExpr(0));
            var right = Visit(context.mainExpr(1));

            return $"({left}) AND ({right})";
        }

        public override string VisitOrExp([NotNull] SQEParser.OrExpContext context)
        {
            var left = Visit(context.mainExpr(0));
            var right = Visit(context.mainExpr(1));

            return $"({left}) OR ({right})";
        }

        public override string VisitParenthesisExp([NotNull] SQEParser.ParenthesisExpContext context)
        {
            var content = Visit(context.mainExpr());
            return $"({content})";
        }

        public override string VisitCompareNumberExp([NotNull] SQEParser.CompareNumberExpContext context)
        {
            var property = context.PROPERTY();
            var op = context.OPERATOR();
            var number = context.NUMBER();

            return $"{property} {op} {number}";
        }

        public override string VisitCompareStringExp([NotNull] SQEParser.CompareStringExpContext context)
        {
            var property = context.PROPERTY();
            var op = context.OPERATOR();
            var number = context.ESCAPEDSTRING();

            return $"{property} {op} {number}";
        }
    }
}
