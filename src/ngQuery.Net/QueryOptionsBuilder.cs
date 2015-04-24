using Newtonsoft.Json;
using ngQuery.Net.ExpressionGeneration;
using ngQuery.Net.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ngQuery.Net
{
    public class QueryOptionsBuilder : IQueryOptionsBuilder
    {
        private readonly IOperatorParser _operatorParser;

        public QueryOptionsBuilder() : this(new OperatorParser()) { }

        internal QueryOptionsBuilder(IOperatorParser operatorParser)
        {
            _operatorParser = operatorParser;
        }

        public string Build<T>() where T : class
        {
            var type = typeof(T);
            var validOperators = new ValidOperators().GetValid();
            var topLevelOperators = validOperators.Where(x => x.IsTopLevelOperator).ToArray();
            var fieldLevelOperators = validOperators.Where(x => !x.IsTopLevelOperator).ToArray();


            var result = new QueryOptions
            {
                TopLevelOperators = topLevelOperators.Select(op => new Operator { DisplayText = op.Display, SystemIdentifier = op.Name }).ToArray(),
                Identifiers = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(prop => new Identifier
                {
                    Type = FindFriendlyType(prop.PropertyType),
                    Display = FindDisplayName(prop),
                    ValidOperators = fieldLevelOperators.Where(op => TypeHelpers.CheckThatTypeSupportsOperator(prop.PropertyType, _operatorParser.Parse(op.Name)))
                                                        .Select(op => new Operator { DisplayText = op.Display, SystemIdentifier = op.Name }).ToArray()
                }).ToArray()
            };

            return JsonConvert.SerializeObject(result);
        }

        private string FindFriendlyType(Type propertyType)
        {
            if (propertyType == typeof(DateTime))
            {
                return "date";
            }

            if (TypeHelpers.IsNumericType(propertyType))
            {
                return "numeric";
            }

            return "text";
        }

        private string FindDisplayName(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null)
            {
                return attribute.Description;
            }

            return property.Name;
        }
    }
}
