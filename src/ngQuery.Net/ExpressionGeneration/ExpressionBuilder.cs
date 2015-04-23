using System;
using System.Linq.Expressions;
using ngQuery.Net.Models;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

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
                if (ruleExpression == null)
                    throw new ArgumentNullException(nameof(ruleExpression));

                if (entityExpression == null)
                    throw new ArgumentNullException(nameof(entityExpression));

                var rule = ruleExpression as IRule;
                if (rule != null)
                    return BuildRule(rule, entityExpression);

                var group = ruleExpression as IRuleGroup;
                if (group != null)
                    return BuildRuleGroup(group, entityExpression);

                throw new NotSupportedException(string.Format("The type of IRuleExpression '{0}' is not supported by this builder.", ruleExpression.GetType().Name));
            }

            private BinaryExpression BuildRule(IRule rule, ParameterExpression entityExpression)
            {
                var operand = _operatorParser.Parse(rule.SelectedOperator);
                var property = Expression.PropertyOrField(entityExpression, rule.SelectedField);
                var value = Expression.Constant(Convert.ChangeType(rule.SelectedEntry, GetMemberType(property.Member)));

                switch (operand)
                {
                    case OperatorEnum.Equals:
                        return Expression.Equal(property, value);
                    case OperatorEnum.NotEquals:
                        return Expression.NotEqual(property, value);
                    case OperatorEnum.GreaterThan:
                        return Expression.GreaterThan(property, value);
                    case OperatorEnum.GreaterThanOrEqualTo:
                        return Expression.GreaterThanOrEqual(property, value);
                    default:
                        throw new NotSupportedException(String.Format("The operator '{0}' is not currently supported", rule.SelectedOperator));
                }
            }

            private BinaryExpression BuildRuleGroup(IRuleGroup group, ParameterExpression entityExpression)
            {
                if(group.List.Count == 0)
                    return Expression.OrElse(Expression.Constant(true), Expression.Constant(true));

                var groupExpressions = group.List.Select(r => Build(r, entityExpression)).ToList();
                var operand = _operatorParser.Parse(group.SelectedTopOperator);

                if (operand == OperatorEnum.And)
                {
                    return CombineExpressions(groupExpressions, Expression.AndAlso);
                }
                else if (operand == OperatorEnum.Or)
                {
                    return CombineExpressions(groupExpressions, Expression.OrElse);
                }

                throw new NotSupportedException("Only the following operators are supported: And, Or");
            }

            private BinaryExpression CombineExpressions(IEnumerable<BinaryExpression> expressions, Func<Expression, Expression, BinaryExpression> combiner)
            {
                return expressions.Aggregate((left, right) => combiner(left, right));
            }

            private Type GetMemberType(MemberInfo member)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Event:
                        return ((EventInfo)member).EventHandlerType;
                    case MemberTypes.Field:
                        return ((FieldInfo)member).FieldType;
                    case MemberTypes.Method:
                        return ((MethodInfo)member).ReturnType;
                    case MemberTypes.Property:
                        return ((PropertyInfo)member).PropertyType;
                    default:
                        throw new ArgumentException("Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
                }
            }
        }
    }
}
