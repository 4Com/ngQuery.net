using ngQuery.Net.Models;

namespace ngQuery.Net.JsonParsing
{
    internal interface IRuleExpressionFactory
    {
        IRule CreateRule();
        IRuleGroup CreateRuleGroup();
    }
}
