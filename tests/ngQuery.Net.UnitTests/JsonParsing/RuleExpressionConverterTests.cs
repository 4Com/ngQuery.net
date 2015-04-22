using FluentAssertions;
using Newtonsoft.Json;
using ngQuery.Net.JsonParsing;
using ngQuery.Net.Models;
using NUnit.Framework;
using System;

namespace ngQuery.Net.UnitTests
{
    [TestFixture]
    public class RuleExpressionConverterTests
    {
        public class Constructor
        {
            [Test]
            public void WithNoConstructorArgumentsShouldNotThrowExpection()
            {
                Action act = () => new RuleExpressionConverter();

                act.ShouldNotThrow();
            }

            [Test]
            public void WhenRuleExpressionFactoryIsNullThenThrowArgumentNullException()
            {
                Action act = () => new RuleExpressionConverter(null);

                act.ShouldThrow<ArgumentNullException>();
            }

            [Test]
            public void WhenRuleExpressionFactoryIsValidThenNoExceptionIsThrown()
            {
                Action act = () => new RuleExpressionConverter(new DefaultRuleExpressionFactory());

                act.ShouldNotThrow();
            }
        }

        public class ReadJson
        {
            private const string jsonRuleGroup = "{ selectedTopLevelOperator: \"AND\", list: [{selectedEntry: 1, selectedField: \"NoHandsets\", selectedOperator: \"greaterthan\"},{selectedTopLevelOperator: \"OR\",list: [{ selectedEntry: \"Ben\", selectedField: \"CFirstName\", selectedOperator: \"equals\"}, {selectedEntry: \"Stu\",selectedField: \"CFirstName\",selectedOperator: \"equals\"}]},{selectedEntry: 2,selectedField: \"NoHandsets\",selectedOperator: \"notequal\"}]}";
            private const string jsonRule = "{selectedEntry: \"Ben\",selectedField: \"CFirstName\",selectedOperator: \"equals\"}";

            [Test]
            public void WhenJsonIsARuleGroupThenReturnedExpressionShouldBeARuleGroup()
            {
                var parser = new RuleExpressionConverter();

                var result = JsonConvert.DeserializeObject<IRuleExpression>(jsonRuleGroup, new RuleExpressionConverter());

                result.Should().BeAssignableTo<IRuleGroup>();

                var ruleGroup = (IRuleGroup)result;
                ruleGroup.SelectedTopOperator.Should().Be("AND");
                ruleGroup.List.Should().HaveCount(3);
                ruleGroup.List[0].Should().BeAssignableTo<IRule>();
                ruleGroup.List[1].Should().BeAssignableTo<IRuleGroup>();
                ruleGroup.List[2].Should().BeAssignableTo<IRule>();
            }

            [Test]
            public void WhenJsonIsARuleThenReturnedExpressionShouldBeARule()
            {
                var parser = new RuleExpressionConverter();

                var result = JsonConvert.DeserializeObject<IRuleExpression>(jsonRule, new RuleExpressionConverter());

                result.Should().BeAssignableTo<IRule>();

                var rule = (IRule)result;
                rule.SelectedEntry.Should().Be("Ben");
                rule.SelectedField.Should().Be("CFirstName");
                rule.SelectedOperator.Should().Be("equals");
            }
        }
    }
}
