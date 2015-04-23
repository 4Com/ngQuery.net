using Newtonsoft.Json;
using ngQuery.Net.ExpressionGeneration;
using ngQuery.Net.JsonParsing;
using ngQuery.Net.Models;
using System;
using System.Linq.Expressions;

namespace ngQuery.Net
{
    public sealed class ExpressionParser : IExpressionParser
    {
        private readonly IExpressionBuilder _expressionBuilder;

        public ExpressionParser() : this(new ExpressionBuilder()) { }

        internal ExpressionParser(IExpressionBuilder expressionBuilder)
        {
            if (expressionBuilder == null)
                throw new ArgumentNullException(nameof(expressionBuilder));

            _expressionBuilder = expressionBuilder;
        }

        public Expression<Func<T, bool>> GenerateExpression<T>(IRuleExpression ruleExpression) where T : class, new()
        {
            if (ruleExpression == null)
                throw new ArgumentNullException(nameof(ruleExpression));

            return _expressionBuilder.BuildExpression<T>(ruleExpression);
        }

        public Expression<Func<T, bool>> GenerateExpression<T>(string json) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            var ruleExpression = JsonConvert.DeserializeObject<IRuleExpression>(json, new RuleExpressionConverter());

            return GenerateExpression<T>(ruleExpression);
        }
    }
}
