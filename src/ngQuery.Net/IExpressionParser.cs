using ngQuery.Net.Models;
using System;
using System.Linq.Expressions;

namespace ngQuery.Net
{
    public interface IExpressionParser
    {
        Expression<Func<TEntity, bool>> GenerateExpression<TEntity>(string json) where TEntity : class, new();
        Expression<Func<TEntity, bool>> GenerateExpression<TEntity>(IRuleExpression ruleExpression) where TEntity : class, new();
    }
}
