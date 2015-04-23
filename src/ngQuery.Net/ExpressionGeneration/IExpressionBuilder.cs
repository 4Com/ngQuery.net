using ngQuery.Net.Models;
using System;
using System.Linq.Expressions;

namespace ngQuery.Net.ExpressionGeneration
{
    internal interface IExpressionBuilder
    {
        Expression<Func<T, bool>> BuildExpression<T>(IRuleExpression ruleExpression) where T : class, new();
    }
}
