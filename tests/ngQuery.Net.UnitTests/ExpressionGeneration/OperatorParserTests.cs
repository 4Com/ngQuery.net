using FluentAssertions;
using ngQuery.Net.Models;
using NUnit.Framework;

namespace ngQuery.Net.ExpressionGeneration.UnitTests
{
    [TestFixture]
    public class OperatorParserTests
    {
        public class Parse
        {
            [TestCase("Equals", OperatorEnum.Equals)]
            [TestCase("NotEquals", OperatorEnum.NotEquals)]
            [TestCase("GreaterThan", OperatorEnum.GreaterThan)]
            [TestCase("GreaterThanOrEqualTo", OperatorEnum.GreaterThanOrEqualTo)]
            [TestCase("LessThan", OperatorEnum.LessThan)]
            [TestCase("LessThanOrEqualTo", OperatorEnum.LessThanOrEqualTo)]
            [TestCase("In", OperatorEnum.In)]
            [TestCase("NotIn", OperatorEnum.NotIn)]
            public void WhenValueIsSuppliedThenCorrectResponseReturned(string operation, OperatorEnum expectedResult)
            {
                var operatorParser = new OperatorParser();

                operatorParser.Parse(operation).Should().Be(expectedResult);
            }

        }
    }
}
