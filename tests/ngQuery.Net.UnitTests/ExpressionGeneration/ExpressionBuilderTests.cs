using FluentAssertions;
using Newtonsoft.Json;
using ngQuery.Net.JsonParsing;
using ngQuery.Net.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ngQuery.Net.ExpressionGeneration.UnitTests
{
    [TestFixture]
    public class ExpressionBuilderTests
    {
        public class Constructor
        {
            [Test]
            public void DefaultShouldNotThrowAnyException()
            {
                Action act = () => new ExpressionBuilder();

                act.ShouldNotThrow();
            }

            [Test]
            public void WhenOperatorParserIsNullThrowsArgumentNullException()
            {
                Action act = () => new ExpressionBuilder(null);

                act.ShouldThrow<ArgumentNullException>();
            }
        }

        public class BuildExpression
        {
            [Test]
            public void WhenPassedSimpleEqualsRuleThenReturnsASingleElementFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: \"1\", selectedField: \"Property1\", selectedOperator: \"Equals\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property1 = "1", Property2 = "2", Property3 = "3" },
                    new TestClass { Property1 = "2", Property2 = "3", Property3 = "4" },
                    new TestClass { Property1 = "3", Property2 = "4", Property3 = "5" },
                    new TestClass { Property1 = "4", Property2 = "4", Property3 = "4" }
                };
                var ruleExpression = JsonConvert.DeserializeObject<IRuleExpression>(testJson, new RuleExpressionConverter());
                var builder = new ExpressionBuilder();

                // Act
                var expression = builder.BuildExpression<TestClass>(ruleExpression);

                // Assert
                expression.Should().NotBeNull();
                testList.AsQueryable().Where(expression).Should().HaveCount(1);
            }

            [Test]
            public void WhenPassedSimpleEqualsRuleThenExpressionIsGeneratedAndReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: \"4\", selectedField: \"Property2\", selectedOperator: \"Equals\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property1 = "1", Property2 = "2", Property3 = "3" },
                    new TestClass { Property1 = "2", Property2 = "3", Property3 = "4" },
                    new TestClass { Property1 = "3", Property2 = "4", Property3 = "5" },
                    new TestClass { Property1 = "4", Property2 = "4", Property3 = "4" }
                };
                var ruleExpression = JsonConvert.DeserializeObject<IRuleExpression>(testJson, new RuleExpressionConverter());
                var builder = new ExpressionBuilder();

                // Act
                var expression = builder.BuildExpression<TestClass>(ruleExpression);

                // Assert
                expression.Should().NotBeNull();
                testList.AsQueryable().Where(expression).Should().HaveCount(2);
            }

            [Test]
            public void WhenPassedSimpleNotEqualsRuleThenReturnsThreeElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: \"1\", selectedField: \"Property1\", selectedOperator: \"NotEquals\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property1 = "1", Property2 = "2", Property3 = "3" },
                    new TestClass { Property1 = "2", Property2 = "3", Property3 = "4" },
                    new TestClass { Property1 = "3", Property2 = "4", Property3 = "5" },
                    new TestClass { Property1 = "4", Property2 = "4", Property3 = "4" }
                };
                var ruleExpression = JsonConvert.DeserializeObject<IRuleExpression>(testJson, new RuleExpressionConverter());
                var builder = new ExpressionBuilder();

                // Act
                var expression = builder.BuildExpression<TestClass>(ruleExpression);

                // Assert
                expression.Should().NotBeNull();
                testList.AsQueryable().Where(expression).Should().HaveCount(3);
            }

            [Test]
            public void WhenPassedSimpleRuleGroupUsingTheAndOperatorThenReturnsASingleElementFromAList()
            {
                // Arrange
                const string testJson = "{ selectedTopLevelOperator: \"And\", list: [{ selectedEntry: \"4\", selectedField: \"Property1\", selectedOperator: \"Equals\" }, { selectedEntry: \"4\", selectedField: \"Property2\", selectedOperator: \"Equals\" }] }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property1 = "1", Property2 = "2", Property3 = "3" },
                    new TestClass { Property1 = "2", Property2 = "3", Property3 = "4" },
                    new TestClass { Property1 = "3", Property2 = "4", Property3 = "5" },
                    new TestClass { Property1 = "4", Property2 = "4", Property3 = "4" }
                };
                var ruleExpression = JsonConvert.DeserializeObject<IRuleExpression>(testJson, new RuleExpressionConverter());
                var builder = new ExpressionBuilder();

                // Act
                var expression = builder.BuildExpression<TestClass>(ruleExpression);

                // Assert
                expression.Should().NotBeNull();
                testList.AsQueryable().Where(expression).Should().HaveCount(1);
            }

            [Test]
            public void WhenPassedSimpleRuleGroupUsingTheOrOperatorThenReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedTopLevelOperator: \"Or\", list: [{ selectedEntry: \"4\", selectedField: \"Property1\", selectedOperator: \"Equals\" }, { selectedEntry: \"4\", selectedField: \"Property2\", selectedOperator: \"Equals\" }] }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property1 = "1", Property2 = "2", Property3 = "3" },
                    new TestClass { Property1 = "2", Property2 = "3", Property3 = "4" },
                    new TestClass { Property1 = "3", Property2 = "4", Property3 = "5" },
                    new TestClass { Property1 = "4", Property2 = "4", Property3 = "4" }
                };
                var ruleExpression = JsonConvert.DeserializeObject<IRuleExpression>(testJson, new RuleExpressionConverter());
                var builder = new ExpressionBuilder();

                // Act
                var expression = builder.BuildExpression<TestClass>(ruleExpression);

                // Assert
                expression.Should().NotBeNull();
                testList.AsQueryable().Where(expression).Should().HaveCount(2);
            }
        }


        public class TestClass
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public string Property3 { get; set; }
        }
    }
}
