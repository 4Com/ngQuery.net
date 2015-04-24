using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ngQuery.Net.ExpressionGeneration;
using ngQuery.Net.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ngQuery.Net
{
    public sealed class QueryOptionsBuilder : IQueryOptionsBuilder
    {
        private readonly IOperatorParser _operatorParser;
        private readonly IValidEntriesGenerator _validEntriesGenerator;

        public QueryOptionsBuilder() : this(new OperatorParser(), new DefaultValidEntriesGenerator()) { }

        internal QueryOptionsBuilder(IOperatorParser operatorParser, IValidEntriesGenerator validEntriesGenerator)
        {
            if (operatorParser == null)
                throw new ArgumentNullException(nameof(operatorParser));

            if (validEntriesGenerator == null)
                throw new ArgumentNullException(nameof(validEntriesGenerator));

            _operatorParser = operatorParser;
            _validEntriesGenerator = validEntriesGenerator;
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
                Fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(prop => new Identifier
                {
                    Type = FindFriendlyType(prop.PropertyType),
                    Display = FindDisplayName(prop),
                    SystemIdentifier = prop.Name,
                    ValidEntries = _validEntriesGenerator.Generate(type, prop),
                    ValidOperators = fieldLevelOperators.Where(op => TypeHelpers.CheckThatTypeSupportsOperator(prop.PropertyType, _operatorParser.Parse(op.Name)))
                                                        .Select(op => new Operator { DisplayText = op.Display, SystemIdentifier = op.Name }).ToArray()
                }).ToArray()
            };

            return JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private string FindFriendlyType(Type propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));


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
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var attribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null)
            {
                return attribute.Description;
            }

            return property.Name;
        }
    }
}
