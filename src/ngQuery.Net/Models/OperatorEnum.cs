using System;
using System.ComponentModel;

namespace ngQuery.Net.Models
{
    [Flags]
    public enum OperatorEnum
    {
        [Description("AND"), TopLevelOperator]
        And,
        [Description("OR"), TopLevelOperator]
        Or,
        [Description("==")]
        Equals,
        [Description("!=")]
        NotEquals,
        [Description(">")]
        GreaterThan,
        [Description(">=")]
        GreaterThanOrEqualTo,
        [Description("<")]
        LessThan,
        [Description("<=")]
        LessThanOrEqualTo,
        [Description("IN")]
        In,
        [Description("NOT IN")]
        NotIn
    }
}
