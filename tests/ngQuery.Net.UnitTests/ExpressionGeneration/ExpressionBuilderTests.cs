using FluentAssertions;
using Newtonsoft.Json;
using ngQuery.Net.JsonParsing;
using ngQuery.Net.Models;
using ngQuery.Net.UnitTests;
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

            [Test]
            public void WhenPassedSimpleRuleUsingTheGreaterThanOperatorThenReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: 2, selectedField: \"Property4\", selectedOperator: \"GreaterThan\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property4 = 1 },
                    new TestClass { Property4 = 2 },
                    new TestClass { Property4 = 3 },
                    new TestClass { Property4 = 4 }
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
            public void WhenPassedSimpleRuleUsingTheGreaterThanOrEqualToOperatorThenReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: 2, selectedField: \"Property4\", selectedOperator: \"GreaterThanOrEqualTo\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property4 = 1 },
                    new TestClass { Property4 = 2 },
                    new TestClass { Property4 = 3 },
                    new TestClass { Property4 = 4 }
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
            public void WhenPassedSimpleRuleUsingTheLessThanOperatorThenReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: 3, selectedField: \"Property4\", selectedOperator: \"LessThan\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property4 = 1 },
                    new TestClass { Property4 = 2 },
                    new TestClass { Property4 = 3 },
                    new TestClass { Property4 = 4 }
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
            public void WhenPassedSimpleRuleUsingTheLessThanOrEqualToOperatorThenReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: 3, selectedField: \"Property4\", selectedOperator: \"LessThanOrEqualTo\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property4 = 1 },
                    new TestClass { Property4 = 2 },
                    new TestClass { Property4 = 3 },
                    new TestClass { Property4 = 4 }
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
            public void WhenPassedSimpleRuleUsingTheInOperatorThenReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: \"1,2,3\", selectedField: \"Property4\", selectedOperator: \"In\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property4 = 1 },
                    new TestClass { Property4 = 2 },
                    new TestClass { Property4 = 3 },
                    new TestClass { Property4 = 4 }
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
            public void WhenPassedSimpleRuleUsingTheNotInOperatorThenReturnsTwoElementsFromAList()
            {
                // Arrange
                const string testJson = "{ selectedEntry: \"1,2,3\", selectedField: \"Property4\", selectedOperator: \"NotIn\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property4 = 1 },
                    new TestClass { Property4 = 2 },
                    new TestClass { Property4 = 3 },
                    new TestClass { Property4 = 4 }
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
            public void WhenPassedSimpleRuleUsingTheGreaterThanOperatorWithTheSelectedPropertyBeingADateTimeTypeThenReturnsTwoElementsFromAList()
            {
                // Arrange
                var testJson = "{ selectedEntry: \"" + DateTime.UtcNow.ToString("o") + "\", selectedField: \"Property5\", selectedOperator: \"GreaterThan\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(-2) },
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(-1) },
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(1) },
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(2) }
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
            public void WhenPassedSimpleRuleUsingTheLessThanOperatorWithTheSelectedPropertyBeingADateTimeTypeThenReturnsTwoElementsFromAList()
            {
                // Arrange
                var testJson = "{ selectedEntry: \"" + DateTime.UtcNow.ToString("o") + "\", selectedField: \"Property5\", selectedOperator: \"LessThan\" }";
                var testList = new List<TestClass>
                {
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(-2) },
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(-1) },
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(1) },
                    new TestClass { Property5 = DateTime.UtcNow.AddHours(2) }
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
    }
}
