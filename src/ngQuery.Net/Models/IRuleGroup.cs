using System.Collections.Generic;

namespace ngQuery.Net.Models
{
    internal interface IRuleGroup : IRuleExpression
    {
        string SelectedTopOperator { get; set; }
        IList<IRuleExpression> List { get; }
    }
}
