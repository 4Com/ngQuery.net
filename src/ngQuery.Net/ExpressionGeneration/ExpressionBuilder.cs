using System;
using System.Linq.Expressions;
using ngQuery.Net.Models;
using System.Linq;

namespace ngQuery.Net.ExpressionGeneration
{
    internal class ExpressionBuilder : IExpressionBuilder
    {
        private readonly IOperatorParser _operatorParser;

        internal ExpressionBuilder() : this(new OperatorParser()) { }

        internal ExpressionBuilder(IOperatorParser operatorParser)
        {
            if (operatorParser == null)
                throw new ArgumentNullException(nameof(operatorParser));

            _operatorParser = operatorParser;
        }

        public Expression<Func<T, bool>> BuildExpression<T>(IRuleExpression ruleExpression) where T : class, new()
        {
            var builder = new Builder(_operatorParser);

            // this ensures the same type param is used throughout else the tree will throw a parameter mismatch exception.
            ParameterExpression param = Expression.Parameter(typeof(T)); 

            return BuildExpressionFromBinaryExpression<T>(builder.Build(ruleExpression, param), param);           
        }

        private static Expression<Func<T, bool>> BuildExpressionFromBinaryExpression<T>(BinaryExpression binaryExpression, params ParameterExpression[] expressions) where T : class, new()
        {
            return Expression.Lambda<Func<T, bool>>(binaryExpression, expressions);
        }

        private class Builder
        {
            private readonly IOperatorParser _operatorParser;

            internal Builder(IOperatorParser operatorParser)
            {
                _operatorParser = operatorParser;
            }

            public BinaryExpression Build(IRuleExpression ruleExpression, ParameterExpression entityExpression)
            {
                var rule = ruleExpression as IRule;
                if (rule != null)
                    return BuildRule(rule, entityExpression);

                return BuildRuleGroup(ruleExpression as IRuleGroup, entityExpression);
            }

            private BinaryExpression BuildRule(IRule rule, ParameterExpression entityExpression)
            {
                var operand = _operatorParser.Parse(rule.SelectedOperator);
                var property = Expression.PropertyOrField(entityExpression, rule.SelectedField);

                switch (operand)
                {
                    case OperatorEnum.Equals:
                        return Expression.Equal(property, Expression.Constant(rule.SelectedEntry));
                    case OperatorEnum.NotEquals:
                        return Expression.NotEqual(property, Expression.Constant(rule.SelectedEntry));
                    default:
                        throw new NotSupportedException(String.Format("The operator '{0}' is not currently supported", rule.SelectedOperator));
                }
            }

            private BinaryExpression BuildRuleGroup(IRuleGroup group, ParameterExpression entityExpression)
            {
                var groupExpressions = group.List.Select(r => Build(r, entityExpression)).ToList();
                var operand = _operatorParser.Parse(group.SelectedTopOperator);

                if (operand == OperatorEnum.And)
                {
                    return groupExpressions.Aggregate((left, right) => Expression.AndAlso(left, right));
                }
                else if (operand == OperatorEnum.Or)
                {
                    BinaryExpression binaryExpression = null;
                    for (var current = 0; (current + 1) < groupExpressions.Count; current++)
                    {
                        var next = current + 1;
                        var thisExpression = Expression.OrElse(groupExpressions[current], groupExpressions[next]);

                        binaryExpression = (binaryExpression == null) ? thisExpression : Expression.OrElse(binaryExpression, thisExpression);
                    }

                    return binaryExpression ?? Expression.OrElse(Expression.Constant(true), Expression.Constant(true));
                }

                throw new NotSupportedException("Only the following operators are supported: And, Or");
            }
        }
    }
}
