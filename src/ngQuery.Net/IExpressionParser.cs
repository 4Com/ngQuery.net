using System;
using System.Linq.Expressions;

namespace ngQuery.Net
{
    public interface IExpressionParser<TEntity> where TEntity : class, new()
    {
        Expression<Func<TEntity, bool>> GenerateExpression(string json);
    }
}
