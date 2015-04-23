using FluentAssertions;
using ngQuery.Net.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ngQuery.Net.UnitTests
{
    [TestFixture]
    public class ExpressionParserTests
    { 
        public class Constructor
        {
            [Test]
            public void WhenCalledThenDoNotThrowException()
            {
                Action act = () => new ExpressionParser();

                act.ShouldNotThrow();
            }
        }

        public class GenerateExpression
        {
            [TestCase(null)]
            [TestCase("")]
            [TestCase("  ")]
            public void WhenJsonIsNullOrWhitespaceThenThrowArgumentNullException(string param)
            {
                var parser = new ExpressionParser();

                Action act = () => parser.GenerateExpression<TestClass>(param);

                act.ShouldThrow<ArgumentNullException>();
            }

            [Test]
            public void WhenRuleExpressionIsNullThenThrowArgumentNullException()
            {
                var parser = new ExpressionParser();

                Action act = () => parser.GenerateExpression<TestClass>(default(IRuleExpression));

                act.ShouldThrow<ArgumentNullException>();
            }

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
                var parser = new ExpressionParser();

                // Act
                var expression = parser.GenerateExpression<TestClass>(testJson);

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
                var parser = new ExpressionParser();

                // Act
                var expression = parser.GenerateExpression<TestClass>(testJson);

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
                var parser = new ExpressionParser();

                // Act
                var expression = parser.GenerateExpression<TestClass>(testJson);

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
                var parser = new ExpressionParser();

                // Act
                var expression = parser.GenerateExpression<TestClass>(testJson);

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
                var parser = new ExpressionParser();

                // Act
                var expression = parser.GenerateExpression<TestClass>(testJson);

                // Assert
                expression.Should().NotBeNull();
                testList.AsQueryable().Where(expression).Should().HaveCount(2);
            }
        }
    }
}
