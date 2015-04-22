using System.Collections.Generic;

namespace ngQuery.Net.Models
{
    internal class RuleGroup : IRuleGroup
    {
        public IList<IRuleExpression> List { get; } = new List<IRuleExpression>();

        public string SelectedTopOperator { get; set; }
    }
}
