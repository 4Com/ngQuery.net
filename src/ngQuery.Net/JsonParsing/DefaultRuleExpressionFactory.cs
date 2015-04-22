using ngQuery.Net.Models;

namespace ngQuery.Net.JsonParsing
{
    internal class DefaultRuleExpressionFactory : IRuleExpressionFactory
    {
        public IRule CreateRule()
        {
            return new Rule();
        }

        public IRuleGroup CreateRuleGroup()
        {
            return new RuleGroup();
        }
    }
}
