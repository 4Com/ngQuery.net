using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ngQuery.Net.Models;
using System;
using System.Linq;

namespace ngQuery.Net.JsonParsing
{
    internal class RuleExpressionConverter : JsonConverter
    {
        private const string SelectedTopLevelOperatorJsonPropertyName = "selectedTopLevelOperator";
        private const string ListJsonPropertyName = "list";
        private const string SelectedEntryJsonPropertyName = "selectedEntry";
        private const string SelectedFieldJsonPropertyName = "selectedField";
        private const string SelectedOperatorJsonPropertyName = "selectedOperator";
        private readonly IRuleExpressionFactory _ruleExpressionFactory;

        internal RuleExpressionConverter() : this(new DefaultRuleExpressionFactory()) { }

        internal RuleExpressionConverter(IRuleExpressionFactory ruleExpressionFactory)
        {
            if (ruleExpressionFactory == null)
                throw new ArgumentNullException(nameof(ruleExpressionFactory));

            _ruleExpressionFactory = ruleExpressionFactory;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IRuleExpression).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ParseObject(serializer.Deserialize<JObject>(reader), serializer);
        }

        private IRuleExpression ParseObject(JObject value, JsonSerializer serializer)
        {
            var rootProperties = value.Properties().ToArray();
            if(rootProperties.Any(x => x.Name == SelectedTopLevelOperatorJsonPropertyName))
            {
                return ParseRuleGroup(value, serializer);
            }
            return ParseRule(value, serializer);
        }

        private IRuleGroup ParseRuleGroup(JObject value, JsonSerializer serializer)
        {
            var group = _ruleExpressionFactory.CreateRuleGroup();

            group.SelectedTopOperator = value[SelectedTopLevelOperatorJsonPropertyName].Value<string>();

            foreach(var token in value[ListJsonPropertyName].Value<JArray>().Values<JObject>())
            {
                group.List.Add(ParseObject(token, serializer));
            }

            return group;
        }

        private IRule ParseRule(JObject value, JsonSerializer serializer)
        {
            var rule = _ruleExpressionFactory.CreateRule();

            rule.SelectedEntry = value[SelectedEntryJsonPropertyName].Value<string>();
            rule.SelectedField = value[SelectedFieldJsonPropertyName].Value<string>();
            rule.SelectedOperator = value[SelectedOperatorJsonPropertyName].Value<string>();

            return rule;
        }
    }
}
