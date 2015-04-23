using System;

namespace ngQuery.Net.Models
{
    [Flags]
    public enum OperatorEnum
    {
        And,
        Or,
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        In,
        NotIn
    }
}
