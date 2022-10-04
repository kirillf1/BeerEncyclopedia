using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts;
using BeerEncyclopedia.Domain;
using System.Linq.Expressions;

namespace BeerEncyclopedia.Application.Specifications
{
    public static class SpecificationBuilderExtensions
    {
        public static ISpecificationBuilder<T> SkipTake<T>(this ISpecificationBuilder<T> @this, QueryBase queryBase)
            where T : class
        {
            @this.Skip(queryBase.PageIndex * queryBase.PageSize)
                .Take(queryBase.PageSize);
            return @this;
        }
        public static IOrderedSpecificationBuilder<T> SetOrderExpression<T>(this ISpecificationBuilder<T> specification, OrderBy orderBy, Expression<Func<T, object?>> funcOrder) where T : class
        {
            if (orderBy == OrderBy.ASC)
                return specification.OrderBy(funcOrder);
            return specification.OrderByDescending(funcOrder);
        }
        public static Expression<Func<T, TReturn>> JoinQueries<T, TReturn>(Func<Expression, Expression, BinaryExpression> joiner, IReadOnlyCollection<Expression<Func<T, TReturn>>> expressions)
        {
            if (!expressions.Any())
            {
                throw new ArgumentException("No expressions were provided");
            }
            var firstExpression = expressions.First();
            var otherExpressions = expressions.Skip(1);
            var firstParameter = firstExpression.Parameters.Single();
            var otherExpressionsWithParameterReplaced = otherExpressions
                .Select(e => ReplaceParameter(e.Body, e.Parameters.Single(), firstParameter));
            var bodies = new[] { firstExpression.Body }.Concat(otherExpressionsWithParameterReplaced);
            var joinedBodies = bodies.Aggregate(joiner);
            return Expression.Lambda<Func<T, TReturn>>(joinedBodies, firstParameter);
        }
        private static T ReplaceParameter<T>(T expr, ParameterExpression toReplace, ParameterExpression replacement)
    where T : Expression
        {
            var replacer = new ExpressionReplacer(e => e == toReplace ? replacement : e);
            return (T)replacer.Visit(expr);
        }
    }
    public class ExpressionReplacer : ExpressionVisitor
    {
        private readonly Func<Expression, Expression> replacer;

        public ExpressionReplacer(Func<Expression, Expression> replacer)
        {
            this.replacer = replacer;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(replacer(node));
        }
    }
}
